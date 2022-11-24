using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using ByteMap.BLL;

namespace ByteMap.Cmd
{
    class Program
    {
        static ImageProcessor _ip = new ImageProcessor();
        static Threshold _th = new Threshold();
        static SourceCodeGen _srcGen = new SourceCodeGen();

        static void Main(string[] args)
        {
            if (args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("Usage: bm BITMAP_FILENAME RESULT_NAME [THRESHOLD]");
            }
            else
            {
                try
                {
                    var img = Image.FromFile(args[0]);
                    if (!(img is Bitmap b))
                        throw new BmException("Invalid image format!");

                    var raw = _ip.GetBytes(b);

                    if (args.Length == 3)
                    {
                        if (!Byte.TryParse(args[2], out var v))
                            throw new BmException("Invalid threshold value! Should be between 0 and 255");
                        _th.Value = v;
                    }
                    else
                    {
                        _th.Value = 128;
                    }

                    var mono = _th.Transform(raw);

                    var text = _srcGen.Generate(mono, b.Width, args[1]);

                    var file = Regex.Replace(args[1], "[^a-zA-Z0-9]", "") + ".h";
                    File.WriteAllText(file, text);
                    Console.WriteLine($"Output written to {file}");
                }
                catch (BmException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("File not found!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something happened: ");
                    Console.WriteLine(ex);
                }
            }
        }
    }
    class BmException : Exception
    {
        public BmException(string msg) : base(msg) { }
    }
}
