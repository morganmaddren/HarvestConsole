using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using HarvestConsole.Commands;

namespace HarvestConsole.Formatters
{
    class Crop52Formatter : Card52Formatter<CropCardData>
    {
        static readonly XRect TemplateRect = new XRect(-.125, -.125, 2.75, 3.75);

        static readonly XFont TitleFont = new XFont("Candara", 13, XFontStyle.Regular);
        static readonly XBrush TitleBrush = XBrushes.Black;
        static readonly XPoint TitleCenter = new XPoint(1.25, 2.1375);
        static readonly XSize TitleSize = new XSize(1.725, .275);

        static readonly string DewIcon = "dew";
        static readonly string HarvestDewIcon = "harvestcosticon";
        static readonly string NoWhiteDewIcon = "nowhiteicon";
        static readonly XSize DewSize = new XSize(.325, .325);
        static readonly XPoint DewTitleCenter = new XPoint(.45, 2.175);
        static readonly XSize DewTitleSize = new XSize(.375, .3675);
        static readonly XPoint DewCenter = new XPoint(.45, 2.825);
        static readonly XPoint DewTopCenter = new XPoint(.45, 2.575);
        static readonly XPoint DewBottomCenter = new XPoint(.45, 3.025);
        static readonly XFont DewFont = new XFont("Tahoma", 11, XFontStyle.Bold);
        static readonly XFont DewTitleFont = new XFont("Tahoma", 12, XFontStyle.Bold);
        static readonly XBrush DewBrush = XBrushes.White;
        static readonly XBrush DewBorderBrush = XBrushes.Black;
        static readonly float DewOffset = .0125f;

        static readonly XFont TextFont = new XFont("Verdana", 7, XFontStyle.Regular);
        static readonly XBrush TextBrush = XBrushes.Black;
        static readonly XPoint TextCenter = new XPoint(1.375, 2.8);
        static readonly XSize TextSize = new XSize(1.5, .75);

        static readonly XPoint TextTopCenter = new XPoint(1.375, 2.575);
        static readonly XSize TextTopSize = new XSize(1.5, .375);

        static readonly XPoint TextBottomCenter = new XPoint(1.375, 3);
        static readonly XSize TextBottomSize = new XSize(1.5, .375);

        static readonly string LineImage = "line";
        static readonly XSize LineSize = new XSize(1.75, .03125);
        static readonly XPoint LineCenter = new XPoint(1.25, 2.8);

        static readonly XPoint PlantsCenter = new XPoint(.365, 0.365);

        static readonly string GrowsIcon = "dew3";
        static readonly XFont GrowsFont = new XFont("Tahoma", 16, XFontStyle.Regular);
        static readonly XBrush GrowsBrush = XBrushes.White;
        static readonly XSize GrowsSize = new XSize(.5, .5);
        static readonly XPoint GrowsCenter = new XPoint(2.25, .35);
        static readonly XPoint GrowsTextOffset = new XPoint(0, .03);

        // calculated properties
        static readonly XRect TitleRect = CenteredAround(TitleCenter, TitleSize);
        static readonly XRect TextRect = CenteredAround(TextCenter, TextSize);
        static readonly XRect TextTopRect = CenteredAround(TextTopCenter, TextTopSize);
        static readonly XRect TextBottomRect = CenteredAround(TextBottomCenter, TextBottomSize);
        static readonly XRect PlantsRect = CenteredAround(PlantsCenter, PlantsSize);
        static readonly XRect PlantsTextRect = CenteredAround(new XPoint(PlantsCenter.X + PlantsTextOffset.X, PlantsCenter.Y + PlantsTextOffset.Y), PlantsSize);
        static readonly XRect GrowsRect = CenteredAround(GrowsCenter, GrowsSize);
        static readonly XRect GrowsTextRect = CenteredAround(new XPoint(GrowsCenter.X + GrowsTextOffset.X, GrowsCenter.Y + GrowsTextOffset.Y), GrowsSize);
        static readonly XRect LineRect = CenteredAround(LineCenter, LineSize);
        static readonly XRect DewTitleRect = CenteredAround(DewTitleCenter, DewTitleSize);
        static readonly XRect DewRect = CenteredAround(DewCenter, DewSize);
        static readonly XRect DewTopRect = CenteredAround(DewTopCenter, DewSize);
        static readonly XRect DewBottomRect = CenteredAround(DewBottomCenter, DewSize);

