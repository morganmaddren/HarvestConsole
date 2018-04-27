using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole.Formatters
{
    class Spell52Formatter : Card52Formatter<SpellCardData>
    {
        static readonly XRect TemplateRect = new XRect(-.125, -.125, 2.75, 3.75);

        static readonly XFont TitleFont = new XFont("Candara", 13, XFontStyle.Regular);
        static readonly XBrush TitleBrush = XBrushes.Black;
        static readonly XPoint TitleCenter = new XPoint(1.25, 0.325);
        static readonly XSize TitleSize = new XSize(1.725, .275);

        static readonly XFont TextFont = new XFont("Verdana", 7, XFontStyle.Regular);
        static readonly XBrush TextBrush = XBrushes.Black;
        static readonly XPoint TextCenter = new XPoint(1.25, 2.8);
        static readonly XSize TextSize = new XSize(1.75, .75);

        static readonly XPoint PlantsCenter = new XPoint(.365, 0.365);

        static readonly string ColorIconSuffix = "_sigil2";
        static readonly XSize ColorSize = new XSize(.5, .5);
        static readonly XPoint ColorCenter = new XPoint(.2, 3.5 - .6);
        static readonly XPoint ColorCenter2 = new XPoint(2.5 - ColorCenter.X, ColorCenter.Y);
        
        static readonly XSize CropReqSize = new XSize(.2125, .2125);
        static readonly XPoint CropReqCenter = new XPoint(.365 - .1, .6);
        static readonly XSize CropReqIncrementOffset = new XSize(0, .25);

        static readonly XFont SeasonFont = new XFont("Arial", 10, XFontStyle.Regular);
        static readonly XBrush SeasonBrush = XBrushes.Black;

        static readonly XRect SeasonRect = new XRect(1.5, 3.5 - .05, 1, 0);

        // calculated properties
        static readonly XRect TitleRect = CenteredAround(TitleCenter, TitleSize);
        static readonly XRect TextRect = CenteredAround(TextCenter, TextSize);
        static readonly XRect PlantsRect = CenteredAround(PlantsCenter, PlantsSize);
        static readonly XRect PlantsTextRect = CenteredAround(new XPoint(PlantsCenter.X + PlantsTextOffset.X, PlantsCenter.Y + PlantsTextOffset.Y), PlantsSize);
        static readonly XRect ColorRect = CenteredAround(ColorCenter, ColorSize);
        static readonly XRect ColorRect2 = CenteredAround(ColorCenter2, ColorSize);

        public override void Draw(Context context, XGraphics gfx, XRect bounds, SpellCardData card, PrintOptions options)
        {
            if (!options.NoPic)
            {
                TryDrawImage(gfx, context.CardImageManager.GetImage(card.Id) ?? context.CardImageManager.GetImage("blank"), ScaleRect(TemplateRect, bounds));
                TryDrawImage(gfx, context.CardImageManager.GetImage("screen"), ScaleRect(TemplateRect, bounds));
            }

            TryDrawImage(gfx, context.TemplateManager.GetImage("h52_" + card.TemplateString), ScaleRect(TemplateRect, bounds));

            if (card.CropRequirement > 0)
            {
                for (int i = 0; i < card.CropRequirement; i++)
                {
                    XRect r = new XRect(CropReqCenter.X + CropReqIncrementOffset.Width * i, CropReqCenter.Y + CropReqIncrementOffset.Height * i, CropReqSize.Width, CropReqSize.Height);
                    TryDrawImage(gfx, context.TemplateManager.GetImage(card.Color + "_symbol"), ScaleRect(r, bounds));
                }
            }

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
            TryDrawImage(gfx, context.TemplateManager.GetImage(card.Color + "_symbol"), ScaleRect(SymbolRect, bounds));

            TryDrawImage(gfx, context.TemplateManager.GetImage(TemplateImages.cut_border), ScaleRect(TemplateRect, bounds));
        }
    }
}
