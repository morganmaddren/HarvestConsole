using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    class CropCardData : CardData
    {
        public string Title { get { return data["title"]; } }
        public string Color { get { return data["color"]; } }
        public int PlantCost { get { return int.Parse(data["plant"]); } }
        public int EffectCost { get { return data["effectcost"] == string.Empty ? 0 : int.Parse(data["effectcost"]); } }
        public string Effect { get { return data["effect"]; } }
        public string EffectType { get { return data["effecttype"]; } }
        public int HarvestCost { get { return int.Parse(data["harvestcost"]); } }
        public string HarvestEffect { get { return data["harvesteffect"]; } }
        public int Offerings { get { return data["offerings"] == string.Empty ? 0 : int.Parse(data["offerings"]); } }
        public string TemplateString { get { return Type + "_" + Color; } }
        public string HarvestPowerEstimate { get { return data["harvestpowerest"]; } }
        public string EffectPowerEstimate { get { return data["effectpowerest"]; } }
    }

    class SpellCardData : CardData
    {
        public string Title { get { return data["title"]; } }
        public string Color { get { return data["color"]; } }
        public int Offerings { get { return data["offerings"] == string.Empty ? 0 : int.Parse(data["offerings"]); } }
        public int PlantValue { get { return int.Parse(data["plant"]); } }
        public int CropRequirement { get { return int.Parse(data["cropreq"]); } }
        public string TemplateString { get { return Type + "_" + Color; } }
        public string Text { get { return data["text"]; } }
        public string EffectPowerEstimate { get { return data["powerest"]; } }
    }

    class SeasonCardData : CardData
    {
        public string Title { get { return data["title"]; } }
        public string Color { get { return data["color"]; } }
        public int PlantValue { get { return int.Parse(data["plant"]); } }
        public int Offerings { get { return data["offerings"] == string.Empty ? 0 : int.Parse(data["offerings"]); } }
        public string TemplateString { get { return Type + "_" + Color; } }
        public string Season { get { return data["time"]; } }
        public string Text { get { return data["text"]; } }
    }

    class BackCardData : CardData
    {
        public string TemplateString { get { return Id; } }
    }

    /// <summary>
    /// Data for a single card
    /// </summary>
    class CardData
    {
        private static Dictionary<string, Func<CardData>> cardFactory = new Dictionary<string, Func<CardData>>()
        {
            ["crop"] = () => new CropCardData(),
            ["spell"] = () => new SpellCardData(),
            ["season"] = () => new SeasonCardData(),
            ["back"] = () => new BackCardData(),
        };

        public string SpreadsheetName { get; private set; }
        public string Id { get; private set; }
        public string Type { get; private set; }
        public int Count { get { return int.Parse(data["count"]); } }

        protected Dictionary<string, string> data;

        protected CardData()
        { 
        }
        
        public static CardData Create(string spreadsheetName, List<string> columns, List<string> values)
        {
            if (columns == null || values == null)
                throw new ArgumentException();

            if (!columns.Any())
                throw new ArgumentException();

            var type = columns[0];
            var card = cardFactory[type]();

            card.SpreadsheetName = spreadsheetName;
            card.Id = values[0];
            card.Type = type;
            card.data = columns.Where(x => !string.IsNullOrEmpty(x)).Zip(values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v, StringComparer.OrdinalIgnoreCase);

            return card;
        }

        public string GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(key);

            return data[key];
        }
    }
}
