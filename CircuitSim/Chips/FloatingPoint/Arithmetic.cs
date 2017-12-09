using System;

namespace CircuitSim.Chips.FloatingPoint.Arithmetic
{
    public class Inv : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Inv() : base($"FltInv{count++}", a => 1.0 / a) { }
    }

    public class Sin : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Sin() : base($"Sin{count++}", Math.Sin) { }
    }

    public class Cos : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Cos() : base($"Cos{count++}", Math.Cos) { }
    }

    public class Tan : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Tan() : base($"Tan{count++}", Math.Tan) { }
    }

    public class Sinh : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Sinh() : base($"Sinh{count++}", Math.Sinh) { }
    }

    public class Cosh : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Cosh() : base($"Cosh{count++}", Math.Cosh) { }
    }

    public class Tanh : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Tanh() : base($"Tanh{count++}", Math.Tanh) { }
    }

    public class Asin : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Asin() : base($"Asin{count++}", Math.Asin) { }
    }

    public class Acos : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Acos() : base($"Acos{count++}", Math.Acos) { }
    }

    public class Atan : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Atan() : base($"Atan{count++}", Math.Atan) { }
    }

    public class Exp : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Exp() : base($"Exp{count++}", Math.Exp) { }
    }

    public class Log : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Log() : base($"Exp{count++}", Math.Log) { }
    }

    public class Abs : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Abs() : base($"FltAbs{count++}", Math.Abs) { }
    }

    public class Neg : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Neg() : base($"FltNeg{count++}", a => -a) { }
    }

    public class Ceil : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Ceil() : base($"Ceil{count++}", Math.Ceiling) { }
    }

    public class Floor : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Floor() : base($"Floor{count++}", Math.Floor) { }
    }

    public class Sign : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Sign() : base($"FltSign{count++}", a => Math.Sign(a)) { }
    }

    public class Round : UnaryFunctor<double, double>
    {
        private static uint count = 0;

        public Round() : base($"Round{count++}", Math.Round) { }
    }

    public class Add : NToOne<double>
    {
        private static uint count = 0;

        public Add(int numInputs = 2) : base(numInputs, $"FltAdd{count++}") { }

        public override void Compute()
        {
            _out = 0.0;
            for (int i = 0; i < NumInputs; i++)
            {
                _out += Inputs[i].Value;
            }
        }
    }

    public class Sub : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Sub() : base($"FltSub{count++}", (a, b) => a - b) { }
    }

    public class Mul : NToOne<double>
    {
        private static uint count = 0;

        public Mul(int numInputs = 2) : base(numInputs, $"FltMul{count++}") { }

        public override void Compute()
        {
            _out = 1.0;
            for (int i = 0; i < NumInputs; i++)
            {
                _out *= Inputs[i].Value;
            }
        }
    }

    public class Div : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Div() : base($"FltDiv{count++}", (a, b) => a / b) { }
    }

    public class Mod : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Mod() : base($"FltMod{count++}", (a, b) => a % b) { }
    }

    public class Pow : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Pow() : base($"Pow{count++}", Math.Pow) { }
    }

    public class Min : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Min() : base($"FltMin{count++}", Math.Min) { }
    }

    public class Max : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public Max() : base($"FltMax{count++}", Math.Max) { }
    }

    public class LogN : BinaryFunctor<double, double, double>
    {
        private static uint count = 0;

        public LogN() : base($"LogN{count++}", Math.Log) { }
    }
}
