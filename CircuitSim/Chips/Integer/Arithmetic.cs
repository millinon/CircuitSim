using System;

namespace CircuitSim.Chips.Integer.Arithmetic
{
    public class Inv : UnaryFunctor<int, int>
    {
        private static uint count = 0;

        public Inv() : base($"IntInv{count++}", a => 1 / a) { }
    }

    public class Abs : UnaryFunctor<int, int>
    {
        private static uint count = 0;

        public Abs() : base($"IntAbs{count++}", Math.Abs) { }
    }

    public class Neg : UnaryFunctor<int, int>
    {
        private static uint count = 0;

        public Neg() : base($"IntNeg{count++}", a => -a) { }
    }

    public class Sign : UnaryFunctor<int, int>
    {
        private static uint count = 0;

        public Sign() : base($"IntSign{count++}", a => Math.Sign(a)) { }
    }

    public class Add : NToOne<int>
    {
        private static uint count = 0;

        public Add(int numInputs = 2) : base(numInputs, $"IntAdd{count++}") { }

        public override void Compute()
        {
            _out = 0;
            for (int i = 0; i < NumInputs; i++)
            {
                _out += Inputs[i].Value;
            }
        }
    }

    public class Sub : BinaryFunctor<int, int, int>
    {
        private static uint count = 0;

        public Sub() : base($"IntSub{count++}", (a, b) => a - b) { }
    }

    public class Mul : NToOne<int>
    {
        private static uint count = 0;

        public Mul(int numInputs = 2) : base(numInputs, $"IntMul{count++}") { }

        public override void Compute()
        {
            _out = 1;
            for (int i = 0; i < NumInputs; i++)
            {
                _out *= Inputs[i].Value;
            }
        }
    }

    public class Div : BinaryFunctor<int, int, int>
    {
        private static uint count = 0;

        public Div() : base($"IntDiv{count++}", (a, b) => a / b) { }
    }

    public class Mod : BinaryFunctor<int, int, int>
    {
        private static uint count = 0;

        public Mod() : base($"IntMod{count++}", (a, b) => a % b) { }
    }

    public class Min : BinaryFunctor<int, int, int>
    {
        private static uint count = 0;

        public Min() : base($"IntMin{count++}", Math.Min) { }
    }

    public class Max : BinaryFunctor<int, int, int>
    {
        private static uint count = 0;

        public Max() : base($"IntMax{count++}", Math.Max) { }
    }
}
