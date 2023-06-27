//Белянин 11.05.17 141
//Класс для создания произвольных функций
//С возможностью нахождения значения в точке и создания производной, расширения
// X, const, sin, cos, tg, asin, acos, atg, ln, exp, +, -, *, /, ^

using System;

namespace SurfaceOn
{
    public abstract class Function
    {
        public abstract double Calc(double x, double y, double z);
        public abstract Function DerivativeX();
        public abstract Function DerivativeY();
        public abstract Function DerivativeZ();

        public static Function operator +(Function a, Function b)
        {
            return new Addition(a, b);
        }
        public static Function operator +(double a, Function b)
        {
            return new Constant(a) + b;
        }
        public static Function operator +(Function a, double b)
        {
            return a + new Constant(b);
        }
        public static Function operator *(Function a, Function b)
        {
            return new Multiplication(a, b);
        }
        public static Function operator *(double a, Function b)
        {
            return new Constant(a) * b;
        }
        public static Function operator *(Function a, double b)
        {
            return a * new Constant(b);
        }
        public static Function operator -(Function a, Function b)
        {
            return new Difference(a, b);
        }
        public static Function operator -(double a, Function b)
        {
            return new Constant(a) - b;
        }
        public static Function operator -(Function a, double b)
        {
            return a - new Constant(b);
        }
        public static Function operator /(Function a, Function b)
        {
            return new Division(a, b);
        }
        public static Function operator /(double a, Function b)
        {
            return new Constant(a) / b;
        }
        public static Function operator /(Function a, double b)
        {
            return a / new Constant(b);
        }
        public static Function operator ^(Function a, Function b)
        {
            return new Power(a, b);
        }
        public static Function operator ^(double a, Function b)
        {
            return new Constant(a) ^ b;
        }
        public static Function operator ^(Function a, double b)
        {
            return a ^ new Constant(b);
        }
        public static Function operator -(Function a)
        {
            return new Constant(0) - a;
        }
    }

    public class Constant : Function
    {
        private readonly double value;
        public Constant(double x)
        {
            value = x;
        }
        public override double Calc(double x, double y, double z)
        {
            return value;
        }
        public override Function DerivativeX()
        {
            return new Constant(0);
        }
        public override Function DerivativeY()
        {
            return new Constant(0);
        }
        public override Function DerivativeZ()
        {
            return new Constant(0);
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }

    public class X : Function
    {
        public override double Calc(double x, double y, double z)
        {
            return x;
        }
        public override Function DerivativeX()
        {
            return new Constant(1);
        }
        public override Function DerivativeY()
        {
            return new Constant(0);
        }
        public override Function DerivativeZ()
        {
            return new Constant(0);
        }
        public override string ToString()
        {
            return "X";
        }
    }

    public class Y : Function
    {
        public override double Calc(double x, double y, double z)
        {
            return y;
        }
        public override Function DerivativeX()
        {
            return new Constant(0);
        }
        public override Function DerivativeY()
        {
            return new Constant(1);
        }
        public override Function DerivativeZ()
        {
            return new Constant(0);
        }
        public override string ToString()
        {
            return "Y";
        }
    }

    public class Z : Function
    {
        public override double Calc(double x, double y, double z)
        {
            return z;
        }
        public override Function DerivativeX()
        {
            return new Constant(0);
        }
        public override Function DerivativeY()
        {
            return new Constant(0);
        }
        public override Function DerivativeZ()
        {
            return new Constant(1);
        }
        public override string ToString()
        {
            return "Z";
        }
    }

    public class Exp : Function
    {
        protected readonly Function inside;
        public Exp(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Exp(inside.Calc(x,y,z));
        }
        public override Function DerivativeX()
        {
            return new Exp(inside) * inside.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return new Exp(inside) * inside.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return new Exp(inside) * inside.DerivativeZ();
        }
        public override string ToString()
        {
            return "Exp(" + inside.ToString() + ")";
        }
    }

    public class Sin : Function
    {
        protected readonly Function inside;
        public Sin(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Sin(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return new Cos(inside) * inside.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return new Cos(inside) * inside.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return new Cos(inside) * inside.DerivativeZ();
        }
        public override string ToString()
        {
            return "Sin(" + inside.ToString() + ")";
        }
    }

    public class Cos : Function
    {
        protected readonly Function inside;
        public Cos(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Cos(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return -new Sin(inside) * inside.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return -new Sin(inside) * inside.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return -new Sin(inside) * inside.DerivativeZ();
        }
        public override string ToString()
        {
            return "Cos(" + inside.ToString() + ")";
        }
    }

    public class Tg : Function
    {
        protected readonly Function inside;
        public Tg(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Tan(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return inside.DerivativeX() / (new Cos(inside) ^ 2);
        }
        public override Function DerivativeY()
        {
            return inside.DerivativeY() / (new Cos(inside) ^ 2);
        }
        public override Function DerivativeZ()
        {
            return inside.DerivativeZ() / (new Cos(inside) ^ 2);
        }
        public override string ToString()
        {
            return "Tg(" + inside.ToString() + ")";
        }
    }

