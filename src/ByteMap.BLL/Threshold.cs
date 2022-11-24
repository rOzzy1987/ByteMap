using System;
using System.Collections.Generic;
using System.Text;

namespace ByteMap.BLL
{
    public class Threshold
    {
        public byte Value { get; set; }

        public byte[] Transform(byte[] src)
        {
            var result = new byte[src.Length];
            for (int i = 0; i < src.Length; i+=3)
            {
                var avg = (byte)(((int) src[i] + src[i + 1] + src[i + 2]) / 3);
                result[i] = result[i + 1] = result[i + 2] = (byte)(avg > Value ? 255 : 0);

            }

            return result;
        }
    }
}
