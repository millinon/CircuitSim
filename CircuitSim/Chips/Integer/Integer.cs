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
