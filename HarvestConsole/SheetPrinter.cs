using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using HarvestConsole.Commands;

namespace HarvestConsole
{
    class SheetPrinter
    {
        Context context;
        PrintOptions options;

        public SheetPrinter(Context context, PrintOptions options)
        {
            this.options = options;
            this.context = context;
        }

        public void CreatePDF(CardDataSpreadsheet sheet)
        {
            var doc = new PdfDocument();
            doc.Info.Title = sheet.Name;
            
            var totalCards = options.NoCount ? sheet.Cards.Count() : sheet.Cards.Sum(x => x.Count);
            
            PdfPage curPage = null;
            XGraphics gfx = null;
            int itemsOnPage = 0;
            foreach (var card in sheet.Cards)
            {
                int count = options.NoCount ? 1 : card.Count;
                for (int i = 0; i < count; i++)
                {
                    if (curPage == null)
                    {
                        curPage = doc.AddPage(CreatePage());
                        gfx = XGraphics.FromPdfPage(curPage);
                        itemsOnPage = 0;
                    }

                    PrintCardOnPage(gfx, itemsOnPage, card);
                    itemsOnPage++;

                    if (itemsOnPage == 9)
                    {
                        PostPrintPage(gfx);
                        curPage = null;
                    }
                }
            }

            if (curPage != null)
            {
                PostPrintPage(gfx);
            }
            
            string filename = context.OutputDirectory + @"\" + sheet.Name + "_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + ".pdf";
            doc.Save(filename);

            Process.Start(filename);
        }

        private PdfPage CreatePage()
        {
            var page = new PdfPage();
            page.Size = PageSize.Letter;

            return page;
        }

        private void PrintCardOnPage(XGraphics gfx, int cardIndex, CardData card)
        {
            double boxHeight = gfx.PdfPage.Height.Inch / 3;
            double boxWidth = gfx.PdfPage.Width.Inch / 3;

            int x = cardIndex % 3;
            int y = cardIndex / 3;
           
            XRect bounds = new XRect(x * boxWidth, y * boxHeight, boxWidth, boxHeight);

            context.FormattingManager.Draw(gfx, bounds, card, options);
        }

        private void PostPrintPage(XGraphics gfx)
        {
            double boxHeight = gfx.PdfPage.Height / 3;
            double boxWidth = gfx.PdfPage.Width / 3;

            XPen pen = new XPen(XColors.LightGray, .5);
            pen.DashStyle = XDashStyle.Dot;

            for (int i = 1; i < 3; i++)
            {
                // horizontal line
                gfx.DrawLine(pen, new XPoint(0, boxHeight * i), new XPoint(gfx.PdfPage.Width, boxHeight * i));

                // vertical line
                gfx.DrawLine(pen, new XPoint(boxWidth * i, 0), new XPoint(boxWidth * i, gfx.PdfPage.Height));

            }


        }
    }
}
