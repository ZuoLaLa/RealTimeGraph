using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealTimeGraph
{
    public partial class RTGControl : UserControl
    {
        public RTGControl()
        {
            InitializeComponent();
            initialGraph();
        }

        private void RTGControl_Resize(object sender, EventArgs e)
        {
            pbTitle.Refresh();
            pbCurve.Refresh();
        }
    }
}
