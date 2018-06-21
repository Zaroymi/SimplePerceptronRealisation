using NetRealization.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Neurons
{
    public interface INeuron
    {
        event EventHandler<NeuronEventArgs> CountEndsEvent;

        List<Connector> InputConnections { get; set; }

        double Result { get; set; }

        List<Connector> OutputConnections { get; set; }

        double CountOutput();

        void Train(double speedTrain, double? moment = null);
    }
}
