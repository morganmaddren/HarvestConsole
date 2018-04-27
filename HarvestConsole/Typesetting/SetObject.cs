using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Typesetting
{
    interface ISetObject
    {
        XVector Position { get; set; }

        XSize Size { get; }

        void Draw();
    }
}
