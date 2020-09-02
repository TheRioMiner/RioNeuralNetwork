//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace RioNeuralNetwork
{
    public unsafe class NeuralNetwork : IDisposable
    {
        /// <summary>
        /// Native instance pointer
        /// </summary>
        public IntPtr InstancePtr { get; private set; } = IntPtr.Zero;

        /// <summary>
        /// Get pointer to list of layers. (Sample of use: LayersPtr[index]->LearnRate = 0.5f;)
        /// </summary>
        public Layer** LayersPtr
        {
            get
            {
                CheckInstance();
                return *(Layer***)(InstancePtr + IntPtr.Size);
            }
        }


        /// <summary>
        /// Learn info, can be saved into file to continue learning after closing program
        /// </summary>
        public LearnInfo LearnInfo = new LearnInfo(0.1f, 0.9f);


        /// <summary>
        /// Check that instance is not disposed
        /// </summary>
        private void CheckInstance()
        {
            //Instance initializated?
            if (InstancePtr == IntPtr.Zero)
                throw new ObjectDisposedException("NeuralNetwork", "NeuralNetwork native instance is null!");
        }


        /// <summary>
        /// Get layer by index
        /// </summary>
        /// <param name="index">Index of layer</param>
        /// <returns>Managed layer structure</returns>
        public Layer this[int index]
        {
            get
            {
                //Check that index in range
                if (index >= LayersCount)
                    throw new ArgumentOutOfRangeException("index", "Index out of range!");

                //Read layer
                return *LayersPtr[index];
            }
        }

        /// <summary>
        /// Get all layers in neural network
        /// If you need get not all layers or just 1 you can just use indexation like that - "Layer l = network[0];"
        /// </summary>
        /// <returns></returns>
        public Layer[] GetLayers()
        {
            var layersPtr = LayersPtr;
            var layers = new Layer[LayersCount];
            for (int i = 0; i < layers.Length; i++)
                layers[i] = *layersPtr[i];
            return layers;
        }


        /// <summary>
        /// Neural network layers count
        /// </summary>
        public int LayersCount
        {
            get
            {
                CheckInstance(); //Check that instance is exists
                return *(int*)(InstancePtr); //Read layers count from instance
            }
        }

        /// <summary>
        /// Neural network input neurons count
        /// </summary>
        public int InputNeuronsCount => LayersPtr[0]->NeuronsCount; //Get neurons count from first layer

        /// <summary>
        /// Neural network output neurons count
        /// </summary>
        public int OutputNeuronsCount => LayersPtr[this.LayersCount - 1]->NeuronsCount; //Get neurons count from last layer



        internal NeuralNetwork(LayerCfg[] layersCfg, bool fullyCopyLayersCfg)
        {
            //Need init neurons weights size?
            if (!fullyCopyLayersCfg)
            {
                layersCfg[0].NeuronsWeightsSize = layersCfg[0].NeuronsCount;
                for (int i = 1; i < layersCfg.Length; i++)
                    layersCfg[i].NeuronsWeightsSize = layersCfg[i - 1].NeuronsCount;
            }

            //Create instance of neural network
            fixed (LayerCfg* layerCfgArrPtr = &layersCfg[0])
            {
                InstancePtr = Native.CreateInstance((IntPtr)layerCfgArrPtr, layersCfg.Length);
                if (InstancePtr == IntPtr.Zero)
                    throw new ArgumentException("Can't initializate - \"NeuralNetwork\"!");
            }
        }

        /// <summary>
        /// Initializate neural network without initialization weights (weights initializate by default to zeroes)
        /// </summary>
        /// <param name="layersCfg">Layers configuration</param>
        public NeuralNetwork(LayerCfg[] layersCfg) : this(layersCfg, false)
        { }

        /// <summary>
        /// Initializate neural network with initialization all weights by desired configuration
        /// </summary>
        /// <param name="layersCfg">Layers configuration</param>
        /// <param name="weightsFillInfo">Weights configuration</param>
        /// <param name="randomSeed">Seed of random that generate weights values</param>
        public NeuralNetwork(LayerCfg[] layersCfg, WeightsFillInfo weightsFillInfo, int randomSeed) : this(layersCfg)
        {
            this.FillWeights(weightsFillInfo, randomSeed);
        }


        /// <summary>
        /// Set desired threading mode to all neural network layers
        /// </summary>
        /// <param name="layersThreadingMode">Layers threading mode</param>
        public void SetAllLayersThreadingMode(ThreadingMode layersThreadingMode)
        {
            var layersPtr = LayersPtr;
            var layersCount = LayersCount;
            for (int l = 0; l < layersCount; l++)
                layersPtr[l]->ThreadingMode = layersThreadingMode;
        }

        /// <summary>
        ///  Set desired learning rate to all neural network layers
        /// </summary>
        /// <param name="layersLearnRate">Layers learning rate</param>
        public void SetAllLayersLearningRate(float layersLearnRate)
        {
            var layersPtr = LayersPtr;
            var layersCount = LayersCount;
            for (int l = 0; l < layersCount; l++)
                layersPtr[l]->LearnRate = layersLearnRate;
        }


        /// <summary>
        /// Fill all layers weights by desired configuration
        /// </summary>
        /// <param name="weightsFillInfo">Weights configuration</param>
        /// <param name="randomSeed">Seed of random that generate weights values</param>
        public void FillWeights(WeightsFillInfo weightsFillInfo, int randomSeed)
        {
            //Instance initializated?
            CheckInstance();

            for (int l = 0; l < LayersCount; l++)
            {
                var layer = this[l];
                for (int i = 0; i < layer.NeuronsCount; i++)
                {
                    //Fill weights by native function
                    int seed = (randomSeed + i) * l;
                    Native.FloatArrayRandomFill(layer.Weights[i], layer.NeuronsWeightsSize, weightsFillInfo.CoefficientOfWeights, weightsFillInfo.NegativeWeights, seed, ThreadingMode.SingleThread);
                }
            }
        }

        /// <summary>
        /// Fill layers weights by desired configurations
        /// </summary>
        /// <param name="weightsFillInfo">Layers weights configurations</param>
        /// <param name="randomSeed">Seed of random that generate weights values</param>
        public void FillWeights(WeightsFillInfo[] weightsFillInfo, int randomSeed)
        {
            //Instance initializated?
            CheckInstance();

            //Check layers count
            if (weightsFillInfo.Length != LayersCount)
                throw new ArgumentException();

            for (int l = 0; l < LayersCount; l++)
            {
                var layer = this[l];
                var layerFillInfo = weightsFillInfo[l];
                for (int i = 0; i < layer.NeuronsCount; i++)
                {
                    //Fill weights by native function
                    int seed = (randomSeed + i) * l;
                    Native.FloatArrayRandomFill(layer.Weights[i], layer.NeuronsWeightsSize, layerFillInfo.CoefficientOfWeights, layerFillInfo.NegativeWeights, seed, ThreadingMode.SingleThread);
                }
            }
        }



        public float* ForwardPropagatePtr(float* inputArrayPtr, int inputArraySize, bool setInputToFirstLayerOutput = false)
        {
            //Check pointer to input array
            if (inputArrayPtr == (float*)0)
                throw new ArgumentNullException("inputArrayPtr", "Input array pointer is null!");

            //Get input neurons count
            int inputNeuronsCount = InputNeuronsCount;

            //Input size in valid?
            if (inputArraySize < inputNeuronsCount)
                throw new ArgumentOutOfRangeException("inputArraySize", "Input array size too small for neural network input!");

            //Forward propagate!
            return Native.ForwardPropagate_Ptr(InstancePtr, inputArrayPtr, inputNeuronsCount, setInputToFirstLayerOutput);
        }

        public float* ForwardPropagatePtr(float[] input, bool setInputToFirstLayerOutput = false)
        {
            //Input not null?
            if (input == null)
                throw new ArgumentNullException("input", "Input array is null!");

            //Get input neurons count
            int inputNeuronsCount = InputNeuronsCount;

            //Check that input size is valid
            if (input.Length < inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too small for neural network input neurons!");
            if (input.Length > inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too big for neural network input neurons!");

            //Fixate arrays
            fixed (float* inputPtr = &input[0])
            {
                //Forward propagate!
                return Native.ForwardPropagate_Ptr(InstancePtr, inputPtr, inputNeuronsCount, setInputToFirstLayerOutput);
            }
        }

        public float[] ForwardPropagate(float[] input, bool setInputToFirstLayerOutput = false)
        {
            //Input not null?
            if (input == null)
                throw new ArgumentNullException("input", "Input array is null!");

            //Get input neurons count
            int inputNeuronsCount = InputNeuronsCount;

            //Check that input size is valid
            if (input.Length < inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too small for neural network input neurons!");
            if (input.Length > inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too big for neural network input neurons!");

            //Create output array
            float[] output = new float[OutputNeuronsCount];

            //Fixate arrays
            fixed (float* inputPtr = &input[0])
            fixed (float* outputPtr = &output[0])
            {
                //Forward propagate!
                bool result = Native.ForwardPropagate_Cpy(InstancePtr, inputPtr, inputNeuronsCount, outputPtr, setInputToFirstLayerOutput);
                if (!result)
                    return null; //Something went wrong!
            }

            //Return result
            return output;
        }

        public bool BackwardPropagateError(float[] expected)
        {
            //Get output neurons count
            int outputNeuronsCount = OutputNeuronsCount;

            //Check expected array size
            if (expected.Length < outputNeuronsCount)
                throw new ArgumentException("expected", "Expected array is too small for neural network output neurons!");
            if (expected.Length > outputNeuronsCount)
                throw new ArgumentException("expected", "Expected array is too big for neural network output neurons!");

            //Fixate input array
            fixed (float* expectedPtr = &expected[0])
            {
                //Backward propagate error!
                return Native.BackwardPropagateError(InstancePtr, expectedPtr, outputNeuronsCount);
            }
        }

        public bool UpdateWeights(float[] input, float learnRate, float alpha = 0.9f)
        {
            //Get input neurons count
            int inputNeuronsCount = InputNeuronsCount;

            //Check that input size is valid
            if (input.Length < inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too small for neural network input neurons!");
            if (input.Length > inputNeuronsCount)
                throw new ArgumentException("input", "Input array is too big for neural network input neurons!");

            //Fixate input array
            fixed (float* inputPtr = &input[0])
            {
                //Update weigths!
                return Native.UpdateWeights(InstancePtr, inputPtr, learnRate, alpha);
            }
        }

        public bool UpdateWeights(float[] input)
        {
            return this.UpdateWeights(input, LearnInfo.LearnRate, LearnInfo.Alpha);
        }



        public void SaveToBinary(string fileName, FileFlags fileFlags = FileFlags.DEFAULT)
        {
            int layersCount = LayersCount;
            bool fileExists = File.Exists(fileName);
            using (var fs = new FileStream(fileName, fileExists ? FileMode.Truncate : FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var bw = new BinaryWriter(fs, Encoding.UTF8))
            {
                //Write file signature
                bw.Write("RNN_BIN1".ToCharArray());

                //Write fileflags
                bw.Write((uint)fileFlags);

                //Write hash if its need
                if (fileFlags.HasFlag(FileFlags.Hash))
                    bw.Write(new byte[16]); //Write hash 16 bytes reserve (it will be overwritted at end of file save)

                //Write layers cfg
                {
                    bw.Write((uint)layersCount);
                    for (int l = 0; l < layersCount; l++)
                    {
                        bw.Write((uint)this[l].NeuronsCount);
                        bw.Write((float)this[l].LearnRate);
                        bw.Write((uint)this[l].ActivationType);
                        bw.Write((int)this[l].ThreadingMode);
                    }
                }

                //Write learn info if its need
                if (fileFlags.HasFlag(FileFlags.LearnInfo))
                    fs.Write(this.LearnInfo); //Save learn info

                //Write layers data
                bool saveWeights = fileFlags.HasFlag(FileFlags.Weights);
                bool saveWeightsMomentum = fileFlags.HasFlag(FileFlags.WeightsMomentum);
                bool saveOutputs = fileFlags.HasFlag(FileFlags.Output);
                bool saveErrors = fileFlags.HasFlag(FileFlags.Errors);
                if (saveWeights || saveWeightsMomentum || saveOutputs || saveErrors)
                {
                    for (int l = 0; l < layersCount; l++)
                    {
                        var layer = this[l];
                        for (int i = 0; i < layer.NeuronsCount; i++)
                        {
                            //Read weights
                            if (saveWeights || saveWeightsMomentum)
                            {
                                float* weightsPtr = layer.Weights[i];
                                float* weightsMomentumPtr = layer.WeightsMomentum[i];
                                for (int j = 0; j < layer.NeuronsWeightsSize; j++)
                                {
                                    if (saveWeights)
                                        bw.Write(weightsPtr[j]);
                                    if (saveWeightsMomentum)
                                        bw.Write(weightsMomentumPtr[j]);
                                }
                            }

                            //Read outputs and errors
                            if (saveOutputs || saveErrors)
                            {
                                for (int j = 0; j < layer.NeuronsCount; j++)
                                {
                                    if (saveOutputs)
                                        bw.Write(layer.Outputs[j]);
                                    if (saveErrors)
                                        bw.Write(layer.Errors[j]);
                                }
                            }
                        }
                    }
                }

                //End of file signature
                bw.Write("RNN_EOF!".ToCharArray());

                //Compute and write hash if its needed
                if (fileFlags.HasFlag(FileFlags.Hash))
                {
                    //Set position to start of file data
                    fs.Position = (8/*signature*/ + 4/*fileflags*/ + 16/*hash*/);

                    //Compute hash of file
                    byte[] hash = null;
                    using (var md5 = MD5.Create())
                        hash = md5.ComputeHash(fs);

                    //Set position to start of hash
                    fs.Position = (8/*signature*/ + 4/*fileflags*/);

                    //Write computed hash of file
                    bw.Write(hash);
                }
            }
        }

        public static NeuralNetwork LoadFromBinary(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs, Encoding.UTF8))
            {
                //Check signature
                string signature = new string(br.ReadChars(8));
                if (!signature.StartsWith("RNN_BIN"))
                    throw new Exception("Invalid file signature!");
                else if (!signature.EndsWith("0") && !signature.EndsWith("1"))
                    throw new Exception("File version mismatch!");

                //Read file flags
                FileFlags fileFlags = (FileFlags)br.ReadUInt32();

                //Need check hash?
                if (fileFlags.HasFlag(FileFlags.Hash))
                {
                    //Read md5 hash
                    byte[] readedHash = br.ReadBytes(16);

                    //Compute hash of file
                    byte[] computedHash = null;
                    using (var md5 = MD5.Create())
                        computedHash = md5.ComputeHash(fs);

                    //Check hashes
                    if (Native.memcmp(computedHash, readedHash, (UIntPtr)16) != 0)
                        throw new InvalidDataException("Hash of file is not match!\nFile is corrupted!");

                    //Set position to start of file data
                    fs.Position = (8/*signature*/ + 4/*fileflags*/ + 16/*hash*/);
                }

                //Get layers count
                uint layersCount = br.ReadUInt32();

                //Read LayersCfg's
                var layersCfg = new LayerCfg[layersCount];
                for (int l = 0; l < layersCount; l++)
                {
                    if (signature.EndsWith("1"))
                    {
                        var neuronsCount = (int)br.ReadUInt32();
                        var learnRate = br.ReadSingle();
                        var activationType = (ActivationType)br.ReadUInt32();
                        var threadingMode = (ThreadingMode)br.ReadInt32();
                        layersCfg[l] = new LayerCfg(neuronsCount, activationType, learnRate, threadingMode);
                    }
                    else if (signature.EndsWith("0"))
                    {
                        var activationType = (ActivationType)br.ReadUInt32();
                        var neuronsCount = (int)br.ReadUInt32();
                        layersCfg[l] = new LayerCfg(neuronsCount, activationType);
                    }
                }

                //Init neural network from readed cfg
                var neuralNetwork = new NeuralNetwork(layersCfg);

                //Need read learn info?
                if (fileFlags.HasFlag(FileFlags.LearnInfo))
                    neuralNetwork.LearnInfo = fs.Read<LearnInfo>();

                //Read layers data
                bool readWeights = fileFlags.HasFlag(FileFlags.Weights);
                bool readWeightsMomentum = fileFlags.HasFlag(FileFlags.WeightsMomentum);
                bool readOutputs = fileFlags.HasFlag(FileFlags.Output);
                bool readErrors = fileFlags.HasFlag(FileFlags.Errors);
                if (readWeights || readWeightsMomentum || readOutputs || readErrors)
                {
                    for (int l = 0; l < layersCount; l++)
                    {
                        //Reading layers data
                        var layer = neuralNetwork[l];
                        for (int i = 0; i < layer.NeuronsCount; i++)
                        {
                            if (readWeights || readWeightsMomentum)
                            {
                                float* weightsPtr = layer.Weights[i];
                                float* weightsMomentumPtr = layer.WeightsMomentum[i];
                                for (int j = 0; j < layer.NeuronsWeightsSize; j++)
                                {
                                    if (readWeights)
                                        weightsPtr[j] = br.ReadSingle();
                                    if (readWeightsMomentum)
                                        weightsMomentumPtr[j] = br.ReadSingle();
                                }
                            }

                            //Read outputs and errors
                            if (readOutputs || readErrors)
                            {
                                for (int j = 0; j < layer.NeuronsCount; j++)
                                {
                                    if (readOutputs)
                                        layer.Outputs[j] = br.ReadSingle();
                                    if (readErrors)
                                        layer.Errors[j] = br.ReadSingle();
                                }
                            }
                        }
                    }
                }

                //Check end file signature
                string endFilesignature = new string(br.ReadChars(8));
                if (endFilesignature != "RNN_EOF!")
                    throw new Exception("End of file not reached!\nSeems file is corrupted?");

                //All ok, return ready neural network instance!
                return neuralNetwork;
            }
        }



        public bool IsDisposed => (InstancePtr == IntPtr.Zero);

        ~NeuralNetwork()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (InstancePtr != IntPtr.Zero)
            {
                Native.DeleteInstance(InstancePtr);
                InstancePtr = IntPtr.Zero;
            }
        }
    }
}
