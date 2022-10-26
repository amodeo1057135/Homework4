namespace Homework4
{
    public partial class Form1 : Form
    {
        public int trials = 100;
        public int paths = 1000;
        public double successProb = 0.5;

        public Bitmap bmap;
        //public Bitmap bmap2;
        public Graphics grap;
        //public Graphics grap2;
        public Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = successProb.ToString();
            textBox2.Text = trials.ToString();
            textBox3.Text = paths.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void parseValues()
        {
            if (textBox1.Text.Length > 0)
            {
                var sR = Double.Parse(textBox1.Text);
                if (sR >= 0 && sR <= 1)
                {
                    successProb = sR;
                }
                else
                {
                    successProb = 0.5;
                    textBox1.Text = successProb.ToString();
                }
            }
            else
            {
                successProb = 0.5;
                textBox1.Text = successProb.ToString();

            }

            if (textBox2.Text.Length > 0)
            {
                var tR = Int32.Parse(textBox2.Text);
                if (tR >= 0)
                {
                    trials = tR;
                }
                else
                {
                    trials = 1000;
                    textBox2.Text = trials.ToString();

                }
            }
            else
            {
                trials = 1000; ;
                textBox2.Text = trials.ToString();
            }

            if (textBox3.Text.Length > 0)
            {
                var pT = Int32.Parse(textBox3.Text);
                if (pT >= 0)
                {
                    paths = pT;
                }
                else
                {
                    paths = 100;
                    textBox3.Text = paths.ToString();

                }
            }
            else
            {
                paths = 100;
                textBox3.Text = paths.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            grap = Graphics.FromImage(bmap);

            var mypoints = new List<Point>();
            var sumYValues = new List<int>();

            parseValues();

            for (int t = 0; t < trials; t++)
            {
                var yValue = 0;
                for (int x = 0; x < paths; x++)
                {
                    var uniform = rand.NextDouble();
                    if (uniform < successProb)
                    {
                        yValue++;
                        mypoints.Add(new Point(virtualX(x, 0, trials, pictureBox1.Width),
                            virtualY(yValue, 0, trials, pictureBox1.Height)));
                    }
                }
                sumYValues.Add(yValue);
            }
            //drawing coordinates
            var pointsArray = mypoints.ToArray();
            Rectangle graphBackground = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
            grap.DrawRectangle(Pens.Black, graphBackground);
            grap.FillRectangle(Brushes.DarkGray, graphBackground);
            grap.DrawLine(Pens.Black, new Point(0, pictureBox1.Height), pointsArray[0]);
            for (int i = 0; i< pointsArray.Length-1; i++ ) {
                grap.DrawLine(Pens.BlueViolet, pointsArray[i], pointsArray[i + 1]);
            }

            //drawing histogram
            var sumArray = sumYValues.ToArray();
            for (int j = 0; j < sumArray.Length; j++)
            {
                var xCord = virtualX(j, 0, trials, pictureBox1.Width);
                //Absolute Frequency
                Rectangle afBar = new Rectangle(xCord, 0, 5, sumArray[j]);
                grap.DrawRectangle(Pens.Yellow, afBar);
                //Relative Frequency
                var relF = (double)sumArray[j]/sumArray.Length;
                Rectangle rBar = new Rectangle(xCord, 0, 5, virtualY((int)relF,0, trials, pictureBox1.Height/2));
                grap.DrawRectangle(Pens.Orange, rBar);
                //Normalized Frequency
                var normValue = (double)sumArray[j]/Math.Sqrt(((double)sumArray.Length));
                Rectangle nfBar = new Rectangle(xCord, 0, 5, virtualY((int)normValue,0, trials, pictureBox1.Height));
                grap.DrawRectangle(Pens.Blue, rBar);

            }

            pictureBox1.Image = bmap;

            //pictureBox2.Image = bmap2; 

        }

        private int virtualX(int x, int minX, int maxX, double W)
        {
            return (int)(W * (double)(x - minX) / (maxX - minX));
        }

        private int virtualY(int y, int minY, int maxY, double H)
        {
            return (int)(H - H * (double)(y - minY) / (maxY - minY));
        }

    }
}