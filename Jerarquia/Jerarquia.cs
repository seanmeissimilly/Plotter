using System;

/// <summary>
/// Jerarquia de funciones y expresiones booleanas.
/// </summary>
namespace Jerarquia
{
    /* No tengo funciones ternarias porque las funciones ternarias son una composición de funciones binarias */

    /// <summary>
    /// Clase base
    /// </summary>
    public abstract class Funcion
    {
        public abstract double Evaluar(double x);

        public abstract Funcion Derivate();
    }

    #region Herederos de Función

    public abstract class Unaria : Funcion
    {
        private Funcion a;
        private double x;

        protected Unaria(Funcion a)
        {
            this.a = a;
        }

        public override double Evaluar(double x)
        {
            this.x = x;
            return Operacion(a.Evaluar(x));
        }

        protected abstract double Operacion(double x);

        public override string ToString()
        {
            x = double.Parse(a.ToString());
            return Operacion(x).ToString();
        }
    }

    public class Identidad : Funcion
    {
        public Identidad()
        {
        }

        public override double Evaluar(double x)
        {
            return x;
        }

        public override Funcion Derivate()
        {
            return new Constante(1);
        }

        public override string ToString()
        {
            return "x";
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Identidad();
        }
    }

    public class Constante : Funcion
    {
        private double x;
        private static double z;

        public Constante(double x)
        {
            this.x = x;
            z = x;
        }

        public override double Evaluar(double y)
        {
            return x;
        }

        public override Funcion Derivate()
        {
            return new Constante(0);
        }

        public override string ToString()
        {
            return x.ToString();
        }

        public static Constante Create(params Funcion[] A)
        {
            return new Constante(z);
        }
    }

    public abstract class Binaria : Funcion
    {
        private Funcion a, b;
        private double x, y;

        protected Binaria(Funcion a, Funcion b)
        {
            this.a = a;
            this.b = b;
        }

        protected abstract double Operacion(double x, double y);

        public override double Evaluar(double x)
        {
            return Operacion(a.Evaluar(x), b.Evaluar(x));
        }

        public override string ToString()
        {
            x = double.Parse(a.ToString());
            y = double.Parse(b.ToString());
            return Operacion(x, y).ToString();
        }
    }

    #endregion

    #region Herederos de Función Unaria

    public class Sin : Unaria
    {
        private Funcion a;

        public Sin(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Sin(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Cos(a), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Sin(args[0]);
        }
    }

    public class Cos : Unaria
    {
        private Funcion a;

        public Cos(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Cos(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Multliplicacion(new Constante(-1), new Sin(a)), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Cos(args[0]);
        }
    }

    public class Tan : Unaria
    {
        private Funcion a;

        public Tan(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Tan(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Pow(new Sec(a), new Constante(2)), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Tan(args[0]);
        }
    }

    public class Cot : Unaria
    {
        private Funcion a;

        public Cot(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return 1 / Math.Tan(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Multliplicacion(new Constante(-1), new Pow(new Csc(a), new Constante(2))), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Cot(args[0]);
        }
    }

    public class Arcos : Unaria
    {
        private Funcion a;

        public Arcos(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Acos(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Multliplicacion(new Constante(-1), new Division(new Constante(1), new Pow(new Resta(new Constante(1), new Pow(a, new Constante(2))), new Constante(0.5)))), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Arcos(args[0]);
        }
    }

    public class Arcsin : Unaria
    {
        private Funcion a;

        public Arcsin(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Asin(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Division(new Constante(1), new Pow(new Resta(new Constante(1), new Pow(a, new Constante(2))), new Constante(0.5))), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Arcsin(args[0]);
        }
    }

    public class Arctan : Unaria
    {
        private Funcion a;

        public Arctan(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Atan(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Division(new Constante(1), new Suma(new Constante(1), new Pow(a, new Constante(2)))), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Arctan(args[0]);
        }
    }

