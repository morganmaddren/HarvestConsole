using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HarvestConsole.Managers
{
    class SpreadsheetManager
    {
        string directory;
        Dictionary<string, CardDataSpreadsheet> spreadsheets;

        public IEnumerable<string> SpreadsheetNames
        {
            get { return spreadsheets.Keys.OrderBy(x => x, StringComparer.OrdinalIgnoreCase); }
        }

        public SpreadsheetManager(string directory)
        {
            this.directory = directory;
            spreadsheets = new Dictionary<string, CardDataSpreadsheet>(StringComparer.OrdinalIgnoreCase);
            this.Refresh();
        }

        public void Refresh()
        {
            spreadsheets.Clear();
            foreach (var f in Directory.EnumerateFiles(this.directory).Where(x => x.EndsWith(".tsv")))
            {
                var sheet = new CardDataSpreadsheet(f);
                spreadsheets[sheet.Name] = sheet;
            }
        }

        public CardDataSpreadsheet Load(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();
            
            if (!spreadsheets.ContainsKey(name))
                throw new ArgumentException();

            spreadsheets[name].Load();

            return spreadsheets[name];
        }
    }
}
