namespace CircuitSim.Chips.Conversion
{
    public class DigitalToAnalog : UnaryFunctor<bool, double>
    {
        private static uint count = 0;

        public DigitalToAnalog(double low, double high) : base($"DigitalToAnalog{count++}", a => a ? high : low) { }
    }

    public class DigitalToString : UnaryFunctor<bool, string>
    {
        private static uint count = 0;

        public DigitalToString() : base($"DigitalToString{count++}", a => a.ToString()) { }
    }

    public class FloatToString : UnaryFunctor<double, string>
    {
        private static uint count = 0;

        public FloatToString() : base($"FloatToString{count++}", a => a.ToString()) { }
    }

    public class IntToString : UnaryFunctor<int, string>
    {
        private static uint count = 0;

        public IntToString() : base($"IntToString{count++}", a => a.ToString()) { }
    }

    public class IntToFloat : UnaryFunctor<int, double>
    {
        private static uint count = 0;

        public IntToFloat() : base($"IntToFloat{count++}", a => (double)a) { }
    }

    public class FloatToInt : UnaryFunctor<double, int>
    {
        private static uint count = 0;

        public FloatToInt() : base($"IntToString{count++}", a => (int)a) { }
    }

    public class DigitalToInt : UnaryFunctor<bool, int>
    {
        private static uint count = 0;

        public DigitalToInt() : base($"DigitalToInt{count++}", a => a ? 1 : 0) { }
    }

    public class DigitalToFloat : UnaryFunctor<bool, double>
    {
        private static uint count = 0;

        public DigitalToFloat() : base($"DigitalToFloat{count++}", a => a ? 1.0 : 0.0) { }
    }
}
