using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ByteMap.BLL
{
    public class ImageProcessor
    {
        public byte[] GetBytes(Bitmap img)
        {
            unsafe
            {
                var w = img.Width;
                var h = img.Height;
                var data = img.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var p = (byte*)data.Scan0.ToPointer();
                var pan = data.Stride - (w * 3);

                var result = new byte[w * h * 3];

                var i = 0;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            result[(y * w + x) * 3 + c] = p[i];
                            i++;
                        }
                    }

                    i += pan;
                }

                img.UnlockBits(data);
                return result;
            }
        }

        public Bitmap GetBitmap(byte[] src, int w, int h, Bitmap dst = null)
        {
            unsafe
            {
                var result = dst ?? new Bitmap(w, h);
                var data = result.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                var p = (byte*)data.Scan0.ToPointer();
                var stride = data.Stride;

                var i = 0;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            p[y * stride + x * 3 + c] = src[i];
                            i++;
                        }
                    }
                }

                result.UnlockBits(data);
                return result;
            }
        }
    }
}
