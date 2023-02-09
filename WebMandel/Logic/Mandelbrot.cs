using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WebMandel.Logic
{
    public class Mandelbrot : IMandelbrot
    {
        const int width = 800;
        const int height = 600;
        public double XMin { get; set; }
        public double YMax { get; set; }
        public double Scale { get; set; }
        public int Iterations { get; set; }

        public Mandelbrot()
        {
            Scale = 4.0 / width;
            XMin = -2.5;
            YMax = 1.5;
            Iterations = 500;
        }

        bool IsMandel(Complex z)
        {
            Complex u = 0;

            for (int i = 0; i < Iterations; i++)
            {
                u = u * u + z;
                if (u.Mod2() > 4) return false;
            }
            return true;
        }

        BitmapSource MakeBitmap()
        {
            var ret = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
            int stride = ret.BackBufferStride;
            var buf = new byte[height * stride];
            Parallel.For(0, height, line =>
                {
                    int lineStart = line * stride;
                    double y = YMax - Scale * line;
                    for (int col = 0; col < width; col++)
                    {
                        Complex z = new Complex(XMin + Scale * col, y);
                        if (!IsMandel(z))
                        {
                            int colIdx = col * 3;
                            int idx = lineStart + colIdx;
                            buf[idx] = 255;
                            buf[idx + 1] = 255;
                            buf[idx + 2] = 255;
                        }
                    }
                });
            ret.WritePixels(new Int32Rect(0, 0, width, height), buf, stride, 0, 0);
            return ret;
        }

        public byte[] GenerateMandelImage()
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(MakeBitmap()));
            var ms = new MemoryStream();
            enc.Save(ms);
            return ms.ToArray();
        }
    }
}