    public class LogN : Unaria
    {
        private Funcion a;

        public LogN(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Log(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Division(new Constante(1), a), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new LogN(args[0]);
        }
    }

    public class ParteEntera : Unaria
    {
        private Funcion a;

        public ParteEntera(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        protected override double Operacion(double x)
        {
            return Math.Truncate(x);
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Constante(0), a.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new ParteEntera(args[0]);
        }
    }

    public class Csc : Unaria
    {
        private Funcion a;

        public Csc(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Multliplicacion(new Multliplicacion(new Constante(-1), new Csc(a)), new Cot(a)), a.Derivate());
        }

        protected override double Operacion(double x)
        {
            return (1 / Math.Sin(x));
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Csc(args[0]);
        }
    }

    public class Sec : Unaria
    {
        private Funcion a;

        public Sec(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Multliplicacion(new Sec(a), new Tan(a)), a.Derivate());
        }

        protected override double Operacion(double x)
        {
            return (1 / Math.Cos(x));
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Sec(args[0]);
        }
    }

    public class Abs : Unaria
    {
        private Funcion a;

        public Abs(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Sign(a), a.Derivate());
        }

        protected override double Operacion(double x)
        {
            return Math.Abs(x);
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Abs(args[0]);
        }
    }

    public class Sign : Unaria
    {
        private Funcion a;

        public Sign(Funcion a)
            : base(a)
        {
            this.a = a;
        }

        public override Funcion Derivate()
        {
            return new Multliplicacion(new Constante(0), a.Derivate());
        }

        protected override double Operacion(double x)
        {
            return Math.Sign(x);
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Sign(args[0]);
        }
    }

    #endregion

    #region Herederos de Función Binaria

    public class Suma : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Suma(Funcion a, Funcion b)
            : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            return x + y;
        }

        public override Funcion Derivate()
        {
            return new Suma(a.Derivate(), b.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Suma(args[0], args[1]);
        }
    }

    public class Resta : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Resta(Funcion a, Funcion b)
            : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            return x - y;
        }

        public override Funcion Derivate()
        {
            return new Resta(a.Derivate(), b.Derivate());
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Resta(args[0], args[1]);
        }
    }

    public class Multliplicacion : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Multliplicacion(Funcion a, Funcion b)
            : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            return x * y;
        }

        public override Funcion Derivate()
        {
            return new Suma(new Multliplicacion(a.Derivate(), b),new Multliplicacion(a, b.Derivate()));
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Multliplicacion(args[0], args[1]);
        }
    }

    public class Division : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Division(Funcion a, Funcion b)
            : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            if (y == 0) throw new InvalidOperationException("No se puede dividir por 0.");
            return x / y;
        }

        public override string ToString()
        {
            x = double.Parse(a.ToString());
            y = double.Parse(b.ToString());

            if (y == 0) throw new InvalidOperationException("No se puede dividir por 0.");

            return (x / y).ToString();
        }


