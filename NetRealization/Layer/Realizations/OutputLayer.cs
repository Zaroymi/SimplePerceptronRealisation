using System;
using System.Collections.Generic;
using System.Text;
using NetRealization.Neurons;
using NetRealization.Other;

namespace NetRealization.Layer
{
    public class OutputLayer : Layer
    {
        public OutputLayer(List<OutputNeuron> neurons) : base(neurons.ConvertAll((neu) => neu as INeuron))
        {
            statusEnds = "Output layer end work";
        }

        public OutputLayer(int count) : base()
        {
            for (int i = 0; i < count; i++)
            {
                Neurons.Add(new OutputNeuron());
            }
            SubscribeNeuronsCountEnds();
            statusEnds = $"Output layer ends";
        }

        public void RightValues(List<double> values)
        {
            int valCount = values.Count;
            if (valCount == Neurons.Count)
            {
                for(int i = 0; i < valCount; i++)
                {
                    (Neurons[i] as OutputNeuron).RightValue = values[i];
                }
            }
        }

        public override bool Connect(List<INeuron> previous, bool isGenerate = false)
        {
            bool result = false;
            foreach (INeuron prev in previous)
            {
                foreach (INeuron neu in Neurons)
                {
                    Connector connect = new Connector(prev, neu);
                    if (isGenerate)
                    {
                        connect.Weight = new Random(connect.GetHashCode()).NextDouble();
                    }
                    neu.InputConnections.Add(connect);
                    prev.OutputConnections.Add(connect);
                }
                result = true;
            }
            return result;
        }
    }
}
