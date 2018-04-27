using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Commands
{
    class ManaCurveCommand : CommandBase
    {
        public override string Name => "manacurve";

        public override string Description => "print the mana curve";

        static readonly Parameter<string> Sheet = new Parameter<string>("sheet");
        static readonly OptionalParameter<bool> Debug = new OptionalParameter<bool>("debug", "False");
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Sheet,
            Debug
        };

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var sheetName = parameters.Get(Sheet);
            var debug = parameters.Get(Debug);
            
            CardDataSpreadsheet sheet = this.Context.SpreadsheetManager.Load(sheetName);
            var spellCards = sheet.Cards.Where(x => x is SpellCardData).Select(x => x as SpellCardData);
            var cropCards = sheet.Cards.Where(x => x is CropCardData).Select(x => x as CropCardData);

            int[] cropSums = new int[8];
            int[] spellSums = new int[8];
            for (int i = 0; i < 8; i++)
            {
                cropSums[i] = cropCards.Where(x => x.Color != "white").Where(x => x.PlantCost == i).Sum(x => x.Count);
                spellSums[i] = spellCards.Where(x => x.PlantValue == i).Sum(x => x.Count);
            }

            Console.WriteLine("");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Crops");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
            for (int i = 0; i < 8; i++)
            {
                string bar = new string('=', cropSums[i]);
                Console.WriteLine(i.ToString() + ": " + bar + " " + cropSums[i].ToString());
            }

            int[,] cropMatrix = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    cropMatrix[i, j] = cropCards.Where(x => x.Color != "white").Where(x => x.PlantCost == i && x.HarvestCost == j).Sum(x => x.Count);
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Plant \\ Dew");
            Console.WriteLine("   1   2   3   4   5   6   7");
            for (int i = 1; i < 8; i++)
            {
                Console.Write(i + ": ");

                for (int j = 1; j < 8; j++)
                {
                    Console.Write($"{cropMatrix[i, j].ToString().PadLeft(3)} ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Spells");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("");
            for (int i = 0; i < 8; i++)
            {
                string bar = new string('=', spellSums[i]);
                Console.WriteLine(i.ToString() + ": " + bar + " " + spellSums[i].ToString());
            }
        }

        private abstract class CardSetStats<T> where T : CardData
        {
            public abstract void Observe(T card);
        }

        class SpellCardStats : CardSetStats<SpellCardData>
        {
            public override void Observe(SpellCardData card)
            {

            }
        }

    }
}
