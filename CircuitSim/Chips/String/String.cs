using CircuitSim.Chips;

namespace String
{
    public class Empty : Predicate<string>
    {
        private static uint count = 0;

        public Empty() : base($"Empty{count++}", string.IsNullOrEmpty) { }
    }

    public class Concat : BinaryFunctor<string, string, string>
    {
        private static uint count = 0;

        public Concat() : base($"Concat{count++}", string.Concat) { }
    }

    public class ToLower : UnaryFunctor<string, string>
    {
        private static uint count = 0;

        public ToLower() : base($"ToLower{count++}", (s) => s.ToLower()) { }
    }

    public class ToUpper : UnaryFunctor<string, string>
    {
        private static uint count = 0;

        public ToUpper() : base($"ToUpper{count++}", (s) => s.ToUpper()) { }
    }

    public class StartsWith : BinaryComparator<string>
    {
        private static uint count = 0;

        public StartsWith() : base($"StartsWith{count++}", (a, b) => a.StartsWith(b)) { }
    }

    public class EndsWith : BinaryComparator<string>
    {
        private static uint count = 0;

        public EndsWith() : base($"EndsWith{count++}", (a, b) => a.EndsWith(b)) { }
    }

    public class Accumulator : Accumulator<string>
    {
        private static uint count = 0;

        public Accumulator(string increment, string start = "") : base($"StrAccumulator{count++}", increment, start) { }

        public override void Increment()
        {
            state += increment;
            Tick();
        }
    }
}
