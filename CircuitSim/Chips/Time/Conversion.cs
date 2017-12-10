namespace CircuitSim.Chips.Time.Conversion
{
    public class ToString : UnaryFunctor<long, string>
    {
        private static uint count = 0;

        public ToString() : base($"TimeToString{count++}", a => a.ToString()) { }
    }

    public class ToFloat : UnaryFunctor<long, double>
    {
        private static uint count = 0;

        public ToFloat() : base($"TimeToFlt{count++}", a => (float) a) { }
    }

    public class ToInt : UnaryFunctor<long, int>
    {
        private static uint count = 0;

        public ToInt() : base($"TimeToInt{count++}", a => (int) a) { }
    }
    
    public class ToIntLow : UnaryFunctor<long, int>
    {
        private static uint count = 0;

        public ToIntLow() : base($"TimeToIntLow{count++}", a => (int) ((ulong)a & 0x00000000ffffffff)) { }
    }
    
    public class ToIntHigh : UnaryFunctor<long, int>
    {
        private static uint count = 0;

        public ToIntHigh() : base($"TimeToIntHigh{count++}", a => (int) (((ulong)a & 0xffffffff00000000) >> 32)) { }
    }
}
