namespace RealTimeGraph
{
    partial class GraphControl
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
            this.components = new System.ComponentModel.Container();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.pbZoom = new System.Windows.Forms.PictureBox();
            this.pbAxisX = new System.Windows.Forms.PictureBox();
            this.pbRight = new System.Windows.Forms.PictureBox();
            this.pbTitle = new System.Windows.Forms.PictureBox();
            this.pbAxisY = new System.Windows.Forms.PictureBox();
            this.pbCurve = new System.Windows.Forms.PictureBox();
            this.ctxGraphMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.graphStyleMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.globalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixedMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dragToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.screenShotMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.panelGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurve)).BeginInit();
            this.ctxGraphMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGraph
            // 
            this.panelGraph.Controls.Add(this.pbZoom);
            this.panelGraph.Controls.Add(this.pbAxisX);
            this.panelGraph.Controls.Add(this.pbRight);
            this.panelGraph.Controls.Add(this.pbTitle);
            this.panelGraph.Controls.Add(this.pbAxisY);
            this.panelGraph.Controls.Add(this.pbCurve);
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(0, 0);
            this.panelGraph.Margin = new System.Windows.Forms.Padding(0);
            this.panelGraph.MinimumSize = new System.Drawing.Size(400, 300);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(400, 300);
            this.panelGraph.TabIndex = 0;
            // 
            // pbZoom
            // 
            this.pbZoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.pbZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbZoom.InitialImage = null;
            this.pbZoom.Location = new System.Drawing.Point(237, 146);
            this.pbZoom.Name = "pbZoom";
            this.pbZoom.Size = new System.Drawing.Size(100, 50);
            this.pbZoom.TabIndex = 5;
            this.pbZoom.TabStop = false;
            this.pbZoom.Visible = false;
            // 
            // pbAxisX
            // 
            this.pbAxisX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbAxisX.BackColor = System.Drawing.Color.Gainsboro;
            this.pbAxisX.Location = new System.Drawing.Point(0, 240);
            this.pbAxisX.Margin = new System.Windows.Forms.Padding(0);
            this.pbAxisX.Name = "pbAxisX";
            this.pbAxisX.Size = new System.Drawing.Size(400, 60);
            this.pbAxisX.TabIndex = 3;
            this.pbAxisX.TabStop = false;
            this.pbAxisX.Paint += new System.Windows.Forms.PaintEventHandler(this.pbAxisX_Paint);
            // 
            // pbRight
            // 
            this.pbRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRight.BackColor = System.Drawing.Color.Gainsboro;
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
            this.pbTitle.BackColor = System.Drawing.Color.Gainsboro;
            this.pbTitle.Location = new System.Drawing.Point(80, 0);
            this.pbTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pbTitle.Name = "pbTitle";
            this.pbTitle.Size = new System.Drawing.Size(280, 40);
            this.pbTitle.TabIndex = 1;
            this.pbTitle.TabStop = false;
            this.pbTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTitle_Paint);
            // 
            // pbAxisY
            // 
            this.pbAxisY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbAxisY.BackColor = System.Drawing.Color.Gainsboro;
            this.pbAxisY.Location = new System.Drawing.Point(0, 0);
            this.pbAxisY.Margin = new System.Windows.Forms.Padding(0);
            this.pbAxisY.Name = "pbAxisY";
            this.pbAxisY.Size = new System.Drawing.Size(80, 240);
            this.pbAxisY.TabIndex = 0;
            this.pbAxisY.TabStop = false;
            this.pbAxisY.Paint += new System.Windows.Forms.PaintEventHandler(this.pbAxisY_Paint);
            // 
            // pbCurve
            // 
            this.pbCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCurve.BackColor = System.Drawing.Color.Black;
            this.pbCurve.ContextMenuStrip = this.ctxGraphMenu;
            this.pbCurve.Location = new System.Drawing.Point(80, 40);
            this.pbCurve.Margin = new System.Windows.Forms.Padding(0);
            this.pbCurve.Name = "pbCurve";
            this.pbCurve.Size = new System.Drawing.Size(280, 200);
            this.pbCurve.TabIndex = 4;
            this.pbCurve.TabStop = false;
            this.pbCurve.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCurve_Paint);
            this.pbCurve.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pbCurve_MouseDoubleClick);
            this.pbCurve.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCurve_MouseDown);
            this.pbCurve.MouseEnter += new System.EventHandler(this.pbCurve_MouseEnter);
            this.pbCurve.MouseLeave += new System.EventHandler(this.pbCurve_MouseLeave);
            this.pbCurve.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCurve_MouseMove);
            this.pbCurve.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCurve_MouseUp);
            this.pbCurve.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pbCurve_MouseWheel);
            // 
            // ctxGraphMenu
            // 
            this.ctxGraphMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphStyleMenu,
            this.showGridMenu,
            this.screenShotMenu});
            this.ctxGraphMenu.Name = "ctxGraphMenu";
            this.ctxGraphMenu.Size = new System.Drawing.Size(153, 92);
            this.ctxGraphMenu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxGraphMenu_Opening);
            // 
            // graphStyleMenu
            // 
            this.graphStyleMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalToolStripMenuItem,
            this.fixedMoveToolStripMenuItem,
            this.zoomInToolStripMenuItem,
            this.dragToolStripMenuItem});
            this.graphStyleMenu.Name = "graphStyleMenu";
            this.graphStyleMenu.Size = new System.Drawing.Size(152, 22);
            this.graphStyleMenu.Text = "Graph Style";
            this.graphStyleMenu.DropDownOpening += new System.EventHandler(this.graphStyleMenu_DropDownOpening);
            this.graphStyleMenu.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.graphStyleMenu_DropDownItemClicked);
            // 
            // globalToolStripMenuItem
            // 
            this.globalToolStripMenuItem.Name = "globalToolStripMenuItem";
            this.globalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.globalToolStripMenuItem.Tag = "GlobalMode";
            this.globalToolStripMenuItem.Text = "Global";
            // 
            // fixedMoveToolStripMenuItem
            // 
            this.fixedMoveToolStripMenuItem.Name = "fixedMoveToolStripMenuItem";
            this.fixedMoveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fixedMoveToolStripMenuItem.Tag = "FixMoveMode";
            this.fixedMoveToolStripMenuItem.Text = "Fixed Move";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.zoomInToolStripMenuItem.Tag = "RectZoomInMode";
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            // 
            // dragToolStripMenuItem
            // 
            this.dragToolStripMenuItem.Name = "dragToolStripMenuItem";
            this.dragToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dragToolStripMenuItem.Tag = "DragMode";
            this.dragToolStripMenuItem.Text = "Drag";
            // 
            // showGridMenu
            // 
            this.showGridMenu.Name = "showGridMenu";
            this.showGridMenu.Size = new System.Drawing.Size(152, 22);
            this.showGridMenu.Text = "Show Grid";
            this.showGridMenu.Click += new System.EventHandler(this.showGridMenu_Click);
            // 
            // screenShotMenu
            // 
            this.screenShotMenu.Name = "screenShotMenu";
            this.screenShotMenu.Size = new System.Drawing.Size(152, 22);
            this.screenShotMenu.Text = "Screen Shot";
            this.screenShotMenu.Click += new System.EventHandler(this.screenShotMenu_Click);
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGraph);
            this.Name = "GraphControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.Resize += new System.EventHandler(this.RTGControl_Resize);
            this.panelGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAxisY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurve)).EndInit();
            this.ctxGraphMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.PictureBox pbTitle;
        private System.Windows.Forms.PictureBox pbAxisY;
        private System.Windows.Forms.PictureBox pbCurve;
        private System.Windows.Forms.PictureBox pbAxisX;
        private System.Windows.Forms.PictureBox pbRight;
        private System.Windows.Forms.PictureBox pbZoom;
        private System.Windows.Forms.ContextMenuStrip ctxGraphMenu;
        private System.Windows.Forms.ToolStripMenuItem graphStyleMenu;
        private System.Windows.Forms.ToolStripMenuItem globalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixedMoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dragToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridMenu;
        private System.Windows.Forms.ToolStripMenuItem screenShotMenu;
    }
}
