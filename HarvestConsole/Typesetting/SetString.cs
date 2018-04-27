using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Typesetting
{
    class SetString : ISetObject
    {
        public SetString(XGraphics gfx, string text, XFont font)
        {
            this.gfx = gfx;
            Text = text;
            this.Font = font;
            Position = new XVector();
        }

        XGraphics gfx;

        public string Text { get; private set; }
        public XFont Font { get; private set; }

        public XVector Position { get; set; }

        public XSize Size
        {
            get { return gfx.MeasureString(Text, Font); }
        }

        public void Draw()
        {
            gfx.DrawString(Text, Font, XBrushes.Black, new XPoint(Position.X, Position.Y));
        }
    }
}
