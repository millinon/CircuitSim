namespace CircuitSim.Chips.String
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

    public class Repeat : Component
    {
        private static uint count = 0;

        public class RepeatInputs : Inputs
        {
            public Input<string> Str;
            public Input<int> Num;

            public RepeatInputs(Component component) : base(2)
            {
                Str = new Input<string>(component, "Str");
                Num = new Input<int>(component, "Num");

            }

            public override void Detach()
            {
                Str.Detach();
                Num.Detach();
            }
        }

        public readonly RepeatInputs Inputs;

        public readonly Outputs<string> Outputs;
        private string _out;

        public Repeat() : base($"StrRepeat{count++}")
        {
            Inputs = new RepeatInputs(this);

            Outputs = new Outputs<string>(this);
        }

        public override void Compute()
        {
            _out = string.Concat(System.Linq.Enumerable.Repeat(Inputs.Str.Value, Inputs.Num.Value));
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }
    }
}
