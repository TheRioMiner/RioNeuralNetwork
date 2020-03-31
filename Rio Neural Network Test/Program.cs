//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using RioNeuralNetwork;

namespace Rio_Neural_Network_Test
{
    static class Program
    {
        static float[][] desiredOutputs = new float[][]
        {
            new float[] { 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, //0
			new float[] { 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, //1
			new float[] { 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }, //2
			new float[] { 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f }, //3
			new float[] { 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f }, //4
			new float[] { 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f }, //5
			new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f }, //6
			new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 0f }, //7
			new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f }, //8
			new float[] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f }, //9
        };

        static LayerCfg[] layersCfg = new LayerCfg[]
        {
            new LayerCfg(256, "sigmoid"),
            new LayerCfg(256, "tanh"),
            new LayerCfg(256, "tanh"),
            new LayerCfg(256, "tanh"),
            new LayerCfg(10, "sigmoid"),
        };

        const int seed = 2221;


        static List<Example> trainDataset;
        static List<Example> testDataset;



        static void Main(string[] args)
        {
            Console.WriteLine("Choose mode - \"test\" or \"train\"\n");
            string mode = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (mode == "train")
            {
                //Load samples
                Console.WriteLine($"Loading samples...");
                trainDataset = LoadSamplesFromDir("train");
                testDataset = LoadSamplesFromDir("test");

                //Write samples amount
                Console.WriteLine($"{trainDataset.Count} train samples loaded");
                Console.WriteLine($"{testDataset.Count} test samples loaded\n");

                //Create neural network
                var network = new NeuralNetwork(layersCfg, new WeightsFillInfo(0.8f), seed);

                //Load neural network from file to resume training
                //var network = NeuralNetwork.LoadFromBinary("trained.bin");

                //Set learn rate and learn until error
                network.LearnInfo.LearnRate = 0.0085f;
                network.LearnInfo.LearnUntilError = 25.0f;
                network.LearnInfo.Alpha = 5f;

                //Train!
                Train(network, seed);

                //Training finished, save neural network to file
                network.SaveToBinary("trained.bin");

                //Write training info
                Console.WriteLine($"\nTraining finished in {network.LearnInfo.Epochs} epochs!");
                Console.WriteLine($"Reached train error: {Math.Round(network.LearnInfo.ErrorPerEpoch, 4)}");
                Console.WriteLine($"Reached test error: {Math.Round(ComputeTestDatasetError(network), 4)}\n");

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            else if (mode == "test")
            {
                if (!File.Exists("trained.bin"))
                {
                    Console.WriteLine("No file of trained neural network!");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }

                if (!File.Exists("test.png"))
                {
                    Console.WriteLine("No file of test image!");
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }

                //Load trained network
                var network = NeuralNetwork.LoadFromBinary("trained.bin");


            Retry:

                //Load image
                float[] imageData = NeuralUtils.ConvertBitmapToFloatArrayGrayscale("test.png");

                //Forward propagate
                var output = network.ForwardPropagate(imageData);

                //Show results
                Console.WriteLine("I think its is: ");
                for (int i = 0; i < output.Length; i++)
                    Console.WriteLine($"{i} - {Math.Round(output[i] * 100f, 3)}%");
                Console.WriteLine("");

                Console.WriteLine("Do you want test again? (y/n)");
                var key = ConsoleKey.Escape;
                while ((key = Console.ReadKey().Key) != ConsoleKey.Y && key != ConsoleKey.N) { } //Wait valid key

                //If yes, retry
                if (key == ConsoleKey.Y)
                {
                    Console.WriteLine("\n");
                    goto Retry;
                }
            }
            else
            {
                Console.WriteLine("Invalid mode!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }


        private static void Train(NeuralNetwork network, int seed)
        {
            //Show chart
            var chartForm = new Chart_Form(network);
            chartForm.Show();

            var random = new Random(seed);
            var lastTrainErrorPerEpoch = 0f;
            var lastTestErrorPerEpoch = network.LearnInfo.LearnUntilError;
            while (lastTestErrorPerEpoch >= network.LearnInfo.LearnUntilError)
            {
                var example = trainDataset[(int)network.LearnInfo.ExampleIndex];
                var output = network.ForwardPropagate(example.Input);
                network.BackwardPropagateError(example.DesiredResult);
                network.UpdateWeights(example.Input);

                //Calc mean square error per example and increase error per epoch
                network.LearnInfo.ErrorPerExample = NeuralUtils.CalcRootMeanSquaredError(example.DesiredResult, output);
                lastTrainErrorPerEpoch += network.LearnInfo.ErrorPerExample;

                //Call DoEvents sometime to fix form freezes
                if (network.LearnInfo.ExampleIndex % 30 == 0)
                    Application.DoEvents();

                //Last example?
                if (++network.LearnInfo.ExampleIndex == trainDataset.Count)
                {
                    //Write epoch info
                    string trainErrorStr = Math.Round(lastTrainErrorPerEpoch, 5).ToString();
                    var testError = ComputeTestDatasetError(network);
                    Console.WriteLine($"Epoch: {network.LearnInfo.Epochs} - TrainError: {trainErrorStr}, TestError: {Math.Round(testError, 5)}");

                    //Shuffle the train datasets
                    trainDataset.Shuffle(random);

                    //Update charts
                    chartForm.TrainErrorPerPoch.Add(lastTrainErrorPerEpoch);
                    chartForm.TestErrorPerPoch.Add(testError);
                    chartForm.RefreshChart();

                    //Prepare for next epoch
                    lastTestErrorPerEpoch = testError;
                    network.LearnInfo.ErrorPerEpoch = lastTrainErrorPerEpoch;
                    network.LearnInfo.ExampleIndex = 0;
                    network.LearnInfo.Epochs++;
                    lastTrainErrorPerEpoch = 0;
                }
            }
        }

        private static unsafe float ComputeTestDatasetError(NeuralNetwork network)
        {
            float testError = 0f;
            foreach (var example in testDataset)
            {
                //Just forward propagate
                float* outputPtr = network.ForwardPropagatePtr(example.Input);

                //Calc mean square error
                testError += NeuralUtils.CalcRootMeanSquaredError(example.DesiredResult, outputPtr);
            }
            return testError;
        }


        private static List<Example> LoadSamplesFromDir(string dirPath)
        {
            List<Example> examplesList = new List<Example>();
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (!dirInfo.Exists)
                throw new DirectoryNotFoundException($"Directory: \"{dirPath}\" - not found!");

            foreach (var dir in dirInfo.GetDirectories())
            {
                var dirInfoSub = new DirectoryInfo(dir.FullName);
                string numStr = Path.GetFileNameWithoutExtension(dirInfoSub.Name);
                int index = int.Parse(numStr);
                foreach (var file in dirInfoSub.GetFiles())
                {
                    using (var bitmap = new Bitmap(file.FullName))
                    using (var resizedBitmap = bitmap.ResizeImage(16, 16))
                    {
                        var floatArray = NeuralUtils.ConvertBitmapToFloatArrayGrayscale(resizedBitmap);
                        var example = new Example(floatArray, desiredOutputs[index]);
                        examplesList.Add(example);
                    }
                }
            }

            return examplesList;
        }
    }
}
