//Белянин 141 05.05.2017
//Класс переводящий строку в обратную польскую запись рекурсивным способом при помощи следующих выражений
//И считающее значение в точке
//E::=T[[+| -] T]*
//T::=R[[*|/] R]*
//R::=M[^R]?
//M::=[-][x|N|F(E)|(E)]
//N::=<число вида double с фиксированной запятой>
//F::=[sin|cos|tg|asin|acos|atg|ln|exp]


using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SurfaceOn
{
    class Lxm
    {
        private char ltype;
        public char type { get { return ltype; } }
        public string value { get { return lvalue; } }
        private string pattern_n = "^[0-9]+(,[0-9]+)?";
        private string pattern_f = "^(sin|cos|ln|tg|atg|asin|acos|exp)";
        private string lvalue = null;

        public void makeop(char c)
        {
            ltype = 'o';
            lvalue = c.ToString();
        }

        private void makeUn()
        {
            ltype = 'f';
            lvalue = "_";
        }

        static public Lxm Unminus()
        {
            Lxm l = new Lxm();
            l.makeUn();
            return l;
        }

        static public Lxm makeOp(char c)
        {
            Lxm l = new Lxm();
            l.makeop(c);
            return l;
        }

        public double OpFunk(double a, double b)
        {
            switch (lvalue)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                case "^":
                    return Math.Pow(a, b);
                default:
                    return double.NaN;
            }
        }

        public double FuFunk(double a)
        {
            switch (lvalue)
            {
                case "_":
                    return -a;
                case "sin":
                    return Math.Sin(a);
                case "cos":
                    return Math.Cos(a);
                case "tg":
                    return Math.Tan(a);
                case "ln":
                    return Math.Log(a);
                case "asin":
                    return Math.Asin(a);
                case "acos":
                    return Math.Acos(a);
                case "atg":
                    return Math.Atan(a);
                case "sqrt":
                    return Math.Sqrt(a);
                case "sqr":
                    return a * a;
                case "abs":
                    return Math.Abs(a);
                case "exp":
                    return Math.Exp(a);
                default:
                    return double.NaN;
            }
        }

        public bool getLxm(string s, int iter)
        {
            if (s[iter] >= '0' && s[iter] <= '9')
            {
                ltype = 'n';
                lvalue = Regex.Match(s.Remove(0, iter), pattern_n).ToString();
                return true;
            }
            else if (Regex.IsMatch(s.Remove(0, iter), pattern_f))
            {
                ltype = 'f';
                lvalue = Regex.Match(s.Remove(0, iter), pattern_f).ToString();
                return true;
            }
            else if (s[iter] == 'x')
            {
                ltype = 'x';
                lvalue = s[iter].ToString();
                return true;
            }
            else if (s[iter] == 'y')
            {
                ltype = 'y';
                lvalue = s[iter].ToString();
                return true;
            }
            else if (s[iter] == 'z')
            {
                ltype = 'z';
                lvalue = s[iter].ToString();
                return true;
            }
            return false;
        }
    }

    class CRpn1
    {
        private bool error = false;
        private string instr = null;
        private List<Lxm> lxms;
        private int iter = 0;
        private string errmsg = null;
        public int itErr { get { return iter; } }
        public string msgErr { get { return errmsg; } }

        private void E()
        {
            T();
            while (error != true && (instr[iter] == '+' || instr[iter] == '-'))
            {
                Lxm lxm = new Lxm();
                lxm.makeop(instr[iter]);
                iter++;
                T();
                lxms.Add(lxm);
            }
        }

        private void T()
        {
            R();
            while (error != true && (instr[iter] == '*' || instr[iter] == '/'))
            {
                Lxm lxm = new Lxm();
                lxm.makeop(instr[iter]);
                iter++;
                R();
                lxms.Add(lxm);
            }
        }

        private void R()
        {
            M();
            if (error != true && instr[iter] == '^')
            {
                iter++;
                R();
                lxms.Add(Lxm.makeOp('^'));
            }
        }

        private void M()
        {
            errmsg = "Ожидалась буква, число или функция";
            Lxm lxm = new Lxm();
            bool unminus;
            if (instr[iter] == '-')
            {
                unminus = true;
                iter++;
            }
            else unminus = false;
            if (instr[iter] == '(')
            {
                iter++;
                E();
                if (error == false && instr[iter] == ')') iter++;
                else if (error == false)
                {
                    error = true;
                    errmsg = "Ожидалась закрывающая скобка )";
                }
            }
            else if (lxm.getLxm(instr, iter))
            {
                iter += lxm.value.Length;
                if (lxm.type == 'f')
                {
                    errmsg = "Ожидалась ( перед функцией";
                    if (instr[iter] == '(')
                    {
                        iter++;
                        E();
                        if (error == false && instr[iter] == ')') iter++;
                        else if (error == false)
                        {
                            error = true;
                            errmsg = "Ожидалась закрывающая скобка )";
                        }
                    }
                    else error = true;
                }
                lxms.Add(lxm);
                if (unminus) lxms.Add(Lxm.Unminus());
            }
            else error = true;
        }

        public bool doRpn(string s)
        {
            Regex regex = new Regex("[^a-z0-9\\+\\*-/,\\(\\)\\^]+");
            if (regex.IsMatch(s))
            {
                iter = regex.Match(s).Index;
                errmsg = "Этот символ НЕ может использоваться в выражении";
                error = true;
                return !error;
            }
            iter = 0;
            instr = s;
            errmsg = null;
            instr += (char)0x01;
            lxms = new List<Lxm>();
            error = false;
            E();
            if (error == false && instr[iter] != (char)0x01)
            {
                error = true;
                errmsg = "Ожидался конец строки";
            }
            return !error;
        }

        public double Func(double x, double y, double z)
        {
            Stack<double> N = new Stack<double>();
            double a, b;
            foreach (Lxm lxm in lxms)
            {
                if (lxm.type == 'x') N.Push(x);
                if (lxm.type == 'y') N.Push(y);
                if (lxm.type == 'z') N.Push(z);
                if (lxm.type == 'n') N.Push(double.Parse(lxm.value));
                if (lxm.type == 'o')
                {
                    a = N.Pop();
                    b = N.Pop();
                    N.Push(lxm.OpFunk(b, a));
                }
                if (lxm.type == 'f') N.Push(lxm.FuFunk(N.Pop()));
            }
            return N.Pop();
        }

        public override string ToString()
        {
            string s = "";
            foreach (Lxm lxm in lxms) s += lxm.value + " ";
            return s;
        }

        public Function doFunction()
        {
            Stack<Function> stack = new Stack<Function>();
            Function F1, F2;
            foreach (Lxm lxm in lxms)
            {
                switch (lxm.type)
                {
                    case 'n':
                        stack.Push(new Constant(double.Parse(lxm.value)));
                        break;
                    case 'o':
                        F2 = stack.Pop();
                        F1 = stack.Pop();
                        switch (lxm.value)
                        {
                            case "+":
                                stack.Push(F1 + F2);
                                break;
                            case "-":
                                stack.Push(F1 - F2);
                                break;
                            case "*":
                                stack.Push(F1 * F2);
                                break;
                            case "/":
                                stack.Push(F1 / F2);
                                break;
                            case "^":
                                stack.Push(F1 ^ F2);
                                break;
                        }
                        break;
                    case 'f':
                        F1 = stack.Pop();
                        switch (lxm.value)
                        {
                            case "sin":
                                stack.Push(new Sin(F1));
                                break;
                            case "cos":
                                stack.Push(new Cos(F1));
                                break;
                            case "ln":
                                stack.Push(new Ln(F1));
                                break;
                            case "_":
                                stack.Push(-F1);
                                break;
                            case "exp":
                                stack.Push(new Exp(F1));
                                break;
                            case "atg":
                                stack.Push(new ATg(F1));
                                break;
                            case "asin":
                                stack.Push(new ASin(F1));
                                break;
                            case "acos":
                                stack.Push(new ACos(F1));
                                break;
                            case "tg":
                                stack.Push(new Tg(F1));
                                break;
                        }
                        break;
                    case 'x':
                        stack.Push(new X());
                        break;
                    case 'y':
                        stack.Push(new Y());
                        break;
                    case 'z':
                        stack.Push(new Z());
                        break;
                }
            }
            return stack.Pop();
        }

    }
}
