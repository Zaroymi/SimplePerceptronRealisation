using System;
using System.Collections.Generic;
using System.Text;

namespace NetRealization.Functions
{
    public static class ActivateFunctions
    {
        public static double Sigmoid(double value)
        {
            return 1 / (1 + Math.Pow(Math.E,-value));
        }

        public static double SigmoigDeriv(double value)
        {
            return (1 - value) * value;
        }
    }
}
