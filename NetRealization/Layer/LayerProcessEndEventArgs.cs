using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Layer
{
    public class LayerProcessEndEventArgs : EventArgs
    {
        public string LayerProcessStatus { get; }

        public LayerProcessEndEventArgs(string status)
        {
            LayerProcessStatus = status;
        }
    }
}
