using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SurfaceOn
{
    class Triangulation : ModelVisual3D
    {
        public List<Triangle> triangles;

        public int Count { get { return triangles.Count(); } }

        public static Triangulation Create(Function function, double[] startingPoint, double radius, double sideLength, List<Color> colors, int triNum, out int[,] graph, out double[,] probability)
        {
            Random rnd = new Random();
            Surface surface = new Surface(function);
            
            List<Triangle> triangles = surface.CreateMesh(out graph, out probability, triNum, startingPoint, sideLength);

            // create a model for each tetrahedron, pick a random color
            Model3DGroup model = new Model3DGroup();
            //foreach (var t in triangles)
            //{
            //    var color = Color.FromArgb((byte)255, (byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
            //    model.Children.Add(t.CreateModel(color, radius));
            //}
            
            for(int i = 0; i < triangles.Count; i++)
            {
                model.Children.Add(triangles[i].CreateModel(colors[i], radius));
            }

            var triangulation = new Triangulation();
            triangulation.triangles = triangles;

            // assign the Visual3DModel property of the ModelVisual3D class
            triangulation.Visual3DModel = model;

            return triangulation;
        }

        public static Triangulation Update(List<Triangle> triangles, double radius)
        {
            Random rnd = new Random();
            Model3DGroup model = new Model3DGroup();
            foreach (var t in triangles)
            {
                Color color;
                if(t.IsAgr) color = Color.FromArgb(255, (byte)rnd.Next(230,256), 0, 0);
                else color = Color.FromArgb(255, (byte)rnd.Next(240, 256), (byte)rnd.Next(240, 256), (byte)rnd.Next(240, 256));
                model.Children.Add(t.CreateModel(color, radius));
            }

            var triangulation = new Triangulation();
            triangulation.triangles = triangles;

            // assign the Visual3DModel property of the ModelVisual3D class
            triangulation.Visual3DModel = model;

            return triangulation;
        }
    }
}
