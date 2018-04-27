using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestConsole.Statistics.Balance;

namespace HarvestConsole.Commands
{
    class BalanceEvalCommand : CommandBase
    {
        public override string Name => "beval";

        public override string Description => "evaluate a balance data string";

        static readonly Parameter<string> Sheet = new Parameter<string>("sheet");
        static readonly Parameter<string> Expression = new Parameter<string>("expression");
        static readonly OptionalParameter<bool> Debug = new OptionalParameter<bool>("debug", "False");
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Sheet,
            Expression,
            Debug
        };

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var sheetName = parameters.Get(Sheet);
            var expr = parameters.Get(Expression);
            var balanceData = BalanceLibrary.GetBalanceData(sheetName);

            var evaluator = new BalanceStringEvaluator(balanceData);
            var result = evaluator.Evaluate(expr);
            Console.WriteLine(result);
        }

        private string FormatDouble(double d)
        {
            return d.ToString("#.00");
        }
    }
}
