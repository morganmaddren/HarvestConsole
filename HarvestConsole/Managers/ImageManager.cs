using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp.Drawing;

namespace HarvestConsole.Managers
{
    class ImageManager
    {
        string directory;
        Dictionary<string, string> imageNames;

        public ImageManager(string directory)
        {
            this.directory = directory;
            this.imageNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.Refresh();
        }
        public void Refresh()
        {
            imageNames.Clear();
            foreach (var f in Directory.EnumerateFiles(this.directory).Where(x => x.EndsWith(".png")))
            {
                string name = new FileInfo(f).Name.Replace(".png", "");
                imageNames[name] = f;
            }
        }

        public XImage GetImage(string name)
        {
            if (!imageNames.ContainsKey(name))
                return null;

            XImage image = XImage.FromFile(imageNames[name]);
            return image;
        }
    }
}
