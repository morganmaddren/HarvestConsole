using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Managers
{
    class MacroManager
    {
        static Dictionary<string, string> macroMapping = new Dictionary<string, string>()
        {
            [""] = "",
        };

        Context context;

        public MacroManager(Context context)
        {
            this.context = context;
        }

        public XImage GetImageMacro(string input)
        {
            return context.TemplateManager.GetImage(input.Substring(1));
        }
    }
}