    public class ASin : Function
    {
        protected readonly Function inside;
        public ASin(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Asin(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return inside.DerivativeX() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override Function DerivativeY()
        {
            return inside.DerivativeY() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override Function DerivativeZ()
        {
            return inside.DerivativeZ() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override string ToString()
        {
            return "ASin(" + inside.ToString() + ")";
        }
    }

    public class ACos : Function
    {
        protected readonly Function inside;
        public ACos(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Acos(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return -inside.DerivativeX() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override Function DerivativeY()
        {
            return -inside.DerivativeY() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override Function DerivativeZ()
        {
            return -inside.DerivativeZ() * ((1 - (inside ^ 2)) ^ (0.5));
        }
        public override string ToString()
        {
            return "ACos(" + inside.ToString() + ")";
        }
    }

    public class ATg : Function
    {
        protected readonly Function inside;
        public ATg(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Tan(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return inside.DerivativeX() * (1 + (inside ^ 2));
        }
        public override Function DerivativeY()
        {
            return inside.DerivativeY() * (1 + (inside ^ 2));
        }
        public override Function DerivativeZ()
        {
            return inside.DerivativeZ() * (1 + (inside ^ 2));
        }
        public override string ToString()
        {
            return "ATg(" + inside.ToString() + ")";
        }
    }

    public class Ln : Function
    {
        protected readonly Function inside;
        public Ln(Function inF)
        {
            inside = inF;
        }
        public override double Calc(double x, double y, double z)
        {
            return Math.Log(inside.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            return inside.DerivativeX() / inside;
        }
        public override Function DerivativeY()
        {
            return inside.DerivativeY() / inside;
        }
        public override Function DerivativeZ()
        {
            return inside.DerivativeZ() / inside;
        }
        public override string ToString()
        {
            return "Ln(" + inside.ToString() + ")";
        }
    }

    public abstract class Operator : Function
    {
        protected readonly Function leftF;
        protected readonly Function rightF;
        protected Operator(Function a, Function b)
        {
            leftF = a;
            rightF = b;
        }
    }

    public class Multiplication : Operator
    {
        public Multiplication(Function a, Function b) : base(a, b) { }
        public override double Calc(double x, double y, double z)
        {
            return leftF.Calc(x, y, z) * rightF.Calc(x, y, z);
        }
        public override Function DerivativeX()
        {
            return leftF.DerivativeX() * rightF + leftF * rightF.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return leftF.DerivativeY() * rightF + leftF * rightF.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return leftF.DerivativeZ() * rightF + leftF * rightF.DerivativeZ();
        }
        public override string ToString()
        {
            return "(" + leftF.ToString() + "*" + rightF.ToString() + ")";
        }
    }

    public class Addition : Operator
    {
        public Addition(Function a, Function b) : base(a, b) { }
        public override double Calc(double x, double y, double z)
        {
            return leftF.Calc(x, y, z) + rightF.Calc(x, y, z);
        }
        public override Function DerivativeX()
        {
            return leftF.DerivativeX() + rightF.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return leftF.DerivativeY() + rightF.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return leftF.DerivativeZ() + rightF.DerivativeZ();
        }
        public override string ToString()
        {
            return "(" + leftF.ToString() + "+" + rightF.ToString() + ")";
        }
    }

    public class Difference : Operator
    {
        public Difference(Function a, Function b) : base(a, b) { }
        public override double Calc(double x, double y, double z)
        {
            return leftF.Calc(x, y, z) - rightF.Calc(x, y, z);
        }
        public override Function DerivativeX()
        {
            return leftF.DerivativeX() - rightF.DerivativeX();
        }
        public override Function DerivativeY()
        {
            return leftF.DerivativeY() - rightF.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            return leftF.DerivativeZ() - rightF.DerivativeZ();
        }
        public override string ToString()
        {
            return "(" + leftF.ToString() + "-" + rightF.ToString() + ")";
        }
    }

    public class Division : Operator
    {
        public Division(Function a, Function b) : base(a, b) { }
        public override double Calc(double x, double y, double z)
        {
            return leftF.Calc(x, y, z) / rightF.Calc(x, y, z);
        }
        public override Function DerivativeX()
        {
            return (leftF.DerivativeX() * rightF - rightF.DerivativeX() * leftF) / rightF / rightF;
        }
        public override Function DerivativeY()
        {
            return (leftF.DerivativeY() * rightF - rightF.DerivativeY() * leftF) / rightF / rightF;
        }
        public override Function DerivativeZ()
        {
            return (leftF.DerivativeZ() * rightF - rightF.DerivativeZ() * leftF) / rightF / rightF;
        }
        public override string ToString()
        {
            return "(" + leftF.ToString() + "/" + rightF.ToString() + ")";
        }
    }

    public class Power : Operator
    {
        public Power(Function a, Function b) : base(a, b) { }
        public override double Calc(double x, double y, double z)
        {
            return Math.Pow(leftF.Calc(x, y, z), rightF.Calc(x, y, z));
        }
        public override Function DerivativeX()
        {
            if (rightF.GetType() == new Constant(0).GetType()) return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeX();
            if (leftF.GetType() == new Constant(0).GetType()) return (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeX();
            return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeX() + (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeX();
        }
        public override Function DerivativeY()
        {
            if (rightF.GetType() == new Constant(0).GetType()) return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeY();
            if (leftF.GetType() == new Constant(0).GetType()) return (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeY();
            return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeY() + (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeY();
        }
        public override Function DerivativeZ()
        {
            if (rightF.GetType() == new Constant(0).GetType()) return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeZ();
            if (leftF.GetType() == new Constant(0).GetType()) return (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeZ();
            return rightF * (leftF ^ (rightF - 1)) * leftF.DerivativeZ() + (leftF ^ (rightF - 1)) * new Ln(leftF) * rightF.DerivativeZ();
        }
        public override string ToString()
        {
            return "(" + leftF.ToString() + "^" + rightF.ToString() + ")";
        }
    }

}
