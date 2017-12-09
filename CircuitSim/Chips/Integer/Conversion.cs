namespace CircuitSim.Chips.Integer.Conversion
{
    public class ToString : UnaryFunctor<int, string>
    {
        private static uint count = 0;

        public ToString() : base($"IntToString{count++}", a => a.ToString()) { }
    }

    public class ToFloat : UnaryFunctor<int, double>
    {
        private static uint count = 0;

        public ToFloat() : base($"IntToFloat{count++}", a => (double) a) { }
    }

    public class ToDigital : UnaryFunctor<int, bool>
    {
        private static uint count = 0;

        public ToDigital() : base($"IntToDigital{count++}", a => a != 0) { }
    }
}
