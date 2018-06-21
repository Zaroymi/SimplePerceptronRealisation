using System;
using System.Collections.Generic;
using System.Text;
using NetRealization.Functions;
using NetRealization.Other;

namespace NetRealization.Neurons
{
    public class BiasNeuron : INeuron
    {
       
        public double Result { get; set; }
        public List<Connector> OutputConnections { get; set; } = new List<Connector>();
        public List<Connector> InputConnections { get; set; } = null;

        public event EventHandler<NeuronEventArgs> CountEndsEvent;

        public BiasNeuron()
        {
            Result = 1;
            InputConnections = null;
        }

        public double CountOutput()
        {
            return Result = 1;
        }

        public void Train(double speedTrain, double? moment = null)
        {
            double delta = 0d;
            foreach (Connector conn in OutputConnections)
            {
                delta += conn.Weight * conn.DeltaPrevLayer;
            }
            delta *= ActivateFunctions.SigmoigDeriv(Result);
            double realMoment = moment ?? 0;
            foreach (Connector conn in OutputConnections)
            {
                double grad = Result * conn.DeltaPrevLayer;
                conn.DeltaWeight = speedTrain * grad + (realMoment * conn.PrevDelta ?? 0);
                conn.Weight += conn.DeltaWeight;
                conn.PrevDelta = conn.DeltaWeight;
            };
        }
    }
}
