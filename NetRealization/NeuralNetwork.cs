using NetRealization.Layer;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using NetRealization.Neurons;
using NetRealization.Other;

namespace NetRealization
{
    public class NeuralNetwork
    {
        public event EventHandler<LayerProcessEndEventArgs> InputEndProcess;

        public event EventHandler<LayerProcessEndEventArgs> HiddenEndProcess;

        public event EventHandler<LayerProcessEndEventArgs> OutEndProcess;

        public event EventHandler<NeuronEventArgs> NeuronEndProcess;

        public InputLayer InputLayer { get; protected set; }

        public List<HiddenLayer> HiddenLayers { get; protected set; } = new List<HiddenLayer>();

        public OutputLayer OutputLayer { get; protected set; }

        public NeuralNetwork(int inputNeurons, int hiddenNeurons, int outputNeurons)
        {
            InputLayer = new InputLayer(inputNeurons);
            HiddenLayers.Add(new HiddenLayer(hiddenNeurons));
            OutputLayer = new OutputLayer(outputNeurons);
            Generate();
            CreateSubscribes();
        }

        public NeuralNetwork(int inputNeurons, int hiddenNeurons, int outputNeurons, int hidLayers)
        {
            InputLayer = new InputLayer(inputNeurons);
            for (int i = 0; i < hidLayers; i++)
            {
                HiddenLayers.Add(new HiddenLayer(hiddenNeurons));
            }
            OutputLayer = new OutputLayer(outputNeurons);
            Generate();
            CreateSubscribes();
        }

        public void Process(List<double> values)
        {
            InputLayer.TakeInputDate(values);
            InputLayer.Process();
            foreach(HiddenLayer layer in HiddenLayers)
            {
                layer.Process();
            }
            OutputLayer.Process();
        }

        public void Train(List<TrainSet> trainSets, int epoch, double trainSpeed, double? moment = null)
        {
            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < trainSets.Count; j++)
                {
                    OutputLayer.RightValues(trainSets[j].Output);
                    Process(trainSets[j].Input);
                    OutputLayer.Train(trainSpeed, moment);
                    foreach(HiddenLayer hLayer in HiddenLayers)
                    {
                        hLayer.Train(trainSpeed, moment);
                    }
                    InputLayer.Train(trainSpeed, moment);
                    //Process(trainSets[j].Input);
                }
            }
        }

        private void CreateSubscribes()
        {
            InputLayer.LayerProcessEndsEvent += InputLayerLayerProcessEndsEvent;
            InputLayer.NeuronEndsCountEvent += NeuronEndsCountEvent;
            foreach (HiddenLayer hidL in HiddenLayers)
            {
                hidL.LayerProcessEndsEvent += HiddenLayerProcessEndsEvent;
                hidL.NeuronEndsCountEvent += NeuronEndsCountEvent;
            }
            OutputLayer.LayerProcessEndsEvent += OutputLayerLayerProcessEndsEvent;
            OutputLayer.NeuronEndsCountEvent += NeuronEndsCountEvent;
        }

        #region EventsSub
        private void NeuronEndsCountEvent(object sender, NeuronEventArgs e)
        {
            NeuronEndProcess?.Invoke(sender, e);
        }

        private void OutputLayerLayerProcessEndsEvent(object sender, LayerProcessEndEventArgs e)
        {
            OutEndProcess?.Invoke(this, e);
        }

        private void HiddenLayerProcessEndsEvent(object sender, LayerProcessEndEventArgs e)
        {
            HiddenEndProcess?.Invoke(this, e);
        }

        private void InputLayerLayerProcessEndsEvent(object sender, LayerProcessEndEventArgs e)
        {
            InputEndProcess?.Invoke(this, e);
        }
        #endregion

        private void Generate()
        {
            if (HiddenLayers.Count != 0)
            {
                HiddenLayers.First().Connect(InputLayer.Neurons, true);
                OutputLayer.Connect(HiddenLayers.Last().Neurons, true);
                if (HiddenLayers.Count > 1)
                {
                    for (int i = 0; i < HiddenLayers.Count - 1; i++)
                    {
                        HiddenLayers[i + 1].Connect(HiddenLayers[i].Neurons, true);
                    }
                }
            }
        }
    }
}
