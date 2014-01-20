using System;
using System.Windows.Forms;

namespace RealTimeGraph
{
    public partial class GraphControl : UserControl
    {
        public GraphControl()
        {
            InitializeComponent();
            InitialGraph();
        }

        private void RTGControl_Resize(object sender, EventArgs e)
        {
            pbTitle.Refresh();
            pbCurve.Refresh();
        }
    }
}
