using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestConsole.Statistics.Balance;

namespace HarvestConsole.Commands
{
    class ReinvestmentSimCommand : CommandBase
    {
        public override string Name => "reinv";

        public override string Description => "print reinvestment sim";

        static readonly Parameter<string> Sheet = new Parameter<string>("sheet");
        static readonly OptionalParameter<bool> Debug = new OptionalParameter<bool>("debug", "False");
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Sheet,
            Debug
        };

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var sheet = parameters.Get(Sheet);
            var balanceData = BalanceLibrary.GetBalanceData(sheet);

            List<Tuple<int, int>> crops = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(2, 2),
                new Tuple<int, int>(3, 3),
                new Tuple<int, int>(5, 4),
                new Tuple<int, int>(7, 5),
            };
            
            foreach (Tuple<int, int> crop in crops)
            {
                int cropHand = crop.Item1;
                int cropDew = crop.Item2;

                Console.WriteLine();
                Console.WriteLine($"Sim with cropHand={cropHand} cropDew={cropDew} cropNet={FormatDouble(balanceData.Data[$"cropnet{cropHand}_{cropDew}"])}");

                var sum = 0;
                var turns = new double[] { 5, 5, 5, 4, 4, 4, 3, 3, 3 };

                for (int i = 0; i < turns.Length; i++)
                {
                    var income = turns[i];
                    sum += (int)income;

                    if (i != turns.Length - 1)
                    {
                        var net = balanceData.Data[$"cropnet{cropHand}_{cropDew}"];
                        net *= income / cropHand;
                        Console.WriteLine($"Turn {i}: {FormatDouble(income)} nets {FormatDouble(net)}");
                        
                        if (i + cropDew < turns.Length)
                            turns[i + cropDew] += net;
                        else
                            turns[turns.Length - 1] += net;
                    }
                    else
                    {
                        Console.WriteLine($"Turn {i}: {FormatDouble(income)} (no net on last turn)");
                    }
                }

                Console.WriteLine($"Sum: {FormatDouble(sum)}");
            }
        }

        private string FormatDouble(double d)
        {
            return d.ToString("#.00");
        }
    }
}
