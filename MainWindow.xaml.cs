using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurfaceOn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var trackball = new Wpf3DTools.Trackball();
            trackball.EventSource = background;
            viewport.Camera.Transform = trackball.Transform;
            light.Transform = trackball.RotateTransform;
        }

        //private BackgroundWorker backgroundWorker;
        Function f;
        double[] startingPoint;
        Triangulation triangulation;
        int[,] graph;
        double[,] probability;
        double sideLength;

        List<Color> colors;
        int triNum = 0;

        void Create()
        {
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            colors = new List<Color>();
            for (int i = 0; i < 5000; i++)
            {
                colors.Add(Color.FromArgb((byte)255, (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
            }
        }

        private void OptButton_Click(object sender, RoutedEventArgs e)
        {
            triNum = 0;

            //double x, y, z;
            //CRpn1 R = new CRpn1();
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += backgroundWorker_DoWork;
            //worker.ProgressChanged += backgroundWorker_ProgressChanged;
            //worker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            //if (!R.doRpn(function.Text))
            //{
            //    MessageBox.Show(R.msgErr);
            //}
            //else
            //{
            //    if (double.TryParse(startingX.Text, out x) && double.TryParse(startingY.Text, out y) && double.TryParse(startingZ.Text, out z))
            //    {
            //        if (double.TryParse(triangleLength.Text, out sideLength))
            //        {
            //            startingPoint = new double[] { x, y, z };
            //            f = R.doFunction();
            //            //MessageBox.Show(f.ToString() + "\n" + f1.ToString());
            //            //backgroundWorker.RunWorkerAsync();
            //            //Stopwatch sw = new Stopwatch();
            //            //sw.Start();
            //            //Create();
            //            //sw.Stop();
            //            //MessageBox.Show(sw.Elapsed.ToString());

            //            worker.RunWorkerAsync();

            //        }
            //    }
            //}
        }

        private void Classic_click(object sender, RoutedEventArgs e)
        {
            //viewport.Children.Add((ModelVisual3D)triangulation);

            int count;
            if (int.TryParse(numPoints.Text, out count))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                List<int> agr = DLA.Pure(graph, probability, count);
                foreach (Triangle t in triangulation.triangles)
                {
                    t.IsAgr = true;
                }
                foreach (int g in agr)
                {
                    triangulation.triangles[g].IsAgr = false;
                }
                triangulation = Triangulation.Update(triangulation.triangles, 10);
                if (triangulation != null) viewport.Children.Remove(triangulation);
                viewport.Children.Add(triangulation);
                sw.Stop();
                MessageBox.Show(sw.Elapsed.ToString());
            }
        }

        private void GenerateUniformButton_Click(object sender, RoutedEventArgs e)
        {
            double x, y, z;
            CRpn1 R = new CRpn1();
            R.doRpn(function.Text);
            if (double.TryParse(startingX.Text, out x) && double.TryParse(startingY.Text, out y) && double.TryParse(startingZ.Text, out z))
            {
                if (double.TryParse(triangleLength.Text, out sideLength))
                {
                    startingPoint = new double[] { x, y, z };
                    f = R.doFunction();
                    if (triangulation != null) viewport.Children.Remove(triangulation);
                    triangulation = Triangulation.Create(f, startingPoint, 10, sideLength, colors,triNum, out graph, out probability);
                    viewport.Children.Add(triangulation);
                    infoText.Text = string.Format("{0} Треугольников", triangulation.Count);
                }
            }

            triNum++;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //if (triangulation != null) viewport.Children.Remove(triangulation);

            //triangulation = Triangulation.Create(f, startingPoint, 10, sideLength, out graph, out probability);
            //viewport.Children.Add(triangulation);

            //infoText.Text = string.Format("{0} triangles", triangulation.Count);

            //e.Result = Triangulation.Create(f, startingPoint, 10, sideLength, colors, out graph, out probability);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // Ошибка была сгенерирована обработчиком события DoWork
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            else
            {
                if (triangulation != null) viewport.Children.Remove(triangulation);
                triangulation = (Triangulation)e.Result;
                MessageBox.Show(triangulation.Count + "");
                //viewport.Children.Add(triangulation);
                infoText.Text = string.Format("{0} triangles", triangulation.Count);
            }
            progressBar.Value = 0;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        
    }
}
