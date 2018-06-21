using NetRealization.Neurons;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Other
{
    public class Connector
    {
        public double OutData { get => FirstNeuron.Result; }

        public double Weight { get; set; }

        public double DeltaWeight { get; set; }

        public double? PrevDelta { get; set; }

        public double DeltaPrevLayer { get; set; }

        public INeuron FirstNeuron { get; }

        public INeuron TargetNeuron { get; }

        public Connector(INeuron first, INeuron target)
        {
            FirstNeuron = first;
            TargetNeuron = target;
        }
    }
}
