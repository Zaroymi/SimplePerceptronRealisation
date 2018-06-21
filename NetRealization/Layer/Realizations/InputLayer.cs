using System;
using System.Collections.Generic;
using System.Text;
using NetRealization.Neurons;

namespace NetRealization.Layer
{
    public class InputLayer : Layer
    { 
        public InputLayer(List<InputNeuron> neurons) : base(neurons.ConvertAll((neu) => neu as INeuron))
        {
            statusEnds = $"Input layer ends";
        }

        public InputLayer(int count) : base()
        {
            for(int i = 0; i < count; i++)
            {
                Neurons.Add(new InputNeuron());
            }
            //Neurons.Add(new BiasNeuron());
            SubscribeNeuronsCountEnds();
            statusEnds = $"Input layer ends";
        }

        public override bool Connect(List<INeuron> previous, bool isGenerate = false)
        {
            return false;
        }

        public bool TakeInputDate(List<double> inputs)
        {
            bool result = false;
            int inputCount = inputs.Count;
            if (inputCount == Neurons.Count)
            {
                for(int i = 0; i < inputCount; i++)
                {
                    Neurons[i].Result = inputs[i];
                }
                result = true;
            }
            return result;
        }
    }
}
