namespace CircuitSim.Chips.Integer.Comparison
{
    public class Equal : BinaryComparator<int>
    {
        private static uint count = 0;

        public Equal() : base($"IntEqual{count++}", (a, b) => a == b) { }
    }

    public class NotEqual : BinaryComparator<int>
    {
        private static uint count = 0;

        public NotEqual() : base($"IntNotEqual{count++}", (a, b) => a != b) { }
    }

    public class GreaterThan : BinaryComparator<int>
    {
        private static uint count = 0;

        public GreaterThan() : base($"IntGreaterThan{count++}", (a, b) => a > b) { }
    }
    
    public class LessThan : BinaryComparator<int>
    {
        private static uint count = 0;

        public LessThan() : base($"IntLessThan{count++}", (a, b) => a < b) { }
    }

    public class GreaterThanOrEqual : BinaryComparator<int>
    {
        private static uint count = 0;

        public GreaterThanOrEqual() : base($"IntGreaterThanEqual{count++}", (a, b) => a >= b) { }
    }
    
    public class LessThanOrEqual : BinaryComparator<int>
    {
        private static uint count = 0;

        public LessThanOrEqual() : base($"IntLessThanEqual{count++}", (a, b) => a <= b) { }
    }
}
