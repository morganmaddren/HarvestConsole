using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole.Formatters
{
    class Season52Formatter : Card52Formatter<SeasonCardData>
    {
        static readonly XRect TemplateRect = new XRect(-.125, -.125, 2.75, 3.75);

        static readonly XFont TitleFont = new XFont("Candara", 13, XFontStyle.Regular);
        static readonly XBrush TitleBrush = XBrushes.Black;
        static readonly XPoint TitleCenter = new XPoint(1.25, 0.325);
        static readonly XSize TitleSize = new XSize(1.725, .275);

        static readonly XFont TextFont = new XFont("Verdana", 8, XFontStyle.Regular);
        static readonly XBrush TextBrush = XBrushes.Black;
        static readonly XPoint TextCenter = new XPoint(1.25, 2.625);
        static readonly XSize TextSize = new XSize(2, .75);

        static readonly XPoint PlantsCenter = new XPoint(1.25, 3.5 - 0.275);

        static readonly XFont SeasonFont = new XFont("Verdana", 8, XFontStyle.Regular);
        static readonly XBrush SeasonBrush = XBrushes.Black;
        static readonly XRect SeasonRect = new XRect(1.3, 3.5 - .1, 1, 0);

        protected static readonly XPoint SymbolTitleLeft = new XPoint(.25, .325);
        protected static readonly XPoint SymbolTitleRight = new XPoint(2.5 - .25, .325);

        // calculated properties
        static readonly XRect TitleRect = CenteredAround(TitleCenter, TitleSize);
        static readonly XRect TextRect = CenteredAround(TextCenter, TextSize);
        static readonly XRect PlantsRect = CenteredAround(PlantsCenter, PlantsSize);
        static readonly XRect PlantsTextRect = CenteredAround(new XPoint(PlantsCenter.X + PlantsTextOffset.X, PlantsCenter.Y + PlantsTextOffset.Y), PlantsSize);

        public override void Draw(Context context, XGraphics gfx, XRect bounds, SeasonCardData card, PrintOptions options)
        {
            TryDrawImage(gfx, context.CardImageManager.GetImage(card.Id) ?? context.CardImageManager.GetImage("blank"), ScaleRect(TemplateRect, bounds));
            TryDrawImage(gfx, context.CardImageManager.GetImage("screen"), ScaleRect(TemplateRect, bounds));

            TryDrawImage(gfx, context.TemplateManager.GetImage("h5_" + card.Type + "_" + card.Season), ScaleRect(TemplateRect, bounds));

            DrawDebugRect(options, gfx, ScaleRect(TitleRect, bounds));
            gfx.DrawString(card.Title, TitleFont, TitleBrush, ScaleRect(TitleRect, bounds), XStringFormats.Center);

            DrawDebugRect(options, gfx, ScaleRect(TextRect, bounds));
            Typesetting.Typesetter.Typeset(context, gfx, card.Text, TextFont, TextBrush, ScaleRect(TextRect, bounds));

            DrawDebugRect(options, gfx, ScaleRect(PlantsRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(PlantsIcon), ScaleRect(PlantsRect, bounds));
            DrawBorderedText(gfx, card.PlantValue.ToString(), PlantsFont, PlantsBrush, PlantsBorderBrush, PlantsTextRect, bounds, PlantsOffset);

            DrawDebugRect(options, gfx, ScaleRect(OfferingsRect, bounds));
            if (card.Offerings > 0)
            {
                TryDrawImage(gfx, context.TemplateManager.GetImage(OfferingsIcon), ScaleRect(OfferingsRect, bounds));
                DrawBorderedText(gfx, card.Offerings.ToString(), OfferingsFont, OfferingsBrush, OfferingsBorderBrush, OfferingsTextRect, bounds, OfferingsOffset);
            }
            
            DrawDebugRect(options, gfx, ScaleRect(SymbolRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(card.Color + "_symbol"), ScaleRect(CenteredAround(SymbolTitleLeft, SymbolSize), bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(card.Color + "_symbol"), ScaleRect(CenteredAround(SymbolTitleRight, SymbolSize), bounds));

            TryDrawImage(gfx, context.TemplateManager.GetImage(TemplateImages.cut_border), ScaleRect(TemplateRect, bounds));
        }
    }
}
