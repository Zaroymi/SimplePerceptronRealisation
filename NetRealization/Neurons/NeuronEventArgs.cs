using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Neurons
{
    public class NeuronEventArgs : EventArgs
    {
        public List<double> InputWeights { get; set; }

        public double Result { get; }

        public double? Error { get; }

        public NeuronEventArgs(List<double> weights, double result, double? error = null)
        {
            InputWeights = weights;
            Result = result;
            Error = error;
        }
    }
}
