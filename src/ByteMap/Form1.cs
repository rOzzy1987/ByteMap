using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ByteMap.BLL;

namespace ByteMap
{
    public partial class Form1 : Form
    {
        private Dotter _dotter;
        private ImageProcessor _ip;
        private Threshold _thr;
        private SourceCodeGen _codeGen;
        private HistogramCalculator _histCalc;
        private HistogramRenderer _histRnd;

        private AboutBox1 _about;

        private Image _img;
        public Form1()
        {
            InitializeComponent();
            _ip = new ImageProcessor();
            _dotter = new Dotter(_ip);
            _thr = new Threshold {Value = (byte) trackBar1.Value};
            _codeGen = new SourceCodeGen();
            _about = new AboutBox1();
            _histCalc = new HistogramCalculator();
            _histRnd = new HistogramRenderer(_ip);

        }

        private void Calculate()
        {
            button2.Enabled = false;
            if (_img is Bitmap b)
            {
                var raw = _ip.GetBytes(b);
                var data = _thr.Transform(raw);
                var w = b.Width;
                var h = b.Height;
                pictureBox2.Image = _dotter.Dot(data, w, h);
                var histogram = _histCalc.Calculate(raw);
                pictureBox1.Image = _histRnd.Render(histogram, _thr.Value);

                textBox2.Text = _codeGen.Generate(data, w, textBox1.Text);
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.SuspendLayout();
                try
                {
                    _img = Image.FromFile(openFileDialog1.FileName);

                    Calculate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.ResumeLayout(false);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            _thr.Value = (byte) trackBar1.Value;
            Calculate();
            this.ResumeLayout(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Regex.Replace(textBox1.Text, "[^a-zA-Z0-9-_.]", "");

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try { 
                    File.WriteAllText(saveFileDialog1.FileName, textBox2.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _about.ShowDialog();
        }
    }
}
