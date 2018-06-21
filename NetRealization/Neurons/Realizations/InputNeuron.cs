using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRealization.Functions;
using NetRealization.Other;

namespace NetRealization.Neurons
{
    public class InputNeuron : INeuron
    {
        public List<Connector> InputConnections { get; set; } = new List<Connector>();
        public double Result { get; set; }
        public List<Connector> OutputConnections { get; set; } = new List<Connector>();

        public event EventHandler<NeuronEventArgs> CountEndsEvent;

        public InputNeuron()
        {
            InputConnections = null;
        }

        public double CountOutput()
        {
            MakeEvent();
            return Result;
        }

        public void MakeEvent()
        {
            List<double> weights = OutputConnections.Select((conn) => conn.Weight).ToList();
            NeuronEventArgs args = new NeuronEventArgs(weights, Result);
            CountEndsEvent?.Invoke(this, args);
        }

        public void Train(double speedTrain, double? moment = null)
        {
            double realMoment = moment ?? 0;
            foreach (Connector conn in OutputConnections)
            {
                double grad = Result * conn.DeltaPrevLayer;
                conn.DeltaWeight = speedTrain * grad + (realMoment * conn.PrevDelta ?? 0);
                conn.Weight += conn.DeltaWeight;
                conn.PrevDelta = conn.DeltaWeight;
            }
            MakeEvent();
        }
    }
}
