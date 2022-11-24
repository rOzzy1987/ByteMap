using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ByteMap.BLL
{
    public class HistogramCalculator
    {
        public byte[] Calculate(byte[] data)
        {
            var result = new double[256];
            var max = 0.0;
            for (int i = 0; i < data.Length; i += 3)
            {
                var avg = (data[i] + data[i + 1] + data[i + 2]) / 3;
                result[avg]++;
                if (result[avg] > max)
                {
                    max = result[avg];
                }
            }

            var hist = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                hist[i] = (byte)(result[i] * 255 / max);
            }

            return hist;
        }
    }

    public class HistogramRenderer
    {
        private ImageProcessor _ip;

        public HistogramRenderer(ImageProcessor ip)
        {
            _ip = ip;
        }

        public Bitmap Render(byte[] histogram, int redLine, int h = 64)
        {
            var w = 256;
            var result = new byte[w * h * 3];

            for (int x = 0; x < w; x++)
            {
                var cd = x * 3;
                var th = h - (histogram[x] * h / 255);
                var pth = h - (x == 0 ? 255 : histogram[x - 1] * h / 255);
                var nth = h - (x == 255 ? 255 : histogram[x + 1] * h / 255);
                for (int y = 0; y < h; y++)
                {
                    var rd = y * w * 3;

                    result[rd + cd] =
                    result[rd + cd + 1] =
                    result[rd + cd + 2] =
                        y >= th
                            ? (byte)0x00
                            : y > pth || y>nth
                                  ?(byte)0x80
                                  :(byte)0xFF;
                    if (x == redLine && result[rd + cd] > 0)
                    {
                        result[rd + cd] =
                            result[rd + cd + 1] = 0;
                    }
                }
            }

            return _ip.GetBitmap(result, 256, h);
        }
    }
}
