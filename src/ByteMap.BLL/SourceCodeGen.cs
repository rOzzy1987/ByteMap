using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ByteMap.BLL
{
    public class SourceCodeGen
    {
        public string Generate(byte[] bitmap, int w, string varName)
        {
            varName = Regex.Replace(varName, "[^a-zA-Z0-9_]", "");
            var sb = new StringBuilder();
            var h = bitmap.Length/(w*3);
            sb.Append(
                $"// Generated monochrome bitmap\r\n" +
                $"// Size: {w}x{h}\r\n" +
                $"static const unsigned char PROGMEM {varName}[] = {{\r\n");
            var i = 0;

            var s = 0;

            for (int y = 0; y < h; y++)
            {
                sb.Append("  ");
                for (int x = 0; x < w; x += 8)
                {
                    byte b = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if (x + j >= w)
                            break;
                        b |= (byte)((bitmap[i] & 1) << (7 - j));
                        i += 3;
                    }

                    sb.Append($"0b{Convert.ToString(b, 2).PadLeft(8, '0')}");
                    if (y != h - 1 || x + 8 < w)
                    {
                        sb.Append(",");
                    }

                    s++;
                }
                sb.Append("\r\n");
            }

            sb.Append("};\r\n");
            sb.Append($"// {s} bytes");
            return sb.ToString();
        }
    }
}
