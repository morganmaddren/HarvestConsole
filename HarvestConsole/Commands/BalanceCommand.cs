using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestConsole.Statistics.Balance;

namespace HarvestConsole.Commands
{
    class BalanceCommand : CommandBase
    {
        public override string Name => "balance";

        public override string Description => "print balance data";

        static readonly Parameter<string> Sheet = new Parameter<string>("sheet");
        static readonly OptionalParameter<string> Expression = new OptionalParameter<string>("expression", "");
        static readonly OptionalParameter<bool> Debug = new OptionalParameter<bool>("debug", "False");
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Sheet,
            Expression,
            Debug
        };

        private class BalancedCard
        {
            public double PowerBudget { get; private set; }
            public double PowerEstimate { get; private set; }

            public BalancedCard(double powerBudget, double estimate)
            {
                this.PowerBudget = powerBudget;
                this.PowerEstimate = estimate;
            }
        }

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var sheetName = parameters.Get(Sheet);
            var balanceData = BalanceLibrary.GetBalanceData(sheetName);
            CardDataSpreadsheet sheet = this.Context.SpreadsheetManager.Load(sheetName);

            var evaluator = new BalanceStringEvaluator(balanceData);
            List<BalancedCard> balanced = new List<BalancedCard>();

            List<Tuple<double, string>> spells = new List<Tuple<double, string>>();
            List<Tuple<double, string>> crops = new List<Tuple<double, string>>();
            foreach (var card in sheet.Cards)
            {
                if (card.Id.Contains("blank"))
                    continue;

                if (card.Type == "spell")
                {
                    var spellCard = (SpellCardData)card;
                    double budget = balanceData.Data[$"spellbudget{spellCard.PlantValue}"];
                    double effectest = evaluator.Evaluate(spellCard.EffectPowerEstimate);
                    double vp = spellCard.Offerings;

                    double opness = budget - effectest - vp;
                    Tuple<double, string> output = new Tuple<double, string>(
                        opness,
                        $"{spellCard.Title}: budget={FormatDouble(budget)} est={FormatDouble(effectest)} vp={vp} sum={FormatDouble(effectest + vp)} delta={FormatDouble(opness)}");

                    spells.Add(output);
                }
                else if (card.Type == "crop")
                {
                    var cropCard = (CropCardData)card;
                    
                    double budget = balanceData.Data[$"cropbudget{cropCard.PlantCost}_{cropCard.HarvestCost}"];
                    double effectsingleturnest = evaluator.Evaluate(cropCard.EffectPowerEstimate);
                    double effectest = balanceData.CostCropEffect(cropCard.PlantCost, cropCard.HarvestCost, cropCard.EffectCost, effectsingleturnest);
                    double harvestest = evaluator.Evaluate(cropCard.HarvestPowerEstimate);
                    double vp = cropCard.Offerings;

                    double totalpower = effectest + harvestest + vp;
                    double opness = budget - totalpower;
                    Tuple<double, string> output = new Tuple<double, string>(
                        opness,
                        $"{cropCard.Title}: budget={FormatDouble(budget)} harvestest={FormatDouble(harvestest)} effectest={FormatDouble(effectest)} vp={vp} sum={FormatDouble(totalpower)} delta={FormatDouble(opness)}");

                    crops.Add(output);
                }
            }

            Console.WriteLine("Spells");
            foreach (var s in spells.OrderBy(x => x.Item1).Select(x => x.Item2))
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.WriteLine("Crops");
            foreach (var s in crops.OrderBy(x => x.Item1).Select(x => x.Item2))
            {
                Console.WriteLine(s);
            }
        }

        private string FormatDouble(double d)
        {
            return d.ToString("#.00");
        }
    }
}
