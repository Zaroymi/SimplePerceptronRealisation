using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRealization.Functions;
using NetRealization.Other;

namespace NetRealization.Neurons
{
    public class OutputNeuron : INeuron
    {
        public List<Connector> InputConnections { get; set; } = new List<Connector>();
        public double Result { get; set; }
        public double Input { get => InputConnections.Sum(conn => conn.Weight * conn.OutData);}
        public List<Connector> OutputConnections { get; set; } = new List<Connector>();

        public List<double> DataErrorsItters { get; set; } = new List<double>();

        public double RightValue { get; set; }

        public event EventHandler<NeuronEventArgs> CountEndsEvent;

        public void Train(double speedTrain, double? moment = null)
        {
            double error = CountError();
            double delta = (RightValue - Result) * ActivateFunctions.SigmoigDeriv(Result);
            foreach(Connector conn in InputConnections)
            {
                conn.DeltaPrevLayer = delta;
            }
            MakeEvent(error);
        }

        public double CountOutput()
        {
            double value = Input;
            double result = ActivateFunctions.Sigmoid(value);
            Result = result;
            MakeEvent();
            return result;
        }

        public void MakeEvent(double? error = null)
        {
            List<double> weights = InputConnections.Select((conn) => conn.Weight).ToList();
            NeuronEventArgs args = new NeuronEventArgs(weights, Result, error);
            CountEndsEvent?.Invoke(this, args);
        }

        private double CountError()
        {
            DataErrorsItters.Add(RightValue - Result);
            return ErrorFunctions.MSE(DataErrorsItters, DataErrorsItters.Count);
        }
    }
}
