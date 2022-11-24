using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ByteMap.BLL
{
    public class Dotter
    {
        private ImageProcessor _ip;

        public Dotter(ImageProcessor ip)
        {
            _ip = ip;
        }

        public Bitmap Dot(byte[] data, int srcW, int srcH)
        {
            var ow = Math.Min(srcW, 128);
            var oh = Math.Min(srcH, 64);
            var w = ow * 2;
            var h = oh * 2;

            byte[] result = new byte[w * h * 3];

            for (int y = 0; y < oh; y++)
            {
                var rd = y * w * 6;
                var hr = w * 3;
                for (int x = 0; x < ow; x++)
                {
                    var cd = x * 6;
                    result[rd +cd + 0] =
                    result[rd +cd + 1] =
                    result[rd +cd + 2] =
                        (byte)(data[(y * srcW + x) * 3] > 0 ? 0xFF : 0x00);

                    result[rd +cd + 3] =
                    result[rd +cd + 4] =
                    result[rd +cd + 5] =
                    result[rd + hr + cd + 0] =
                    result[rd + hr + cd + 1] =
                    result[rd + hr + cd + 2] =
                        (byte)(data[(y * srcW + x) * 3] > 0 ? 0xA0 : 0x00);


                    result[rd + hr + cd + 3] =
                    result[rd + hr + cd + 4] =
                    result[rd + hr + cd + 5] =
                        (byte)(data[(y * srcW + x) * 3] > 0 ? 0x50 : 0x00);
                }
            }

            return _ip.GetBitmap(result, w, h);
        }
    }
}
