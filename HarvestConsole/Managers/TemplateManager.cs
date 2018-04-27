using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Managers
{
    class TemplateManager : ImageManager
    {
        public TemplateManager(string directory)
            : base(directory)
        {
        }

        public XImage GetImage(TemplateImages template)
        {
            return GetImage(template.ToString());
        }
    }
}