        public override Funcion Derivate()
        {
            return
                new Division(
                    new Resta(new Multliplicacion(a.Derivate(), b),new Multliplicacion(a, b.Derivate())),
                    new Pow(b, new Constante(2)));
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Division(args[0], args[1]);
        }
    }

    public class Pow : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Pow(Funcion a, Funcion b)
            : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            return Math.Pow(x, y);
        }

        public override Funcion Derivate()
        {
            if (b is Constante) return new Multliplicacion(b, new Pow(a, new Resta(b, new Constante(1))));
            
            return new Multliplicacion(new Pow(a, b), new Suma(new Multliplicacion(b.Derivate(), new LogN(a)), new Division(new Multliplicacion(b, a.Derivate()), a)));
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Pow(args[0], args[1]);
        }
    }

    public class Resto : Binaria
    {
        private double x, y;
        private Funcion a, b;

        public Resto(Funcion a, Funcion b) : base(a, b)
        {
            this.a = a;
            this.b = b;
        }

        public override Funcion Derivate()
        {
            return new Resto(a.Derivate(), b.Derivate());
        }

        protected override double Operacion(double x, double y)
        {
            this.x = x;
            this.y = y;
            return x % y;
        }

        public static Funcion Create(params Funcion[] args)
        {
            return new Resto(args[0], args[1]);
        }
    }

    #endregion

    #region Expresiones booleanas

    public interface IBooleanExpressions
    {
        bool Evaluate(double x);
    }


    /// <summary>
    /// Clase base
    /// </summary>
    public abstract class Comparador : IBooleanExpressions
    {
        internal Funcion a;
        internal Funcion b;
        protected Comparador(Funcion a, Funcion b)
        {
            this.a = a;
            this.b = b;
        }

        public abstract bool Evaluate(double x);
    }

    /// <summary>
    /// Expresión booleana ">"
    /// </summary>
    public class MayorQue : Comparador
    {
        private MayorQue(Funcion a, Funcion b)
            : base(a, b)
        {
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x) > b.Evaluar(x);
        }

        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new MayorQue(args[0], args[1]);
        }
    }

    /// <summary>
    /// Expresión booleana "=="
    /// </summary>
    public class Igual : Comparador
    {
        private Igual(Funcion a, Funcion b)
            : base(a, b)
        {
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x) == b.Evaluar(x);
        }

        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new Igual(args[0], args[1]);
        }
    }

    /// <summary>
    /// Expresión booleana "<".
    /// </summary>
    public class MenorQue : Comparador
    {
        private MenorQue(Funcion a, Funcion b)
            : base(a, b)
        {}
        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new MenorQue(args[0], args[1]);
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x)< b.Evaluar(x);
        }
    }

    /// <summary>
    /// Expresión booleana "<=".
    /// </summary>
    public class MenorIgual : Comparador
    {
        private MenorIgual(Funcion a, Funcion b)
            : base(a, b)
        {
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x) <= b.Evaluar(x);
        }

        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new MenorIgual(args[0], args[1]);
        }
    }

    /// <summary>
    /// Expresión booleana ">=".
    /// </summary>
    public class MayorIgual : Comparador
    {
        private MayorIgual(Funcion a, Funcion b)
            : base(a, b)
        {
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x) >= b.Evaluar(x);
        }

        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new MayorIgual(args[0], args[1]);
        }
    }

    /// <summary>
    /// Expresión booleana "!=".
    /// </summary>
    public class Diferente : Comparador
    {
        private Diferente(Funcion a, Funcion b)
            : base(a, b)
        {
        }

        public override bool Evaluate(double x)
        {
            return a.Evaluar(x) != b.Evaluar(x);
        }

        public static IBooleanExpressions Create(params Funcion[] args)
        {
            return new Diferente(args[0], args[1]);
        }
    }

    public interface IOperadoresLogicos
    {
        bool Evaluate(bool left, bool right);
    }

    /// <summary>
    /// Expresión booleana "&&".
    /// </summary>
    public class And :IOperadoresLogicos
    {
        private bool left;
        private bool right;

        public And(bool a, bool b)
        {
            left = a;
            right = b;
        }
        public bool Evaluate(bool left, bool right)
        {
            return left && right;
        }

        public static IOperadoresLogicos Create(params bool[] args)
        {
            return new And(args[0], args[1]);
        }
    }

    /// <summary>
    /// Expresión booleana "||".
    /// </summary>
    public class Or:IOperadoresLogicos 
    {
        private bool left;
        private bool right;

        public Or(bool a, bool b)
        {
            left = a;
            right = b;
        }

        public bool Evaluate(bool left, bool right)
        {
            return left || right;
        }

        public static IOperadoresLogicos Create(params bool[] args)
        {
            return new Or(args[0], args[1]);
        }
    }

    #endregion
}
