using System;

namespace CircuitSim.Chips.FloatingPoint
{
    public class Random : Random<double>
    {
        private static uint count = 0;

        public Random(int seed = -1) : base($"FltRandom{count++}", seed) { }

        public override void Compute()
        {
            _out = r.NextDouble();
        }
    }

    public class RisingEdge : Signal.EdgeDetector<double>
    {
        private static int count = 0;

        public RisingEdge() : base($"FltRisingEdge{count++}") { }

        public override void Compute()
        {
            edge = Inputs.A.Value > last;
            last = Inputs.A.Value;
        }
    }

    public class FallingEdge : Signal.EdgeDetector<double>
    {
        private static int count = 0;

        public FallingEdge() : base($"FltFallingEdge{count++}") { }

        public override void Compute()
        {
            edge = Inputs.A.Value < last;
            last = Inputs.A.Value;
        }
    }

    public class Accumulator : Accumulator<double>
    {
        private static uint count = 0;

        public Accumulator(double start = 0.0, double increment = 1.0) : base($"FltAccumulator{count++}", increment, start) { }

        public override void Increment()
        {
            state += increment;
            Tick();
        }
    }

    public class Counter : Counter<double>
    {
        private static uint count = 0;

        public Counter(double start = 0, double increment = 1, double decrement = 1) : base($"FltCounter{count++}", increment, decrement, start) { }

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
}
