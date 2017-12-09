namespace CircuitSim.Chips.String.Comparison
{
    public class Equal : BinaryComparator<string>
    {
        private static uint count = 0;

        public Equal() : base($"StrEqual{count++}", (a, b) => a == b) { }
    }

    public class EqualIgnoreCase : BinaryComparator<string>
    {
        private static uint count = 0;

        public EqualIgnoreCase() : base($"StrEqualIgnoreCase{count++}", (a, b) => string.Equals(a, b, System.StringComparison.OrdinalIgnoreCase)) { }
    }

    public class NotEqual : BinaryComparator<string>
    {
        private static uint count = 0;

        public NotEqual() : base($"StrNotEqual{count++}", (a, b) => a != b) { }
    }

    public class GreaterThan : BinaryComparator<string>
    {
        private static uint count = 0;

        public GreaterThan() : base($"StrGreaterThan{count++}", (a, b) => string.Compare(a, b) > 0) { }
    }
    
    public class LessThan : BinaryComparator<string>
    {
        private static uint count = 0;

        public LessThan() : base($"StrLessThan{count++}", (a, b) => string.Compare(a, b) < 0) { }
    }

    public class GreaterThanOrEqual : BinaryComparator<string>
    {
        private static uint count = 0;

        public GreaterThanOrEqual() : base($"StrGreaterThanEqual{count++}", (a, b) => string.Compare(a, b) >= 0) { }
    }
    
    public class LessThanOrEqual : BinaryComparator<string>
    {
        private static uint count = 0;

        public LessThanOrEqual() : base($"StrLessThanEqual{count++}", (a, b) => string.Compare(a, b) <= 0) { }
    }
}
