namespace RealTimeGraph
{
    partial class RTGControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelGraph = new System.Windows.Forms.Panel();
            this.pbCurve = new System.Windows.Forms.PictureBox();
            this.pbAxisX = new System.Windows.Forms.PictureBox();
            this.pbRight = new System.Windows.Forms.PictureBox();
            this.pbTitle = new System.Windows.Forms.PictureBox();
            this.pbAxisY = new System.Windows.Forms.PictureBox();
            this.panelGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisY)).BeginInit();
            this.SuspendLayout();
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.pbCurve);
            this.panelGraph.Controls.Add(this.pbAxisX);
            this.panelGraph.Controls.Add(this.pbRight);
            this.panelGraph.Controls.Add(this.pbTitle);
            this.panelGraph.Controls.Add(this.pbAxisY);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Margin = new System.Windows.Forms.Padding(0);
            this.panelGraph.MinimumSize = new System.Drawing.Size(400, 300);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(400, 300);
            this.panelGraph.TabIndex = 0;
            // 
            // pbCurve
            // 
            this.pbCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCurve.BackColor = System.Drawing.Color.Black;
            this.pbCurve.Location = new System.Drawing.Point(80, 40);
            this.pbCurve.Margin = new System.Windows.Forms.Padding(0);
            this.pbCurve.Name = "pbCurve";
            this.pbCurve.Size = new System.Drawing.Size(280, 200);
            this.pbCurve.TabIndex = 4;
            this.pbCurve.TabStop = false;
            // 
            // pbAxisX
            // 
            this.pbAxisX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbAxisX.BackColor = System.Drawing.Color.Blue;
            this.pbAxisX.Location = new System.Drawing.Point(0, 240);
            this.pbAxisX.Margin = new System.Windows.Forms.Padding(0);
            this.pbAxisX.Name = "pbAxisX";
            this.pbAxisX.Size = new System.Drawing.Size(400, 60);
            this.pbAxisX.TabIndex = 3;
            this.pbAxisX.TabStop = false;
            // 
            // pbRight
            // 
            this.pbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRight.BackColor = System.Drawing.Color.Yellow;
            this.pbRight.Location = new System.Drawing.Point(360, 0);
            this.pbRight.Margin = new System.Windows.Forms.Padding(0);
            this.pbRight.Name = "pbRight";
            this.pbRight.Size = new System.Drawing.Size(40, 240);
            this.pbRight.TabIndex = 2;
            this.pbRight.TabStop = false;
            // 
            // pbTitle
            // 
            this.pbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pbTitle.Location = new System.Drawing.Point(80, 0);
            this.pbTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pbTitle.Name = "pbTitle";
            this.pbTitle.Size = new System.Drawing.Size(280, 40);
            this.pbTitle.TabIndex = 1;
            this.pbTitle.TabStop = false;
            // 
            // pbAxisY
            // 
            this.pbAxisY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbAxisY.BackColor = System.Drawing.Color.Red;
            this.pbAxisY.Location = new System.Drawing.Point(0, 0);
            this.pbAxisY.Margin = new System.Windows.Forms.Padding(0);
            this.pbAxisY.Name = "pbAxisY";
            this.pbAxisY.Size = new System.Drawing.Size(80, 240);
            this.pbAxisY.TabIndex = 0;
            this.pbAxisY.TabStop = false;
            // 
            // RTGControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGraph);
            this.Name = "RTGControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.panelGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.PictureBox pbTitle;
        private System.Windows.Forms.PictureBox pbAxisY;
        private System.Windows.Forms.PictureBox pbCurve;
        private System.Windows.Forms.PictureBox pbAxisX;
        private System.Windows.Forms.PictureBox pbRight;
    }
}
