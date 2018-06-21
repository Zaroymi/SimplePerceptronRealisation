using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRealization.Functions;
using NetRealization.Other;

namespace NetRealization.Neurons
{
    public class HiddenNeuron : INeuron
    {
        public List<Connector> InputConnections { get; set; } = new List<Connector>();
        public double Result { get; set; }
        public double Input { get => InputConnections.Sum(conn => conn.Weight * conn.OutData); }
        public List<Connector> OutputConnections { get; set; } = new List<Connector>();

        public event EventHandler<NeuronEventArgs> CountEndsEvent;

        public double CountOutput()
        {
            double value = Input;
            double result = ActivateFunctions.Sigmoid(value);
            Result = result;
            MakeEvent();
            return result;
        }
        
        public void MakeEvent()
        {
            List<double> weights = InputConnections.Select((conn) => conn.Weight).ToList();
            NeuronEventArgs args = new NeuronEventArgs(weights, Result);
            CountEndsEvent?.Invoke(this, args);
        }

        public void Train(double speedTrain, double? moment = null)
        {
            double delta = 0d;
            foreach(Connector conn in OutputConnections)
            {
                delta += conn.Weight * conn.DeltaPrevLayer;
            }
             delta *= ActivateFunctions.SigmoigDeriv(Result);
            double realMoment = moment ?? 0;
            foreach (Connector conn in OutputConnections)
            {
                double grad = Result * conn.DeltaPrevLayer;
                conn.DeltaWeight = (speedTrain * grad) + (realMoment * conn.PrevDelta ?? 0);
                conn.Weight += conn.DeltaWeight;
                conn.PrevDelta = conn.DeltaWeight;
            }
            foreach (Connector conn in InputConnections)
            {
                conn.DeltaPrevLayer = conn.DeltaWeight;
            }
            MakeEvent();
        }
    }
}
