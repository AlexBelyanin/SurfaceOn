using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SurfaceOn
{
    public class Vertex
    {
        public Vertex(double x, double y, double z, int id)
        {
            Position = new double[] { x, y, z };
            Id = id;
        }

        public Vertex(double[] coordinates, int id)
        {
            Position = new double[] { coordinates[0], coordinates[1], coordinates[2] };
            Id = id;
        }

        public Point3D ToPoint3D()
        {
            return new Point3D(Position[0], Position[1], Position[2]);
        }

        public double[] Position { get; set; }
        public int Id { get; }
    }
}
