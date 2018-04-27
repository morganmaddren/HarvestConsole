using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HarvestConsole
{
    class CardDataSpreadsheet
    {
        private string filename;

        Dictionary<string, CardData> cards;

        public IEnumerable<CardData> Cards { get { return this.cards.Values; } }

        public string Name { get; private set; }

        public bool Loaded { get; private set; }

        public CardDataSpreadsheet(string filename)
        {
            this.filename = filename;
            var info = new FileInfo(filename);
            this.Name = info.Name.Replace("CardData - ", "").Replace(".tsv", "");
            cards = new Dictionary<string, CardData>();
        }

        public void Load()
        {
            if (Loaded)
                return;

            bool nextIsHeader = true;
            List<string> headers = new List<string>();
            foreach (var line in File.ReadLines(filename))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    nextIsHeader = true;
                    continue;
                }

                var fields = line.SplitAndTrim('\t', StringSplitOptions.None);

                if (nextIsHeader)
                {
                    headers = fields.ToList();
                    nextIsHeader = false;
                    continue;
                }

                var card = CardData.Create(Name, headers, fields);

                if (cards.ContainsKey(card.Id))
                    throw new InvalidOperationException();

                cards[card.Id] = card;
            }

            this.Loaded = true;
        }

        public void Unload()
        {
            cards = null;
            this.Loaded = false;
        }

    }
}
