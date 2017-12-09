namespace CircuitSim.Chips.FloatingPoint.Comparison
{
    public class Equal : BinaryComparator<double>
    {
        private static uint count = 0;

        public Equal() : base($"FltEqual{count++}", (a, b) => a == b) { }
    }

    public class NotEqual : BinaryComparator<double>
    {
        private static uint count = 0;

        public NotEqual() : base($"FltNotEqual{count++}", (a, b) => a != b) { }
    }

    public class GreaterThan : BinaryComparator<double>
    {
        private static uint count = 0;

        public GreaterThan() : base($"FltGreaterThan{count++}", (a, b) => a > b) { }
    }
    
    public class LessThan : BinaryComparator<double>
    {
        private static uint count = 0;

        public LessThan() : base($"FltLessThan{count++}", (a, b) => a < b) { }
    }

    public class GreaterThanOrEqual : BinaryComparator<double>
    {
        private static uint count = 0;

        public GreaterThanOrEqual() : base($"FltGreaterThanEqual{count++}", (a, b) => a >= b) { }
    }
    
    public class LessThanOrEqual : BinaryComparator<double>
    {
        private static uint count = 0;

        public LessThanOrEqual() : base($"FltLessThanEqual{count++}", (a, b) => a <= b) { }
    }
}
