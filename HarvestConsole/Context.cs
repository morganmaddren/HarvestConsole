using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestConsole.Managers;

namespace HarvestConsole
{
    class Context
    {
        public string CurrentDirectory { get; set; } = @"C:\Users\morga\OneDrive\Harvest\HarvestConsole";
        public string CardDataDirectory { get { return CurrentDirectory + @"\CardData"; } }
        public string ImagesDirectory { get { return CurrentDirectory + @"\Images"; } }
        public string OutputDirectory { get { return CurrentDirectory + @"\Output"; } }
        public string TemplatesDirectory { get { return CurrentDirectory + @"\Templates"; } }

        public SpreadsheetManager SpreadsheetManager { get; private set; }
        public TemplateManager TemplateManager { get; private set; }
        public ImageManager CardImageManager { get; private set; }
        public FormattingManager FormattingManager { get; private set; }
        public MacroManager MacroManager { get; private set; }

        public bool IsExiting { get; set; } = false;

        public Context()
        {
            SpreadsheetManager = new SpreadsheetManager(CardDataDirectory);
            TemplateManager = new TemplateManager(TemplatesDirectory);
            CardImageManager = new ImageManager(ImagesDirectory);
            FormattingManager = new FormattingManager(this);
            MacroManager = new MacroManager(this);
        }
    }
}
