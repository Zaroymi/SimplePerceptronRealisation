using System;
using System.Collections.Generic;
using System.Text;
using NetRealization.Neurons;
using NetRealization.Other;

namespace NetRealization.Layer
{
    public class HiddenLayer : Layer
    {
        public HiddenLayer(List<HiddenNeuron> neurons) : base(neurons.ConvertAll((neu) => neu as INeuron))
        {
            statusEnds = "Hidden layer ends";
        }

        public HiddenLayer(int count) : base()
        {
            for (int i = 0; i < count; i++)
            {
                Neurons.Add(new HiddenNeuron());
            }
            //Neurons.Add(new BiasNeuron());
            SubscribeNeuronsCountEnds();
            statusEnds = $"Hidden layer ends";
        }

        public override bool Connect(List<INeuron> previous, bool isGenerate = false)
        {
            bool result = false;
            foreach(INeuron prev in previous)
            {
                foreach (INeuron neu in Neurons)
                {
                    Connector connect = new Connector(prev, neu);
                    if(isGenerate)
                    {
                        connect.Weight = new Random(connect.GetHashCode()).NextDouble();
                    }
                    if (neu is BiasNeuron)
                    {
                        prev.OutputConnections.Add(connect);
                    }
                    else
                    {
                        prev.OutputConnections.Add(connect);
                        neu.InputConnections.Add(connect);
                    }
                }
                result = true;
            }
            return result;
        }
    }
}
