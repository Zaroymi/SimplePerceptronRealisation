using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetRealization.Functions
{
    public static class ErrorFunctions
    {
        public static double MSE(List<double> value, double countItter)
        {
            return value.ConvertAll((val) => Math.Pow(val, 2)).Sum() / countItter;
        }
    }
}
