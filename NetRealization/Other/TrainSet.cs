using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Other
{
    public class TrainSet
    {
        public List<double> Input { get; }

        public List<double> Output { get; }

        public TrainSet(List<double> input, List<double> output)
        {
            Input = input;
            Output = output;
        }
    }
}
