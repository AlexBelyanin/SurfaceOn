using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SurfaceOn
{
    //public struct QuadricEquasion
    //{
    //    private double[] quadric { get; }
    //    private double x, y, z, X, Y, Z;
    //    public double this[int i] { get { return quadric[i]; } }
    //    public double maxX { get { return X; } }
    //    public double maxY { get { return Y; } }
    //    public double maxZ { get { return Z; } }
    //    public double minX { get { return x; } }
    //    public double minY { get { return y; } }
    //    public double minZ { get { return z; } }

    //    public QuadricEquasion(double a, double b, double c, double d, double e, double f, double g, double x, double y, double z, double X, double Y, double Z)
    //    {
    //        quadric = new double[] { a, b, c, d, e, f, g };
    //        this.x = x;
    //        this.y = y;
    //        this.z = z;
    //        this.X = X;
    //        this.Y = Y;
    //        this.Z = Z;
    //    }
    //    public double GetValue(double[] coordinates)
    //    {
    //        return quadric[0] * coordinates[0] * coordinates[0] + quadric[1] * coordinates[1] * coordinates[1] + quadric[2] * coordinates[2] * coordinates[2]
    //            + quadric[3] * coordinates[0] + quadric[4] * coordinates[1] + quadric[5] * coordinates[2] + quadric[6];
    //    }

    //}

    public class Surface
    {
        //QuadricEquasion Equasion;
        Function function;

        public Surface(Function function)
        {
            this.function = function;
        }

        public class MeshPoint
        {
            public double[] Coordinates;
            public double[] Normal, TandentVector1, TandentVector2;
            private double angle;
            public double Angle { get { return angle; } }
            public bool NeedToChangeAngle;
            private int id;
            public int Id { get { return id; } }
            public bool DistanceCheck;

            private class Gradient
            {
                public static double[] GetValue(Function f,double[] coordinates)
                {
                    return new double[] { f.DerivativeX().Calc(coordinates[0], coordinates[1], coordinates[2]), f.DerivativeY().Calc(coordinates[0], coordinates[1], coordinates[2]), f.DerivativeZ().Calc(coordinates[0], coordinates[1], coordinates[2]) };
                }

                public static double GetLengthSquared(Function f, double[] coordinates)
                {
                    return Math.Pow(f.DerivativeX().Calc(coordinates[0], coordinates[1], coordinates[2]), 2) + Math.Pow(f.DerivativeY().Calc(coordinates[0], coordinates[1], coordinates[2]), 2) + Math.Pow(f.DerivativeZ().Calc(coordinates[0], coordinates[1], coordinates[2]), 2);
                }
            }

            public static double Distance(MeshPoint a, MeshPoint b)
            {
                return Math.Sqrt(Math.Pow(a.Coordinates[0] - b.Coordinates[0], 2) + Math.Pow(a.Coordinates[1] - b.Coordinates[1], 2) + Math.Pow(a.Coordinates[2] - b.Coordinates[2], 2));
            }

            public static double Distance(Vertex a, Vertex b)
            {
                return Math.Sqrt(Math.Pow(a.Position[0] - b.Position[0], 2) + Math.Pow(a.Position[1] - b.Position[1], 2) + Math.Pow(a.Position[2] - b.Position[2], 2));
            }

            private static double[] Newton(Function function, double[] coordinates)
            {
                double[] u = new double[] { coordinates[0], coordinates[1], coordinates[2] };
                double[] v, c;
                double m, eps = 0.0001;
                for (int i = 0; i < 10000; i++)
                {
                    v = u;
                    u = new double[3];
                    m = function.Calc(v[0], v[1], v[2]) / Gradient.GetLengthSquared(function, v);
                    c = Gradient.GetValue(function, v);
                    u[0] = v[0] - m * c[0];
                    u[1] = v[1] - m * c[1];
                    u[2] = v[2] - m * c[2];
                    if (Math.Pow(u[0] - v[0], 2) + Math.Pow(u[1] - v[1], 2) + Math.Pow(u[2] - v[2], 2) < eps) return u;
                }
                return u;
            }

            public void ChangeAngle(MeshPoint a, MeshPoint b)
            {
                //string s = "";
                //s += a.Coordinates[0] + " " + a.Coordinates[1] + " " + a.Coordinates[2] + "\n" + b.Coordinates[0] + " " + b.Coordinates[1] + " " + b.Coordinates[2] + "\n";
                double coor1, coor2, coor3, coor4, coor5, coor6, w1, w2;
                coor1 = Normal[0] * (a.Coordinates[0] - Coordinates[0]) + Normal[1] * (a.Coordinates[1] - Coordinates[1]) + Normal[2] * (a.Coordinates[2] - Coordinates[2]);
                coor2 = TandentVector1[0] * (a.Coordinates[0] - Coordinates[0]) + TandentVector1[1] * (a.Coordinates[1] - Coordinates[1]) + TandentVector1[2] * (a.Coordinates[2] - Coordinates[2]);
                coor3 = TandentVector2[0] * (a.Coordinates[0] - Coordinates[0]) + TandentVector2[1] * (a.Coordinates[1] - Coordinates[1]) + TandentVector2[2] * (a.Coordinates[2] - Coordinates[2]);
                w1 = Math.Atan2(coor3, coor2);
                //s += coor1 + " " + coor2 + " " + coor3 + " " + "\n";
                coor4 = Normal[0] * (b.Coordinates[0] - Coordinates[0]) + Normal[1] * (b.Coordinates[1] - Coordinates[1]) + Normal[2] * (b.Coordinates[2] - Coordinates[2]);
                coor5 = TandentVector1[0] * (b.Coordinates[0] - Coordinates[0]) + TandentVector1[1] * (b.Coordinates[1] - Coordinates[1]) + TandentVector1[2] * (b.Coordinates[2] - Coordinates[2]);
                coor6 = TandentVector2[0] * (b.Coordinates[0] - Coordinates[0]) + TandentVector2[1] * (b.Coordinates[1] - Coordinates[1]) + TandentVector2[2] * (b.Coordinates[2] - Coordinates[2]);
                w2 = Math.Atan2(coor6, coor5);
                if (w2 >= w1) this.angle = w2 - w1;
                else this.angle = w2 - w1 + 2 * Math.PI;
                //w1 = Math.Acos((coor1 * coor4 + coor2 * coor5 + coor3 * coor6) / (Math.Sqrt(coor1 * coor1 + coor2 * coor2 + coor3 * coor3) * Math.Sqrt(coor4 * coor4 + coor5 * coor5 + coor6 * coor6)));
                //s += coor4 + " " + coor5 + " " + coor6 + "\n";
                //w2 = (coor2 * coor6 - coor3 * coor5);
                //if (w2 > 0) Angle = w1;
                //else Angle = 2 * Math.PI - w1;

                NeedToChangeAngle = false;
                //MessageBox.Show("\n"+ w1 + " " + w2 + "\n" + this.Angle.ToString());
            }


            public static bool BadAngleCheck(List<MeshPoint> firstPoligon, int firstPoint, List<MeshPoint> secondPoligon, int secondPoint)
            {
                double coor2, coor3, coor5, coor6, w1, w2, w3;
                MeshPoint middle = firstPoligon[firstPoint], left, right, near = secondPoligon[secondPoint];
                if (firstPoint > 0)
                {
                    if (firstPoint < firstPoligon.Count - 1)
                    {
                        left = firstPoligon[firstPoint - 1];
                        right = firstPoligon[firstPoint + 1];
                    }
                    else
                    {
                        left = firstPoligon[firstPoint - 1];
                        right = firstPoligon[0];
                    }
                }
                else
                {
                    if (firstPoint < firstPoligon.Count - 1)
                    {
                        left = firstPoligon[firstPoligon.Count - 1];
                        right = firstPoligon[firstPoint + 1];
                    }
                    else
                    {
                        left = firstPoligon[firstPoligon.Count - 1];
                        right = firstPoligon[0];
                    }
                }
                coor2 = middle.TandentVector1[0] * (left.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (left.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (left.Coordinates[2] - middle.Coordinates[2]);
                coor3 = middle.TandentVector2[0] * (left.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (left.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (left.Coordinates[2] - middle.Coordinates[2]);
                w1 = Math.Atan2(coor3, coor2);
                coor5 = middle.TandentVector1[0] * (right.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (right.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (right.Coordinates[2] - middle.Coordinates[2]);
                coor6 = middle.TandentVector2[0] * (right.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (right.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (right.Coordinates[2] - middle.Coordinates[2]);
                w2 = Math.Atan2(coor6, coor5);
                coor5 = middle.TandentVector1[0] * (near.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (near.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (near.Coordinates[2] - middle.Coordinates[2]);
                coor6 = middle.TandentVector2[0] * (near.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (near.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (near.Coordinates[2] - middle.Coordinates[2]);
                w3 = Math.Atan2(coor6, coor5);
                if (w2 >= w1) w2 = w2 - w1;
                else w2 = w2 - w1 + 2 * Math.PI;
                if (w3 >= w1) w3 = w3 - w1;
                else w3 = w3 - w1 + 2 * Math.PI;
                if (w2 < w3) return true;
                else
                {
                    middle = secondPoligon[secondPoint];
                    near = firstPoligon[firstPoint];
                    if (secondPoint > 0)
                    {
                        if (secondPoint < secondPoligon.Count - 1)
                        {
                            left = secondPoligon[secondPoint - 1];
                            right = secondPoligon[secondPoint + 1];
                        }
                        else
                        {
                            left = secondPoligon[secondPoint - 1];
                            right = secondPoligon[0];
                        }
                    }
                    else
                    {
                        if (secondPoint < secondPoligon.Count - 1)
                        {
                            left = secondPoligon[secondPoligon.Count - 1];
                            right = secondPoligon[secondPoint + 1];
                        }
                        else
                        {
                            left = secondPoligon[secondPoligon.Count - 1];
                            right = secondPoligon[0];
                        }
                    }
                    coor2 = middle.TandentVector1[0] * (left.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (left.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (left.Coordinates[2] - middle.Coordinates[2]);
                    coor3 = middle.TandentVector2[0] * (left.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (left.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (left.Coordinates[2] - middle.Coordinates[2]);
                    w1 = Math.Atan2(coor3, coor2);
                    coor5 = middle.TandentVector1[0] * (right.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (right.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (right.Coordinates[2] - middle.Coordinates[2]);
                    coor6 = middle.TandentVector2[0] * (right.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (right.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (right.Coordinates[2] - middle.Coordinates[2]);
                    w2 = Math.Atan2(coor6, coor5);
                    coor5 = middle.TandentVector1[0] * (near.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector1[1] * (near.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector1[2] * (near.Coordinates[2] - middle.Coordinates[2]);
                    coor6 = middle.TandentVector2[0] * (near.Coordinates[0] - middle.Coordinates[0]) + middle.TandentVector2[1] * (near.Coordinates[1] - middle.Coordinates[1]) + middle.TandentVector2[2] * (near.Coordinates[2] - middle.Coordinates[2]);
                    w3 = Math.Atan2(coor6, coor5);
                    if (w2 >= w1) w2 = w2 - w1;
                    else w2 = w2 - w1 + 2 * Math.PI;
                    if (w3 >= w1) w3 = w3 - w1;
                    else w3 = w3 - w1 + 2 * Math.PI;
                    if (w2 < w3) return true;
                    else return false;
                }
            }

            public MeshPoint(MeshPoint point)
            {
                Coordinates = new double[] { point.Coordinates[0], point.Coordinates[1], point.Coordinates[2] };
                Normal = new double[] { point.Normal[0], point.Normal[1], point.Normal[2] };
                TandentVector1 = new double[] { point.TandentVector1[0], point.TandentVector1[1], point.TandentVector1[2] };
                TandentVector2 = new double[] { point.TandentVector2[0], point.TandentVector2[1], point.TandentVector2[2] };
                angle = -1;
                NeedToChangeAngle = true;
                id = point.id;
                DistanceCheck = false;
                point.DistanceCheck = false;
            }

            public MeshPoint(double[] coordinates, Surface surface, double delta, int id)
            {
                Coordinates = Newton(surface.function, coordinates);
                Normal = Gradient.GetValue(surface.function, Coordinates);
                double temp = Math.Sqrt(Gradient.GetLengthSquared(surface.function, Coordinates));
                Normal[0] /= temp;
                Normal[1] /= temp;
                Normal[2] /= temp;
                TandentVector1 = new double[3];
                if (Normal[0] > 0.5 || Normal[1] > 0.5)
                {
                    temp = Math.Sqrt(Math.Pow(Normal[0], 2) + Math.Pow(Normal[1], 2));
                    TandentVector1[0] = Normal[1] / temp;
                    TandentVector1[1] = -Normal[0] / temp;
                }
                else
                {
                    temp = Math.Sqrt(Math.Pow(Normal[0], 2) + Math.Pow(Normal[2], 2));
                    TandentVector1[0] = -Normal[2] / temp;
                    TandentVector1[2] = Normal[0] / temp;
                }
                TandentVector2 = new double[3];
                TandentVector2[0] = Normal[1] * TandentVector1[2] - Normal[2] * TandentVector1[1];
                TandentVector2[1] = -Normal[0] * TandentVector1[2] + Normal[2] * TandentVector1[0];
                TandentVector2[2] = Normal[0] * TandentVector1[1] - Normal[1] * TandentVector1[0];
                angle = -1;
                NeedToChangeAngle = true;
                this.id = id;
                DistanceCheck = true;
            }
        }

        private void S3(List<MeshPoint> mesh, List<MeshPoint> frontPoligon, List<Triangle> triangles, double delta, int point = -1)
        {
            if (point == -1)
            {
                point = 0;
                for (int i = 0; i < frontPoligon.Count; i++)
                {
                    if (frontPoligon[i].Angle < frontPoligon[point].Angle) point = i;
                }
            }
            double w = frontPoligon[point].Angle;
            int nt = (int)Math.Truncate(3 * w / Math.PI) + 1;
            double deltaW = w / nt;
            MeshPoint v1, v2, p = frontPoligon[point];
            //MessageBox.Show(point + "\n" + frontPoligon[point].Coordinates[0] + " " + frontPoligon[point].Coordinates[1] + " " + frontPoligon[point].Coordinates[2] + "\n" + w + " "+ nt);

            //2
            if (point > 0)
            {
                if (point < frontPoligon.Count - 1)
                {
                    v1 = frontPoligon[point - 1];
                    v2 = frontPoligon[point + 1];
                }
                else
                {
                    v1 = frontPoligon[point - 1];
                    v2 = frontPoligon[0];
                }
            }
            else
            {
                if (point < frontPoligon.Count - 1)
                {
                    v1 = frontPoligon[frontPoligon.Count - 1];
                    v2 = frontPoligon[point + 1];
                }
                else
                {
                    v1 = frontPoligon[frontPoligon.Count - 1];
                    v2 = frontPoligon[0];
                }
            }
            if (deltaW < 0.8 && nt > 1)
            {
                nt--;
                deltaW = w / nt;
            }
            else if (nt == 1 && deltaW > 0.8 && MeshPoint.Distance(v1, v2) > 1.25 * delta)
            {
                nt = 2;
                deltaW = w / nt;
            }
            else if (w < 3 && (MeshPoint.Distance(v1, p) <= 0.5 * delta || MeshPoint.Distance(v2, p) <= 0.5 * delta))
            {
                nt = 1;
                deltaW = w;
            }

            //3
            if (nt == 1)
            {
                triangles.Add(new Triangle(new Vertex(v1.Coordinates, v1.Id), new Vertex(p.Coordinates, p.Id), new Vertex(v2.Coordinates, v2.Id)));
                v1.NeedToChangeAngle = true;
                v2.NeedToChangeAngle = true;
                frontPoligon.RemoveAt(point);
                return;
            }
            double coor1, coor2, coor3, coor4, coor5, coor6, z, y;
            double[] coordinates = new double[3];
            coor1 = p.Normal[0] * (v1.Coordinates[0] - p.Coordinates[0]) + p.Normal[1] * (v1.Coordinates[1] - p.Coordinates[1]) + p.Normal[2] * (v1.Coordinates[2] - p.Coordinates[2]);
            coor2 = p.TandentVector1[0] * (v1.Coordinates[0] - p.Coordinates[0]) + p.TandentVector1[1] * (v1.Coordinates[1] - p.Coordinates[1]) + p.TandentVector1[2] * (v1.Coordinates[2] - p.Coordinates[2]);
            coor3 = p.TandentVector2[0] * (v1.Coordinates[0] - p.Coordinates[0]) + p.TandentVector2[1] * (v1.Coordinates[1] - p.Coordinates[1]) + p.TandentVector2[2] * (v1.Coordinates[2] - p.Coordinates[2]);
            //coor4 = p.Normal[0] * (v2.Coordinates[0] - p.Coordinates[0]) + p.Normal[1] * (v2.Coordinates[1] - p.Coordinates[1]) + p.Normal[2] * (v2.Coordinates[2] - p.Coordinates[2]);
            //coor5 = p.TandentVector1[0] * (v2.Coordinates[0] - p.Coordinates[0]) + p.TandentVector1[1] * (v2.Coordinates[1] - p.Coordinates[1]) + p.TandentVector1[2] * (v2.Coordinates[2] - p.Coordinates[2]);
            //coor6 = p.TandentVector2[0] * (v2.Coordinates[0] - p.Coordinates[0]) + p.TandentVector2[1] * (v2.Coordinates[1] - p.Coordinates[1]) + p.TandentVector2[2] * (v2.Coordinates[2] - p.Coordinates[2]);

            double det, m1, m2, m3, m4, m5, m6, m7, m8, m9;
            det = p.Normal[0] * (p.TandentVector1[1] * p.TandentVector2[2] - p.TandentVector1[2] * p.TandentVector2[1]) +
                p.Normal[1] * (p.TandentVector1[2] * p.TandentVector2[0] - p.TandentVector1[0] * p.TandentVector2[2]) +
                p.Normal[2] * (p.TandentVector1[0] * p.TandentVector2[1] - p.TandentVector1[1] * p.TandentVector2[0]);
            m1 = (p.TandentVector1[1] * p.TandentVector2[2] - p.TandentVector1[2] * p.TandentVector2[1]) / det;
            m4 = (p.TandentVector1[2] * p.TandentVector2[0] - p.TandentVector1[0] * p.TandentVector2[2]) / det;
            m7 = (p.TandentVector1[0] * p.TandentVector2[1] - p.TandentVector1[1] * p.TandentVector2[0]) / det;
            m2 = (p.Normal[2] * p.TandentVector2[1] - p.Normal[1] * p.TandentVector2[2]) / det;
            m5 = (p.Normal[0] * p.TandentVector2[2] - p.Normal[2] * p.TandentVector2[0]) / det;
            m8 = (p.Normal[1] * p.TandentVector2[0] - p.Normal[0] * p.TandentVector2[1]) / det;
            m3 = (p.Normal[1] * p.TandentVector1[2] - p.Normal[2] * p.TandentVector1[1]) / det;
            m6 = (p.Normal[2] * p.TandentVector1[0] - p.Normal[0] * p.TandentVector1[2]) / det;
            m9 = (p.Normal[0] * p.TandentVector1[1] - p.Normal[1] * p.TandentVector1[0]) / det;
            string s = v1.Coordinates[0] + " " + v1.Coordinates[1] + " " + v1.Coordinates[2] + "\n";
            s += (m1 * coor1 + m2 * coor2 + m3 * coor3 + p.Coordinates[0]) + " " + (m4 * coor1 + m5 * coor2 + m6 * coor3 + p.Coordinates[1]) + " " + (m7 * coor1 + m8 * coor2 + m9 * coor3 + p.Coordinates[2]) + "\n";
            s += "n " + p.Normal[0] + " " + p.Normal[1] + " " + p.Normal[2] + "\n" +
                "t1 " + p.TandentVector1[0] + " " + p.TandentVector1[1] + " " + p.TandentVector1[2] + "\n" +
                "t2 " + p.TandentVector2[0] + " " + p.TandentVector2[1] + " " + p.TandentVector2[2] + "\n" +
                det + "\n" + m1 + " " + m2 + " " + m3 + "\n" + m4 + " " + m5 + " " + m6 + "\n" + m7 + " " + m8 + " " + m9 +
                "\n" + coor1 + " " + coor2 + " " + coor3;

            //MessageBox.Show(s);
            s = coor1 + " " + coor2 + " " + coor3 + "\n";
            List<double[]> kek = new List<double[]>();
            for (int i = 1; i < nt; i++)
            {
                y = coor2 * Math.Cos(deltaW * i) - coor3 * Math.Sin(deltaW * i);
                z = coor2 * Math.Sin(deltaW * i) + coor3 * Math.Cos(deltaW * i);
                s += "0 " + y + " " + z + "\n";
                double tmp = Math.Sqrt(y * y + z * z);
                coordinates[0] = 0 * m1 + y * m2 * delta / tmp + z * m3 * delta / tmp + p.Coordinates[0];
                coordinates[1] = 0 * m4 + y * m5 * delta / tmp + z * m6 * delta / tmp + p.Coordinates[1];
                coordinates[2] = 0 * m7 + y * m8 * delta / tmp + z * m9 * delta / tmp + p.Coordinates[2];
                //kek.Add(new double[] { coordinates[0], coordinates[1], coordinates[2] });
                //string s = coordinates[0] + " " + coordinates[1] + " " + coordinates[2] + "\n";
                MeshPoint meshPoint = new MeshPoint(coordinates, this, delta, mesh.Count);
                //s+= meshPoint.Coordinates[0] + " " + meshPoint.Coordinates[1] + " " + meshPoint.Coordinates[2] + "\n";
                //MessageBox.Show(s);
                frontPoligon.Insert(point + i, meshPoint);
                mesh.Add(meshPoint);
            }
            //MessageBox.Show(s);
            //MessageBox.Show(v1.Coordinates[0] + " " + v1.Coordinates[1] + " " + v1.Coordinates[2] + "\n"
            //    + p.Coordinates[0] + " " + p.Coordinates[1] + " " + p.Coordinates[2] + "\n" +
            //    v2.Coordinates[0] + " " + v2.Coordinates[1] + " " + v2.Coordinates[2] + "\n");
            //triangles.Add(new Triangle(new Vertex(v1.Coordinates), new Vertex(p.Coordinates), new Vertex(kek[0])));
            //triangles.Add(new Triangle(new Vertex(v2.Coordinates), new Vertex(p.Coordinates), new Vertex(kek[kek.Count - 1])));
            triangles.Add(new Triangle(new Vertex(v1.Coordinates, v1.Id), new Vertex(p.Coordinates, p.Id), new Vertex(frontPoligon[point + 1].Coordinates, frontPoligon[point + 1].Id)));
            triangles.Add(new Triangle(new Vertex(v2.Coordinates, v2.Id), new Vertex(p.Coordinates, p.Id), new Vertex(frontPoligon[point + nt - 1].Coordinates, frontPoligon[point + nt - 1].Id)));
            for (int i = 1; i < nt - 1; i++)
            {
                //triangles.Add(new Triangle(new Vertex(kek[i - 1]), new Vertex(p.Coordinates), new Vertex(kek[i])));

                triangles.Add(new Triangle(new Vertex(frontPoligon[point + i].Coordinates, frontPoligon[point + i].Id), new Vertex(p.Coordinates, p.Id), new Vertex(frontPoligon[point + i + 1].Coordinates, frontPoligon[point + i + 1].Id)));
            }
            v1.NeedToChangeAngle = true;
            v2.NeedToChangeAngle = true;
            frontPoligon.RemoveAt(point);
        }

        private void CreateGraph(List<Triangle> triangles, out int[,] graph, out double[,] probability)
        {
            graph = new int[triangles.Count, 3];
            probability = new double[triangles.Count, 3];
            double a, b, c;
            for (int i = 0; i < triangles.Count; i++)
            {
                a = MeshPoint.Distance(triangles[i].Vertices[0], triangles[i].Vertices[1]);
                b = MeshPoint.Distance(triangles[i].Vertices[1], triangles[i].Vertices[2]);
                c = MeshPoint.Distance(triangles[i].Vertices[2], triangles[i].Vertices[0]);
                for (int j = 0; j < triangles.Count; j++)
                {
                    if (i != j)
                    {
                        if((triangles[i].Vertices[0].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[0].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[0].Id == triangles[j].Vertices[2].Id)
                        && (triangles[i].Vertices[1].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[1].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[1].Id == triangles[j].Vertices[2].Id))
                        {
                            graph[i, 0] = j;
                            probability[i, 0] = (1 / a) / (1 / a + 1 / b + 1 / c);
                        }
                        if ((triangles[i].Vertices[2].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[2].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[2].Id == triangles[j].Vertices[2].Id)
                        && (triangles[i].Vertices[1].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[1].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[1].Id == triangles[j].Vertices[2].Id))
                        {
                            graph[i, 1] = j;
                            probability[i, 1] = (1 / b) / (1 / a + 1 / b + 1 / c);
                        }
                        if ((triangles[i].Vertices[0].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[0].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[0].Id == triangles[j].Vertices[2].Id)
                        && (triangles[i].Vertices[2].Id == triangles[j].Vertices[0].Id || triangles[i].Vertices[2].Id == triangles[j].Vertices[1].Id || triangles[i].Vertices[2].Id == triangles[j].Vertices[2].Id))
                        {
                            graph[i, 2] = j;
                            probability[i, 2] = (1 / c) / (1 / a + 1 / b + 1 / c);
                        }
                    }
                }
            }
        }

        public List<Triangle> CreateMesh(out int[,] graph, out double[,] probability,int triNum,double[] StartingPoint = null, double delta = 1)
        {
            List<Triangle> triangles = new List<Triangle>();
            List<MeshPoint> mesh = new List<MeshPoint>();
            List<List<MeshPoint>> frontPoligons = new List<List<MeshPoint>>();
            frontPoligons.Add(new List<MeshPoint>());
            List<MeshPoint> frontPoligon = frontPoligons[0];
            double[] tmpCoor;
            bool wasS2;
            double tmp;
            int tmpI;
            //int[,] graph;
            //double[,] probability;
            /*triangles.Add(new Triangle(new Vertex(0, 0, 10), new Vertex(10, 0, 0), new Vertex(0, 1, 0)));
            triangles.Add(new Triangle(new Vertex(0, 0, -10), new Vertex(10, 0, 0), new Vertex(0, 1, 0)));
            triangles.Add(new Triangle(new Vertex(0, 0, 10), new Vertex(-10, 0, 0), new Vertex(0, 1, 0)));
            triangles.Add(new Triangle(new Vertex(0, 0, -10), new Vertex(-10, 0, 0), new Vertex(0, 1, 0)));
            triangles.Add(new Triangle(new Vertex(0, 0, 10), new Vertex(0, 0, -10), new Vertex(10, 0, 0)));
            triangles.Add(new Triangle(new Vertex(0, 0, 10), new Vertex(0, 0, -10), new Vertex(-10, 0, 0)));*/


            //s0
            if (StartingPoint == null)
            {
                StartingPoint = new double[] { 0, 0, 0 };
            }
            mesh.Add(new MeshPoint(StartingPoint, this, delta, mesh.Count));

            string s = mesh[0].Coordinates[0].ToString() + " " + mesh[0].Coordinates[1].ToString() + " " + mesh[0].Coordinates[2].ToString() + "\n";

            for (int i = 0; i < 6; i++)
            {
                double tmp1 = delta * Math.Cos(Math.PI * i / 3), tmp2 = delta * Math.Sin(Math.PI * i / 3);
                tmpCoor = new double[] { mesh[0].Coordinates[0], mesh[0].Coordinates[1], mesh[0].Coordinates[2] };
                tmpCoor[0] += tmp1 * mesh[0].TandentVector1[0] + tmp2 * mesh[0].TandentVector2[0];
                tmpCoor[1] += tmp1 * mesh[0].TandentVector1[1] + tmp2 * mesh[0].TandentVector2[1];
                tmpCoor[2] += tmp1 * mesh[0].TandentVector1[2] + tmp2 * mesh[0].TandentVector2[2];
                MeshPoint point = new MeshPoint(tmpCoor, this, delta, mesh.Count);
                mesh.Add(point);
                frontPoligon.Add(point);

                s += mesh[i + 1].Coordinates[0].ToString() + " " + mesh[i + 1].Coordinates[1].ToString() + " " + mesh[i + 1].Coordinates[2].ToString() + "\n";
            }
            //MessageBox.Show(s);
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[1].Coordinates, mesh[1].Id), new Vertex(mesh[2].Coordinates, mesh[2].Id)));
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[2].Coordinates, mesh[2].Id), new Vertex(mesh[3].Coordinates, mesh[3].Id)));
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[3].Coordinates, mesh[3].Id), new Vertex(mesh[4].Coordinates, mesh[4].Id)));
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[4].Coordinates, mesh[4].Id), new Vertex(mesh[5].Coordinates, mesh[5].Id)));
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[5].Coordinates, mesh[5].Id), new Vertex(mesh[6].Coordinates, mesh[6].Id)));
            triangles.Add(new Triangle(new Vertex(mesh[0].Coordinates, mesh[0].Id), new Vertex(mesh[6].Coordinates, mesh[6].Id), new Vertex(mesh[1].Coordinates, mesh[1].Id)));

            for (int e = 0; e < triNum; e++)
            {
                //s1
                for (int i = 0; i < frontPoligon.Count; i++)
                {
                    //MessageBox.Show("fp " + frontPoligon[i].Coordinates[0] + " " + frontPoligon[i].Coordinates[1] + " " + frontPoligon[i].Coordinates[2]);
                    if (frontPoligon[i].NeedToChangeAngle)
                    {
                        if (i > 0)
                        {
                            if (i < frontPoligon.Count - 1) frontPoligon[i].ChangeAngle(frontPoligon[i - 1], frontPoligon[i + 1]);
                            else frontPoligon[i].ChangeAngle(frontPoligon[i - 1], frontPoligon[0]);
                        }
                        else
                        {
                            if (i < frontPoligon.Count - 1) frontPoligon[i].ChangeAngle(frontPoligon[frontPoligon.Count - 1], frontPoligon[i + 1]);
                            else frontPoligon[i].ChangeAngle(frontPoligon[frontPoligon.Count - 1], frontPoligon[0]);
                        }
                    }
                }

                //s2 small angle check (remark b)
                wasS2 = false;
                for (int i = 0; i < frontPoligon.Count; i++)
                {
                    if (frontPoligon[i].Angle < 1.308996)
                    {
                        wasS2 = true;
                        S3(mesh, frontPoligon, triangles, delta, i);
                        //MessageBox.Show("small angle check");
                        break;
                    }
                }

                //s2
                if (!wasS2)
                    for (int i = 0; i < frontPoligon.Count; i++)
                    {
                        if (i == 0) tmp = 2;
                        else if (i == 1) tmp = 1;
                        else tmp = 0;
                        for (int j = i + 3; j < frontPoligon.Count - tmp; j++)
                        {
                            if (MeshPoint.Distance(frontPoligon[i], frontPoligon[j]) < delta && frontPoligon[i].DistanceCheck && frontPoligon[j].DistanceCheck)
                            {
                                //s = "Wow1 " + e + "\n" +
                                //    frontPoligon[i].Coordinates[0] + " " + frontPoligon[i].Coordinates[1] + " " + frontPoligon[i].Coordinates[2] + "\n" +
                                //    frontPoligon[j].Coordinates[0] + " " + frontPoligon[j].Coordinates[1] + " " + frontPoligon[j].Coordinates[2] + "\n";
                                //s += frontPoligon.Count + " " + i + " " + j + " ";
                                
                                //triangles.Add(new Triangle(new Vertex(frontPoligon[i].Coordinates), new Vertex(frontPoligon[j].Coordinates), new Vertex(new double[] { 0, 0, 0 })));
                                List<MeshPoint> newPoligon = new List<MeshPoint>();
                                newPoligon.Add(new MeshPoint(frontPoligon[i]));
                                for (int k = i + 1; k < j; k++)
                                {
                                    newPoligon.Add(frontPoligon[k]);
                                }
                                newPoligon.Add(new MeshPoint(frontPoligon[j]));
                                frontPoligons.Add(newPoligon);
                                frontPoligon.RemoveRange(i + 1, j - i - 1);

                                //s += frontPoligon.Count;
                                //s += "\n" + frontPoligon[i].Angle + " " + frontPoligon[i].Id;
                                //MessageBox.Show(s);

                                frontPoligon[i].NeedToChangeAngle = true;
                                frontPoligon[i + 1].NeedToChangeAngle = true;
                                i = frontPoligon.Count;
                                wasS2 = true;
                                break;
                            }
                        }
                    }

                if (!wasS2)
                    for (int i = 0; i < frontPoligon.Count; i++)
                    {
                        for (int j = 1; j < frontPoligons.Count; j++)
                        {
                            for (int k = 0; k < frontPoligons[j].Count; k++)
                            {
                                if (MeshPoint.Distance(frontPoligon[i], frontPoligons[j][k]) < delta && frontPoligon[i].DistanceCheck && frontPoligons[j][k].DistanceCheck && !MeshPoint.BadAngleCheck(frontPoligon, i, frontPoligons[j], k))
                                {
                                    //s = "";
                                    //for (int y = 0; y < frontPoligon.Count; y++) s += frontPoligon[y].Id + " ";
                                    //s += "\n";
                                    //for (int y = 0; y < frontPoligons[j].Count; y++) s += frontPoligons[j][y].Id + " ";
                                    //s += "\n";
                                    //s += frontPoligon[i].Id + " " + frontPoligons[j][k].Id + "\n";
                                    //s += MeshPoint.Distance(frontPoligon[i + 1], frontPoligons[j][k + 1]) + " " + frontPoligon[i + 1].Id + " " + frontPoligons[j][k + 1].Id + "\n";
                                    //s += MeshPoint.Distance(frontPoligon[i + 1], frontPoligons[j][k - 1]) + " " + frontPoligon[i + 1].Id + " " + frontPoligons[j][k - 1].Id + "\n";
                                    //s += MeshPoint.Distance(frontPoligon[i - 1], frontPoligons[j][k + 1]) + " " + frontPoligon[i - 1].Id + " " + frontPoligons[j][k + 1].Id + "\n";
                                    //s += MeshPoint.Distance(frontPoligon[i - 1], frontPoligons[j][k - 1]) + " " + frontPoligon[i - 1].Id + " " + frontPoligons[j][k - 1].Id + "\n";
                                    //MessageBox.Show("Wow2 " + e);

                                    double tmpId = frontPoligon[i].Id;
                                    for (int q = 0; q < frontPoligons[j].Count; q++)
                                    {
                                        int m = q + k;
                                        if (m >= frontPoligons[j].Count) m -= frontPoligons[j].Count;
                                        frontPoligon.Insert(i + 1 + q, frontPoligons[j][m]);
                                    }
                                    frontPoligon.Insert(i + 1 + frontPoligons[j].Count, frontPoligon[i + 1]);
                                    frontPoligon.Insert(i + 2 + frontPoligons[j].Count, frontPoligon[i]);

                                    //for (int y = 0; y < frontPoligon.Count; y++) s += frontPoligon[y].Id + " ";
                                    //s += "\n" + frontPoligon[i].Angle + " " + frontPoligon[i + 1].Angle + "\n";

                                    if (i > 0)
                                    {
                                        frontPoligon[i].ChangeAngle(frontPoligon[i - 1], frontPoligon[i + 1]);
                                    }
                                    else
                                    {
                                        frontPoligon[i].ChangeAngle(frontPoligon[frontPoligon.Count - 1], frontPoligon[i + 1]);
                                    }
                                    frontPoligon[i + 1].ChangeAngle(frontPoligon[i], frontPoligon[i + 2]);

                                    //s += frontPoligon[i].Angle + " " + frontPoligon[i + 1].Angle + "\n";


                                    if (frontPoligon[i].Angle < frontPoligon[i + 1].Angle)
                                    {
                                        tmpI = frontPoligon.Count;
                                        S3(mesh, frontPoligon, triangles, delta, i);
                                        tmpI = frontPoligon.Count - tmpI;
                                        frontPoligon[i + tmpI + 1].ChangeAngle(frontPoligon[i + tmpI], frontPoligon[i + tmpI + 2]);
                                        S3(mesh, frontPoligon, triangles, delta, i + tmpI + 1);

                                        //s += "i<i+1";
                                    }
                                    else
                                    {
                                        S3(mesh, frontPoligon, triangles, delta, i + 1);
                                        if (i > 0)
                                        {
                                            frontPoligon[i].ChangeAngle(frontPoligon[i - 1], frontPoligon[i + 1]);
                                        }
                                        else
                                        {
                                            frontPoligon[i].ChangeAngle(frontPoligon[frontPoligon.Count - 1], frontPoligon[i + 1]);
                                        }
                                        S3(mesh, frontPoligon, triangles, delta, i);

                                        //s += "i>=i+1";
                                    }
                                    wasS2 = true;
                                    for (int y = 0; y < frontPoligon.Count; y++)
                                    {
                                        if (frontPoligon[y].Id == tmpId)
                                        {
                                            frontPoligon[y].NeedToChangeAngle = true;
                                            frontPoligon[y - 1].NeedToChangeAngle = true;
                                            //s += "angle ntc " + frontPoligon[y].Id + " " + frontPoligon[y - 1].Id + "\n";
                                            break;
                                        }
                                    }
                                    frontPoligons.RemoveAt(j);
                                    j = frontPoligons.Count;
                                    i = frontPoligon.Count;

                                    //MessageBox.Show(s);
                                    break;
                                }
                            }
                        }
                    }


                //s3
                if (!wasS2)
                    S3(mesh, frontPoligon, triangles, delta);
                //s4
                if (frontPoligon.Count == 3)
                {
                    triangles.Add(new Triangle(new Vertex(frontPoligon[0].Coordinates, frontPoligon[0].Id), new Vertex(frontPoligon[1].Coordinates, frontPoligon[1].Id), new Vertex(frontPoligon[2].Coordinates, frontPoligon[2].Id)));
                    frontPoligons.RemoveAt(0);
                    //MessageBox.Show("end " + e + " " + frontPoligons.Count);
                    if (frontPoligons.Count > 0) frontPoligon = frontPoligons[0];
                    else break;
                }
            }
            CreateGraph(triangles, out graph, out probability);
            //s = "";
            //for(int i = 0; i < graph.GetLength(0); i++)
            //{
            //    s += probability[i, 0] + " " + probability[i, 1] + " " + probability[i, 2] + "\n";
            //}
            //MessageBox.Show(s);
            return triangles;
        }
    }
}
