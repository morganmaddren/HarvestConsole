using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole.Formatters
{
    class CardBackFormatter : CardFormatter<BackCardData>
    {
        static readonly XRect TemplateRect = new XRect(-.125, -.125, 2.75, 3.75);

        static readonly XFont TitleFont = new XFont("Candara", 13, XFontStyle.Regular);
        static readonly XBrush TitleBrush = XBrushes.Black;
        static readonly XPoint TitleCenter = new XPoint(1.25, 0.325);
        static readonly XSize TitleSize = new XSize(1.725, .275);

        static readonly XFont TextFont = new XFont("Verdana", 8, XFontStyle.Regular);
        static readonly XBrush TextBrush = XBrushes.Black;
        static readonly XPoint TextCenter = new XPoint(1.25, 2.875);
        static readonly XSize TextSize = new XSize(1.75, .75);

        static readonly string PlantsIcon = "hand";
        static readonly XFont PlantsFont = new XFont("Tahoma", 18, XFontStyle.Regular);
        static readonly XBrush PlantsBrush = XBrushes.Black;
        static readonly XSize PlantsSize = new XSize(.5, .5);
        static readonly XPoint PlantsCenter = new XPoint(.365, 0.365);
        static readonly XPoint PlantsTextOffset = new XPoint(0, .03);

        static readonly string OfferingsIcon = "h5_jug";
        static readonly XFont OfferingsFont = new XFont("Tahoma", 10, XFontStyle.Regular);
        static readonly XBrush OfferingsBrush = XBrushes.White;
        static readonly XSize OfferingsSize = new XSize(.35, .35);
        static readonly XPoint OfferingsCenter = new XPoint(2.5 - .25, 3.5 - .25);
        static readonly XPoint OfferingsTextOffset = new XPoint(-.01, 0);

        static readonly string ColorIconSuffix = "_sigil2";
        static readonly XSize ColorSize = new XSize(.5, .5);
        static readonly XPoint ColorCenter = new XPoint(.2, 3.5 - .6);
        static readonly XPoint ColorCenter2 = new XPoint(2.5 - ColorCenter.X, ColorCenter.Y);

        static readonly string CropReqIcon = "leaf";
        static readonly XSize CropReqSize = new XSize(.25, .25);
        static readonly XPoint CropReqCenter = new XPoint(.5, .4);
        static readonly XSize CropReqIncrementOffset = new XSize(.2, 0);

        static readonly XFont SeasonFont = new XFont("Arial", 10, XFontStyle.Regular);
        static readonly XBrush SeasonBrush = XBrushes.Black;

        static readonly XRect SeasonRect = new XRect(1.5, 3.5 - .05, 1, 0);

        // calculated properties
        static readonly XRect TitleRect = CenteredAround(TitleCenter, TitleSize);
        static readonly XRect TextRect = CenteredAround(TextCenter, TextSize);
        static readonly XRect PlantsRect = CenteredAround(PlantsCenter, PlantsSize);
        static readonly XRect PlantsTextRect = CenteredAround(new XPoint(PlantsCenter.X + PlantsTextOffset.X, PlantsCenter.Y + PlantsTextOffset.Y), PlantsSize);
        static readonly XRect OfferingsRect = CenteredAround(OfferingsCenter, OfferingsSize);
        static readonly XRect OfferingsTextRect = CenteredAround(new XPoint(OfferingsCenter.X + OfferingsTextOffset.X, OfferingsCenter.Y + OfferingsTextOffset.Y), OfferingsSize);
        static readonly XRect ColorRect = CenteredAround(ColorCenter, ColorSize);
        static readonly XRect ColorRect2 = CenteredAround(ColorCenter2, ColorSize);

        public override void Draw(Context context, XGraphics gfx, XRect bounds, BackCardData card, PrintOptions options)
        {
            TryDrawImage(gfx, context.TemplateManager.GetImage(card.TemplateString), ScaleRect(TemplateRect, bounds));

            TryDrawImage(gfx, context.TemplateManager.GetImage(TemplateImages.cut_border), ScaleRect(TemplateRect, bounds));
        }
    }
}
