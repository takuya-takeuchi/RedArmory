using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Logo
{
    public static class IconUtility
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        public static void SaveIcon(Stream input, string outputFilePath)
        {
            var fullPath = Path.GetFullPath(outputFilePath);

            using (var stream = File.Create(fullPath))
            {
                SaveIcon(input, stream);
            }
        }

        public static void SaveIcon(Stream input, Stream output)
        {
            using (var bitmap = new Bitmap(input))
            {
                var handle = bitmap.GetHicon();
                using (var icon = Icon.FromHandle(handle))
                {
                    icon.Save(output);
                }
                DestroyIcon(handle);
            }
        }
    }
}