        public override void Draw(Context context, XGraphics gfx, XRect bounds, CropCardData card, PrintOptions options)
        {
            if (!options.NoPic)
            {
                TryDrawImage(gfx, context.CardImageManager.GetImage(card.Id) ?? context.CardImageManager.GetImage("blank"), ScaleRect(TemplateRect, bounds));
                TryDrawImage(gfx, context.CardImageManager.GetImage("screen"), ScaleRect(TemplateRect, bounds));
            }

            TryDrawImage(gfx, context.TemplateManager.GetImage("h52_" + card.TemplateString), ScaleRect(TemplateRect, bounds));

            DrawDebugRect(options, gfx, ScaleRect(TitleRect, bounds));
            gfx.DrawString(card.Title, TitleFont, TitleBrush, ScaleRect(TitleRect, bounds), XStringFormats.Center);

            DrawDebugRect(options, gfx, ScaleRect(DewTitleRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(DewIcon), ScaleRect(DewTitleRect, bounds));
            DrawBorderedText(gfx, card.HarvestCost.ToString(), DewTitleFont, DewBrush, DewBorderBrush, DewTitleRect, bounds, DewOffset);

            XRect harvestRect = TextRect;
            XRect dewRect = DewRect;
            if (!string.IsNullOrEmpty(card.Effect))
            {
                harvestRect = TextBottomRect;
                dewRect = DewBottomRect;

                DrawDebugRect(options, gfx, ScaleRect(DewTopRect, bounds));

                if (card.EffectType == "nowp")
                    TryDrawImage(gfx, context.TemplateManager.GetImage(NoWhiteDewIcon), ScaleRect(DewTopRect, bounds));
                TryDrawImage(gfx, context.TemplateManager.GetImage(DewIcon), ScaleRect(DewTopRect, bounds));
                DrawBorderedText(gfx, card.EffectCost.ToString(), DewFont, DewBrush, DewBorderBrush, DewTopRect, bounds, DewOffset);

                //string effectheader = "$branchleft $b ON TURN $b " + string.Concat(Enumerable.Repeat("$dew ", card.EffectCost)) + "$branchright $n ";
                DrawDebugRect(options, gfx, ScaleRect(TextTopRect, bounds));
                Typesetting.Typesetter.Typeset(context, gfx, card.Effect, TextFont, TextBrush, ScaleRect(TextTopRect, bounds));

                TryDrawImage(gfx, context.TemplateManager.GetImage(LineImage), ScaleRect(LineRect, bounds));
            }

            DrawDebugRect(options, gfx, ScaleRect(dewRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(HarvestDewIcon), ScaleRect(dewRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(DewIcon), ScaleRect(dewRect, bounds));
            DrawBorderedText(gfx, card.HarvestCost.ToString(), DewFont, DewBrush, DewBorderBrush, dewRect, bounds, DewOffset);

            DrawDebugRect(options, gfx, ScaleRect(harvestRect, bounds));
            //string header = "$harvestleft $b HARVEST $b " + string.Concat(Enumerable.Repeat("$dew ", card.HarvestCost)) + "$harvestright $n ";
            Typesetting.Typesetter.Typeset(context, gfx, card.HarvestEffect, TextFont, TextBrush, ScaleRect(harvestRect, bounds));

            DrawDebugRect(options, gfx, ScaleRect(PlantsRect, bounds));
            TryDrawImage(gfx, context.TemplateManager.GetImage(PlantsIcon), ScaleRect(PlantsRect, bounds));
            DrawBorderedText(gfx, card.PlantCost.ToString(), PlantsFont, PlantsBrush, PlantsBorderBrush, PlantsTextRect, bounds, PlantsOffset);

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

        private static void DrawCaps(Context context, XGraphics gfx, XRect bounds, string text)
        {
            /*string firstLetter = text[0].ToString();
            var firstLetterBounds = gfx.MeasureString(firstLetter, TitleFirstLetterFont);
            string rest = text.Substring(1);
            var restBounds = gfx.MeasureString(rest, TitleFont);
            gfx.DrawString(firstLetter, TitleFirstLetterFont, TitleBrush)*/
        }
    }
}
