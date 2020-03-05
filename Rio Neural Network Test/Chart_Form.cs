//RioNeuralNetwork: License information is available here - "https://github.com/TheRioMiner/RioNeuralNetwork/blob/master/LICENSE" or in file "LICENCE"

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using RioNeuralNetwork;

namespace Rio_Neural_Network_Test
{
    public partial class Chart_Form : Form
    {
        private NeuralNetwork _network;
        public List<float> TrainErrorPerPoch = new List<float>();
        public List<float> TestErrorPerPoch = new List<float>();

        public Chart_Form(NeuralNetwork network)
        {
            this._network = network;
            InitializeComponent();
            learnRate_numericUpDown.Value = (decimal)network.LearnInfo.LearnRate;
            learnUntil_numericUpDown.Value = (decimal)network.LearnInfo.LearnUntilError;
            myChart.ChartAreas.Clear();
            myChart.ChartAreas.Add(new ChartArea("Chart"));
        }

        public void RefreshChart()
        {
            float upperLimit = (float)upperLimit_numericUpDown.Value;

            //Train examples error
            var trainErrorPerEpochSeries = new Series("Train - Error per epoch (dot)");
            var trainErrorPerEpochChangeSpeedSeries = new Series("Train - Error per epoch (line)");
            {
                //ErrorPerEpoch
                trainErrorPerEpochSeries.ChartType = SeriesChartType.Point;
                trainErrorPerEpochSeries.Color = System.Drawing.Color.BlueViolet;
                trainErrorPerEpochSeries.MarkerSize = 4;
                trainErrorPerEpochSeries.ChartArea = "Chart";

                //ErrorPerEpochChangeSpeed
                trainErrorPerEpochChangeSpeedSeries.ChartType = SeriesChartType.FastLine;
                trainErrorPerEpochChangeSpeedSeries.Color = System.Drawing.Color.Yellow;
                trainErrorPerEpochChangeSpeedSeries.MarkerSize = 1;
                trainErrorPerEpochChangeSpeedSeries.ChartArea = "Chart";

                //Fill
                for (int i = 0; i < TrainErrorPerPoch.Count; i++)
                {
                    float err = TrainErrorPerPoch[i];
                    if (err <= upperLimit)
                    {
                        trainErrorPerEpochSeries.Points.AddXY(i, err);
                        trainErrorPerEpochChangeSpeedSeries.Points.AddXY(i, err);
                    }
                }
            }

            //Test examples error
            var testErrorPerEpochSeries = new Series("Test - Error per epoch (dot)");
            var testErrorPerEpochChangeSpeedSeries = new Series("Test - Error per epoch (line)");
            {
                //ErrorPerEpoch
                testErrorPerEpochSeries.ChartType = SeriesChartType.Point;
                testErrorPerEpochSeries.Color = System.Drawing.Color.BlueViolet;
                testErrorPerEpochSeries.MarkerSize = 4;
                testErrorPerEpochSeries.ChartArea = "Chart";

                //ErrorPerEpochChangeSpeed
                testErrorPerEpochChangeSpeedSeries.ChartType = SeriesChartType.FastLine;
                testErrorPerEpochChangeSpeedSeries.Color = System.Drawing.Color.Red;
                testErrorPerEpochChangeSpeedSeries.MarkerSize = 1;
                testErrorPerEpochChangeSpeedSeries.ChartArea = "Chart";

                //Fill
                for (int i = 0; i < TestErrorPerPoch.Count; i++)
                {
                    float err = TestErrorPerPoch[i];
                    if (err <= upperLimit)
                    {
                        testErrorPerEpochSeries.Points.AddXY(i, err);
                        testErrorPerEpochChangeSpeedSeries.Points.AddXY(i, err);
                    }
                }
            }

            myChart.Series.Clear();
            myChart.Series.Add(trainErrorPerEpochSeries);
            myChart.Series.Add(trainErrorPerEpochChangeSpeedSeries);
            myChart.Series.Add(testErrorPerEpochSeries);
            myChart.Series.Add(testErrorPerEpochChangeSpeedSeries);
        }

        private void learnRate_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._network.LearnInfo.LearnRate = (float)learnRate_numericUpDown.Value;
        }

        private void learnUntil_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this._network.LearnInfo.LearnUntilError = (float)learnUntil_numericUpDown.Value;
        }

        private void upperLimit_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            RefreshChart();
        }

        private void saveNetworkCfg_button_Click(object sender, EventArgs e)
        {
            this._network.SaveToBinary("trained.bin");
        }
    }
}
