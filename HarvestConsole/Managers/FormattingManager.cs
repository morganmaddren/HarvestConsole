using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using HarvestConsole.Formatters;
using HarvestConsole.Commands;

namespace HarvestConsole.Managers
{
    class FormattingManager
    {
        Dictionary<Type, Func<ICardFormatter>> formatters = new Dictionary<Type, Func<ICardFormatter>>()
        {
            [typeof(CropCardData)] = () => new Crop52Formatter(),
            [typeof(SpellCardData)] = () => new Spell52Formatter(),
            [typeof(SeasonCardData)] = () => new Season52Formatter(),
            [typeof(BackCardData)] = () => new CardBackFormatter()
        };
        
        Context context;
        public FormattingManager(Context context)
        {
            this.context = context;
        }
        
        public void Draw(XGraphics gfx, XRect bounds, CardData card, PrintOptions options)
        {
            ICardFormatter formatter = formatters[card.GetType()]();
            formatter.Draw(this.context, gfx, bounds, card, options);
        }
    }
}
