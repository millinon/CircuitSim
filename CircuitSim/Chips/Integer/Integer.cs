using System;
using System.Collections.Generic;

namespace CircuitSim.Chips.Integer
{
    public class Random : Random<int>
    {
        private static uint count = 0;

        public Random(int seed = -1) : base($"IntRandom{count++}", seed) { }

        public override void Compute()
        {
            _out = r.Next();
        }
    }

    public class RisingEdge : Signal.EdgeDetector<int>
    {
        private static int count = 0;

        public RisingEdge() : base($"IntRisingEdge{count++}") { }

        public override void Compute()
        {
            edge = Inputs.A.Value > last;
            last = Inputs.A.Value;
        }
    }

    public class FallingEdge : Signal.EdgeDetector<int>
    {
        private static int count = 0;

        public FallingEdge() : base($"IntFallingEdge{count++}") { }

        public override void Compute()
        {
            edge = Inputs.A.Value < last;
            last = Inputs.A.Value;
        }
    }

    public class Accumulator : Accumulator<int>
    {
        private static uint count = 0;

        public Accumulator(int start = 0, int increment = 1) : base($"IntAccumulator{count++}", increment, start) { }

        public override void Increment()
        {
            state += increment;
            Tick();
        }
    }

    public class Counter : Counter<int>
    {
        private static uint count = 0;

        public Counter(int start = 0, int increment = 1, int decrement = 1) : base($"IntCounter{count++}", increment, decrement, start) { }

        public override void Increment()
        {
            state += increment;
            Tick();
        }

        public override void Decrement()
        {
            state -= decrement;
            Tick();
        }
    }

    public class IntSum : Component
    {
        private static uint count = 0;

        public Inputs<List<int>> Inputs;

        public readonly Outputs<int> Outputs;
        private int _out;

        private List.Reduce<int, int> reducer = new List.Reduce<int, int>("sum");
        private Arithmetic.Add adder = new Arithmetic.Add();
        private Input.Constant<int> zero = new Chips.Input.Constant<int>(0);

        public IntSum() : base($"IntSum{count++}")
        {
            Inputs = new Inputs<List<int>>(this);

            Outputs = new Outputs<int>(this);

            zero.Tick();
        }

        public override void Compute()
        {
            reducer.Inputs.A = Inputs.A;
            reducer.Inputs.Func.Source = adder.Out;
            reducer.Inputs.StartingValue.Source = zero.Outputs[0];
            adder.Inputs[0].Source = reducer.Outputs.CurrentVal;
            adder.Inputs[1].Source = reducer.Outputs.FuncInput;

            reducer.Tick();
            _out = reducer.Outputs.Out.Value;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }
    }

    namespace Comparison
    {
        public class EQ : BinaryComparator<int>
        {
            private static uint count = 0;

            public EQ() : base($"IntEQ{count++}", (a, b) => a == b) { }
        }

        public class GT : BinaryComparator<int>
        {
            private static uint count = 0;

            public GT() : base($"IntGT{count++}", (a, b) => a > b) { }
        }

        public class GTE : BinaryComparator<int>
        {
            private static uint count = 0;

            public GTE() : base($"IntGTE{count++}", (a, b) => a >= b) { }
        }

        public class LT : BinaryComparator<int>
        {
            private static uint count = 0;

            public LT() : base($"IntLT{count++}", (a, b) => a < b) { }
        }

        public class LTE : BinaryComparator<int>
        {
            private static uint count = 0;

            public LTE() : base($"IntGTE{count++}", (a, b) => a >= b) { }
        }

        public class NEQ : BinaryComparator<int>
        {
            private static uint count = 0;

            public NEQ() : base($"IntNEQ{count++}", (a, b) => a != b) { }
        }
    }

    namespace Arithmetic
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

    public class IsEven : Predicate<int>
    {
        private static uint count = 0;

        public IsEven() : base($"IsEven{count++}", a => a % 2 == 0) { }
    }

    public class IsOdd : Predicate<int>
    {
        private static uint count = 0;

        public IsOdd() : base($"IsOdd{count++}", a => a % 2 == 1) { }
    }
}
