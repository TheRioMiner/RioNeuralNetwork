namespace Rio_Neural_Network_Test
{
    partial class Chart_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.myChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.upperLimit_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.panel = new System.Windows.Forms.Panel();
            this.learnUntil_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.learnRate_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.saveNetworkCfg_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.myChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimit_numericUpDown)).BeginInit();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.learnUntil_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.learnRate_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // myChart
            // 
            this.myChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            legend3.Name = "Legend1";
            this.myChart.Legends.Add(legend3);
            this.myChart.Location = new System.Drawing.Point(2, 1);
            this.myChart.Margin = new System.Windows.Forms.Padding(2);
            this.myChart.Name = "myChart";
            this.myChart.Size = new System.Drawing.Size(569, 338);
            this.myChart.TabIndex = 0;
            this.myChart.Text = "Chart";
            // 
            // upperLimit_numericUpDown
            // 
            this.upperLimit_numericUpDown.DecimalPlaces = 1;
            this.upperLimit_numericUpDown.Location = new System.Drawing.Point(61, 4);
            this.upperLimit_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.upperLimit_numericUpDown.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.upperLimit_numericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.upperLimit_numericUpDown.Name = "upperLimit_numericUpDown";
            this.upperLimit_numericUpDown.Size = new System.Drawing.Size(58, 20);
            this.upperLimit_numericUpDown.TabIndex = 1;
            this.upperLimit_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.upperLimit_numericUpDown.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.upperLimit_numericUpDown.ValueChanged += new System.EventHandler(this.upperLimit_numericUpDown_ValueChanged);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.learnUntil_numericUpDown);
            this.panel.Controls.Add(this.learnRate_numericUpDown);
            this.panel.Controls.Add(this.label3);
            this.panel.Controls.Add(this.saveNetworkCfg_button);
            this.panel.Controls.Add(this.label2);
            this.panel.Controls.Add(this.label1);
            this.panel.Controls.Add(this.upperLimit_numericUpDown);
            this.panel.Location = new System.Drawing.Point(1, 340);
            this.panel.Margin = new System.Windows.Forms.Padding(2);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(570, 26);
            this.panel.TabIndex = 2;
            // 
            // learnUntil_numericUpDown
            // 
            this.learnUntil_numericUpDown.DecimalPlaces = 3;
            this.learnUntil_numericUpDown.Location = new System.Drawing.Point(343, 4);
            this.learnUntil_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.learnUntil_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.learnUntil_numericUpDown.Name = "learnUntil_numericUpDown";
            this.learnUntil_numericUpDown.Size = new System.Drawing.Size(58, 20);
            this.learnUntil_numericUpDown.TabIndex = 7;
            this.learnUntil_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.learnUntil_numericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.learnUntil_numericUpDown.ValueChanged += new System.EventHandler(this.learnUntil_numericUpDown_ValueChanged);
            // 
            // learnRate_numericUpDown
            // 
            this.learnRate_numericUpDown.DecimalPlaces = 4;
            this.learnRate_numericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.learnRate_numericUpDown.Location = new System.Drawing.Point(203, 3);
            this.learnRate_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.learnRate_numericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.learnRate_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.learnRate_numericUpDown.Name = "learnRate_numericUpDown";
            this.learnRate_numericUpDown.Size = new System.Drawing.Size(69, 20);
            this.learnRate_numericUpDown.TabIndex = 3;
            this.learnRate_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.learnRate_numericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            262144});
            this.learnRate_numericUpDown.ValueChanged += new System.EventHandler(this.learnRate_numericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Learn Until:";
            // 
            // saveNetworkCfg_button
            // 
            this.saveNetworkCfg_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveNetworkCfg_button.Location = new System.Drawing.Point(413, 3);
            this.saveNetworkCfg_button.Margin = new System.Windows.Forms.Padding(2);
            this.saveNetworkCfg_button.Name = "saveNetworkCfg_button";
            this.saveNetworkCfg_button.Size = new System.Drawing.Size(147, 20);
            this.saveNetworkCfg_button.TabIndex = 5;
            this.saveNetworkCfg_button.Text = "Save Network Cfg";
            this.saveNetworkCfg_button.UseVisualStyleBackColor = true;
            this.saveNetworkCfg_button.Click += new System.EventHandler(this.saveNetworkCfg_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Learning rate:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Upper limit:";
            // 
            // Chart_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 366);
            this.Controls.Add(this.myChart);
            this.Controls.Add(this.panel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Chart_Form";
            this.Text = "Chart";
            ((System.ComponentModel.ISupportInitialize)(this.myChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upperLimit_numericUpDown)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.learnUntil_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.learnRate_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart myChart;
        private System.Windows.Forms.NumericUpDown upperLimit_numericUpDown;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown learnRate_numericUpDown;
        private System.Windows.Forms.Button saveNetworkCfg_button;
        private System.Windows.Forms.NumericUpDown learnUntil_numericUpDown;
        private System.Windows.Forms.Label label3;
    }
}