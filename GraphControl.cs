﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace RealTimeGraph
{
    public partial class GraphControl : UserControl
    {
        private GraphProperties graphProperties;

        public GraphControl()
        {
            InitializeComponent();
            InitialGraph();
        }

        private void InitialGraph()
        {
            graphProperties = new GraphProperties();
            graphData = new DataGraph();
            initialRect = new DataRect();

            // 默认初始处于滚动模式
            GraphStyle = GraphMode.FixMoveMode;

            IsShowGrid = false;

            GraphTitle = "位移实时显示曲线";
            AxisXTitle = "时间(s)";
            AxisYTitle = "位移(mm)";

            pbZoom.BackColor = Color.FromArgb(50, 0, 64, 128);
            pbZoom.Visible = false;

            MsgOutput = "Ready";
        }

        private void RTGControl_Resize(object sender, EventArgs e)
        {
            pbTitle.Refresh();
            pbCurve.Refresh();
        }

        private void graphStyleMenu_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem parent = sender as ToolStripDropDownItem;
            if (parent != null)
            {
                string graphStyleStr = this.GraphStyle.ToString();
                foreach (ToolStripMenuItem item in parent.DropDownItems)
                {
                    item.Checked = item.Tag.Equals(graphStyleStr);
                }
            }
        }

        private void graphStyleMenu_DropDownItemClicked(
            object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string graphStyleStr = item.Tag as string;
            if (graphStyleStr != null)
            {
                this.GraphStyle = (GraphMode)Enum.Parse(
                    typeof(GraphMode), graphStyleStr);
                this.Refresh();
            }
        }

        private void ctxGraphMenu_Opening(
            object sender, System.ComponentModel.CancelEventArgs e)
        {
            showGridMenu.Checked = IsShowGrid;
        }

        private void showGridMenu_Click(object sender, EventArgs e)
        {
            IsShowGrid = !IsShowGrid;
            this.Refresh();
        }

        private void screenShotMenu_Click(object sender, EventArgs e)
        {
            this.ScreenShot();
        }
    }
}
