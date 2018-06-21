using NetRealization.Neurons;
using NetRealization.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Layer
{
    public abstract class Layer
    {
        public event EventHandler<LayerProcessEndEventArgs> LayerProcessEndsEvent;

        public event EventHandler<NeuronEventArgs> NeuronEndsCountEvent;

        public List<INeuron> Neurons { get; } = new List<INeuron>();

        public int NeuronsEndsCount { get; protected set; }

        protected string statusEnds = "Layer ends";
    
        public Layer(List<INeuron> neurons)
        {
            Neurons = neurons;
            SubscribeNeuronsCountEnds();
        }

        public Layer()
        {
        }

        protected void SubscribeNeuronsCountEnds()
        {
            foreach(INeuron neu in Neurons)
            {
                neu.CountEndsEvent += NeuronCountEnds;
            }
        }

        protected void NeuronCountEnds(object sender, NeuronEventArgs e)
        {
            NeuronsEndsCount++;
            NeuronEndsCountEvent?.Invoke(this, e);
            if(Neurons.Count == NeuronsEndsCount)
            {
                LayerProcessEndsEvent?.Invoke(this, new LayerProcessEndEventArgs(statusEnds));
            }
        }

        public void Process()
        {
            foreach(INeuron neu in Neurons)
            {
                neu.CountOutput();
            }
        }

        public void Train(double speedTrain, double? moment = null)
        {
            foreach (INeuron neuron in Neurons)
            {
                neuron.Train(speedTrain, moment);
            }
        }

        public abstract bool Connect(List<INeuron> previous, bool isGenerate);
    }
}
