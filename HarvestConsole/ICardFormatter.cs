using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole
{
    interface ICardFormatter
    {
        void Draw(Context context, XGraphics gfx, XRect bounds, CardData card, PrintOptions options);
    }

    interface ICardFormatter<T> : ICardFormatter where T : CardData
    {
        void Draw(Context context, XGraphics gfx, XRect bounds, T card, PrintOptions options);
    }
}
