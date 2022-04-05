using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BarleyBreak
{
    internal static class BarelyBreakUtils
    {
        public static BitmapImage LoadImage(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            return image;
        }

        public static BitmapSource[,] CutImage(BitmapImage source, int rows, int columns)
        {
            BitmapSource[,] cropped = new BitmapSource[rows, columns];

            int width = source.PixelWidth / columns;
            int height = source.PixelWidth / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var rect = new Int32Rect(j * width, i * height, width, height);
                    cropped[i, j] = new CroppedBitmap(source, rect);
                }
            }

            return cropped;
        }

        public static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj, Func<T, bool> filter) where T : DependencyObject
        {
            if (depObj == null)
            {
                yield return (T)Enumerable.Empty<T>();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);

                if (ithChild == null)
                {
                    continue;
                }

                if (ithChild is T t && filter(t))
                {
                    yield return t;
                }

                foreach (T childOfChild in FindVisualChilds(ithChild, filter))
                {
                    yield return childOfChild;
                }
            }
        }
    }
}
