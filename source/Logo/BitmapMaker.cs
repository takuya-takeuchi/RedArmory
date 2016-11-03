using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Logo
{
    public static class BitmapMaker
    {
        public static void Start(BitmapMakerArgs args)
        {
            var dirName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var fileName = Path.GetFileNameWithoutExtension(args.FileName);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = DateTime.Now.ToString("ffffff");
            }
            var extension = Path.GetExtension(args.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".png";
            }
            var scaleTransform = new ScaleTransform();
            args.Element.RenderTransform = scaleTransform;
            args.Element.UpdateLayout();

            // Create the directory to deploy image files to.
            Directory.CreateDirectory(dirName);

            // Save an image file for the original image.
            SaveAsImage(args.Element, scaleTransform, dirName, fileName, extension);

            // Save image files for the original image and the specified sizes.
            foreach (var size in args.ImageSizes)
            {
                SaveAsImage(args.Element, scaleTransform, dirName, fileName, extension, false, size);
            }

            if (args.CreateBlackVersion)
            {
                // Blacken elements on the canvas.
                SetBlackImage(args.Element);

                // Save an image file for the blackened image.
                SaveAsImage(args.Element, scaleTransform, dirName, fileName, extension, true);

                // Save image files for the blackened image and the specified sizes.
                foreach (var size in args.ImageSizes)
                {
                    SaveAsImage(args.Element, scaleTransform, dirName, fileName, extension, true, size);
                }
            }

            // Open the directory that image files have been deployed to.
            var filePath_original = CreateFilePath(dirName, fileName, extension);
            Process.Start("explorer", string.Format("/select,\"{0}\"", filePath_original));
            Application.Current.Shutdown();
        }

        static void SaveAsImage(FrameworkElement element, ScaleTransform scaleTransform, string dirName, string fileName, string extension, bool isBlack = false, Size? size = null)
        {
            scaleTransform.ScaleX = size.HasValue ? size.Value.Width / element.ActualWidth : 1.0;
            scaleTransform.ScaleY = size.HasValue ? size.Value.Height / element.ActualHeight : 1.0;

            var bitmap = size.HasValue ? BitmapUtility.CreateImage(element, size.Value) : BitmapUtility.CreateImage(element);
            var filePath = CreateFilePath(dirName, fileName, extension, isBlack, size);
            BitmapUtility.SaveImage(filePath, bitmap);
        }

        static string CreateFilePath(string dirName, string fileName, string extension, bool isBlack = false, Size? size = null)
        {
            var sizeString = size.HasValue ? string.Format("-{0}x{1}", size.Value.Width, size.Value.Height) : "";
            return Path.Combine(dirName, string.Format("{0}{2}{3}{1}", fileName, extension, isBlack ? "-black" : "", sizeString));
        }

        static void SetBlackImage(FrameworkElement element)
        {
            if (element is Panel)
            {
                var shapes = ((Panel)element).Children.OfType<System.Windows.Shapes.Shape>();
                var blackBrush = new SolidColorBrush(Colors.Black);
                foreach (var shape in shapes)
                {
                    shape.Fill = blackBrush;
                }
                element.UpdateLayout();
            }
        }
    }

    public class BitmapMakerArgs
    {
        public FrameworkElement Element { get; set; }
        public string FileName { get; set; }
        public bool CreateBlackVersion { get; set; }
        public Size[] ImageSizes { get; set; }
    }
}
