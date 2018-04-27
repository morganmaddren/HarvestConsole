using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace HarvestConsole.Typesetting
{
    class Typesetter
    {
        public static void Typeset(Context context, XGraphics gfx, string text, XFont font, XBrush brush, XRect bounds)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            List<string> tokens = StringTokenizer.Tokenize(text);
            List<List<ISetObject>> lines = new List<List<ISetObject>>();
            lines.Add(new List<ISetObject>());

            var lineHeight = gfx.MeasureString("A", font).Height;
            XUnit curLength = 0;
            XUnit curHeight = 0;
            XFont boldedFont = new XFont(font.Name, font.Size, XFontStyle.Bold);

            bool bolded = false;

            foreach (string token in tokens)
            {
                ISetObject obj = null;
                if (token.StartsWith("$"))
                {
                    if (token == "$n")
                    {
                        if (curLength != 0)
                        {
                            lines.Add(new List<ISetObject>());
                            curHeight += lineHeight;
                        }
                        curLength = 0;
                        continue;
                    }

                    if (token == "$np")
                    {
                        if (curLength != 0)
                        {
                            lines.Add(new List<ISetObject>());
                            curHeight += lineHeight * 1.25;
                        }
                        curLength = 0;
                        continue;
                    }
                    
                    if (token == "$b")
                    {
                        bolded = !bolded;
                        continue;
                    }

                    XImage image = context.MacroManager.GetImageMacro(token);
                    if (image != null)
                    {
                        obj = new SetImage(gfx, image);
                    }
                }
                
                if (obj == null)
                {
                    obj = new SetString(gfx, token, bolded ? boldedFont : font);
                }

                if (curLength + obj.Size.Width > bounds.Width)
                {
                    lines.Add(new List<ISetObject>());
                    curLength = 0;
                    curHeight += lineHeight;

                    // Whitespace doesn't need to get added to beginning of next line;
                    if (token == " ")
                    {
                        continue;
                    }
                }

                var curLine = lines.Last();

                curLine.Add(obj);
                obj.Position = new XVector(curLength, curHeight);
                curLength += obj.Size.Width;
            }

            if (lines.Last().Count == 0)
            {
                lines.RemoveAt(lines.Count - 1);
            }

            // center lines vertical & horiz
            var bottom = lines.Last().First().Position.Y - lineHeight / 2;
            var vertoffset = (bounds.Height - bottom) / 2;
            foreach (var line in lines)
            {
                if (!line.Any())
                    continue;

                var right = line.Last().Position.X + line.Last().Size.Width;
                var horizoffset = (bounds.Width - right) / 2;

                foreach (var word in line)
                {
                    word.Position = new XVector(word.Position.X + horizoffset, word.Position.Y + vertoffset);
                }
            }

            foreach (var line in lines)
            {
                foreach (var word in line)
                {
                    word.Position = new XVector(word.Position.X + bounds.Left, word.Position.Y + bounds.Top);
                    word.Draw();
                }
            }

        }
    }
}
