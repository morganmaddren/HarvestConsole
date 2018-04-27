using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole.Formatters
{
    class Card52Formatter<T> : ICardFormatter<T>, ICardFormatter where T : CardData
    {
        static readonly XRect TemplateRect = new XRect(-.125, -.125, 2.75, 3.75);

        static readonly XFont IdFont = new XFont("Arial", 6, XFontStyle.Regular);
        static readonly XBrush IdBrush = XBrushes.Black;

        static readonly XRect IdRect = new XRect(.15, 3.5 - .1, 1.5, 0);
        static readonly XRect CountRect = new XRect(2.5 - .75, 3.5 - .1, 1.5, 0);

        protected static readonly string PlantsIcon = "hand";
        protected static readonly XFont PlantsFont = new XFont("Tahoma", 18, XFontStyle.Regular);
        protected static readonly XBrush PlantsBrush = XBrushes.White;
        protected static readonly XBrush PlantsBorderBrush = XBrushes.Black;
        protected static readonly XSize PlantsSize = new XSize(.5, .5);
        protected static readonly XPoint PlantsTextOffset = new XPoint(0, .03);
        protected static readonly float PlantsOffset = .0125f;

        protected static readonly string OfferingsIcon = "h5_jug";
        protected static readonly XFont OfferingsFont = new XFont("Tahoma", 12, XFontStyle.Regular);
        protected static readonly XBrush OfferingsBrush = XBrushes.White;
        protected static readonly XBrush OfferingsBorderBrush = XBrushes.DarkGoldenrod;
        protected static readonly float OfferingsOffset = .0125f;
        protected static readonly XSize OfferingsSize = new XSize(.275, .45);
        protected static readonly XPoint OfferingsDefaultCenter = new XPoint(2.5 - .2375, 3.5 - .325);
        protected static readonly XPoint OfferingsTextOffset = new XPoint(0, 0.075);

        protected static readonly XPoint SymbolCenter = new XPoint(1.25, 3.5 - .2125);
        protected static readonly XSize SymbolSize = new XSize(.2125, .2125);

        protected static readonly XRect OfferingsRect = CenteredAround(OfferingsDefaultCenter, OfferingsSize);
        protected static readonly XRect OfferingsTextRect = CenteredAround(new XPoint(OfferingsDefaultCenter.X + OfferingsTextOffset.X, OfferingsDefaultCenter.Y + OfferingsTextOffset.Y), OfferingsSize);
        protected static readonly XRect SymbolRect = CenteredAround(SymbolCenter, SymbolSize);

        public void Draw(Context context, XGraphics gfx, XRect bounds, CardData card, PrintOptions options)
        {
            Draw(context, gfx, bounds, (T)card, options);

            DrawDebugRect(options, gfx, ScaleRect(IdRect, bounds));
            gfx.DrawString(card.SpreadsheetName + " " + card.Id, IdFont, IdBrush, ScaleRect(IdRect, bounds), XStringFormats.Default);
            gfx.DrawString("Count: " + card.Count, IdFont, IdBrush, ScaleRect(CountRect, bounds), XStringFormats.Default);
        }

        public virtual void Draw(Context context, XGraphics gfx, XRect bounds, T card, PrintOptions options)
        {
            throw new NotImplementedException();
        }

        protected static XRect ScaleRect(XRect rect, XRect bounds)
        {
            double xOffset = bounds.X + (bounds.Width - 2.5) / 2;
            double yOffset = bounds.Y + (bounds.Height - 3.5) / 2;

            return new XRect(
                new XUnit(xOffset + rect.X, XGraphicsUnit.Inch),
                new XUnit(yOffset + rect.Y, XGraphicsUnit.Inch),
                new XUnit(rect.Width, XGraphicsUnit.Inch),
                new XUnit(rect.Height, XGraphicsUnit.Inch));
        }

        protected static XRect CenteredAround(XPoint point, XSize size)
        {
            return new XRect(point.X - size.Width / 2, point.Y - size.Height / 2, size.Width, size.Height);
        }

        protected static void TryDrawImage(XGraphics gfx, XImage image, XRect bounds)
        {
            if (image != null)
                gfx.DrawImage(image, bounds);
        }

        protected static void DrawDebugRect(PrintOptions options, XGraphics gfx, XRect bounds)
        {
            if (options.Debug)
                gfx.DrawRectangle(XPens.Red, bounds);
        }

        protected static void DrawBorderedText(XGraphics gfx, string text, XFont font, XBrush mainBrush, XBrush backBrush, XRect centerRect, XRect bounds, float offset)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    float borderOffset = offset;
                    //if (i != 0 && j != 0)
                    //    borderOffset *= .7f;

                    XRect borderRect = centerRect + new XPoint(i * borderOffset, j * borderOffset);

                    gfx.DrawString(text, font, backBrush, ScaleRect(borderRect, bounds), XStringFormats.Center);
                }
            }

            gfx.DrawString(text, font, mainBrush, ScaleRect(centerRect, bounds), XStringFormats.Center);
        }
    }
}
