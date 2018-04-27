using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Typesetting
{
    class SetImage : ISetObject
    {
        public XVector Position { get; set; }

        public XSize Size
        {
            get
            {
                return new XSize(7, 7);
            }
        }

        public XVector Offset
        {
            get { return new XVector(0, .5); }
        }

        XGraphics gfx;
        XImage image;

        public SetImage(XGraphics gfx, XImage image)
        {
            this.gfx = gfx;
            this.image = image;
        }

        public void Draw()
        {
            gfx.DrawImage(this.image, new XRect(this.Position.X + Offset.X, this.Position.Y - this.Size.Height + Offset.Y, this.Size.Width, this.Size.Height));
        }
    }
}
