namespace CircuitSim.Chips.Digital.Comparison
{
    public class Equal : BinaryComparator<bool>
    {
        private static uint count = 0;

        public Equal() : base($"DigitalEqual{count++}", (a, b) => a == b) { }
    }

    public class NotEqual : BinaryComparator<bool>
    {
        private static uint count = 0;

        public NotEqual() : base($"DigitalNotEqual{count++}", (a, b) => a != b) { }
    }
}
