namespace CircuitSim.Chips.Digital.Conversion
{
    public class ToAnalog : UnaryFunctor<bool, double>
    {
        private static uint count = 0;

        public ToAnalog(double low, double high) : base($"DigitalToAnalog{count++}", a => a ? high : low) { }
    }

    public class ToInteger : UnaryFunctor<bool, int>
    {
        private static uint count = 0;
        
        public ToInteger() : base($"DigitalToInteger{count++}", a => a ? 1 : 0) { }

        public ToInteger(int low, int high) : base($"DigitalToInteger{count++}", a => a ? high : low) { }
    }

    public class ToString : UnaryFunctor<bool, string>
    {
        private static uint count = 0;

        public ToString() : base($"DigitalToString{count++}", a => a.ToString()) { }

        public ToString(string low, string high) : base($"DigitalToString{count++}", a => a ? high : low) { }
        
    }
}
