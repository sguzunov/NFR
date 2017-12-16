using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloydSteinberg
{
    public class Program
    {
        public static void Main()
        {
            var files = ReadDirectory("./../Input");
            foreach (var file in files)
            {
                var outputImageBm = FloydSteinberg(file);
                outputImageBm.Save("./../Output/cat-error-diffusion.jpg");
            }
        }

        private static Bitmap FloydSteinberg(string file)
        {
            var bm = new Bitmap(file);
            int width = bm.Width;
            int height = bm.Height;

            var outputBm = new Bitmap(width, height);
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    var oldPixel = bm.GetPixel(x, y);
                    var newPixel = Approximate(oldPixel.R);
                    bm.SetPixel(x, y, Color.FromArgb(newPixel, newPixel, newPixel));

                    var error = oldPixel.R - newPixel;

                    int r1 = bm.GetPixel(x + 1, y).R + (7 * error / 16);
                    int r2 = bm.GetPixel(x - 1, y + 1).R + (3 * error / 16);
                    int r3 = bm.GetPixel(x, y + 1).R + (5 * error / 16);
                    int r4 = bm.GetPixel(x + 1, y + 1).R + (1 * error / 16);

                    if (r1 > 255) r1 = 255;
                    if (r2 > 255) r2 = 255;
                    if (r3 > 255) r3 = 255;
                    if (r4 > 255) r4 = 255;
                    outputBm.SetPixel(x + 1, y, Color.FromArgb(r1, r1, r1));
                    outputBm.SetPixel(x - 1, y + 1, Color.FromArgb(r2, r2, r2));
                    outputBm.SetPixel(x, y + 1, Color.FromArgb(r3, r3, r3));
                    outputBm.SetPixel(x + 1, y + 1, Color.FromArgb(r4, r4, r4));
                }
            }

            return outputBm;
        }

        private static int Approximate(byte c)
        {
            if (c >= 0 && c < 63)
                return 0;
            if (c >= 63 && c < 127)
                return 63;
            if (c >= 127 && c < 191)
                return 127;
            if (c >= 191 && c < 255)
                return 191;
            else
                return 0;
        }

        private static ICollection<string> ReadDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return new List<string>();
            }

            return Directory.GetFiles("./../Input");
        }
    }
}
