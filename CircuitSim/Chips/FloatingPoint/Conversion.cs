using System;

namespace CircuitSim.Chips.FloatingPoint.Conversion
{
    public class ToString : UnaryFunctor<double, string>
    {
        private static uint count = 0;

        public ToString() : base($"FloatToString{count++}", a => a.ToString()) { }
    }

    public class Ceil : UnaryFunctor<double, int>
    {
        private static uint count = 0;

        public Ceil() : base($"Ceil{count++}", a => (int) Math.Ceiling(a)) { }

    }
    
    public class Floor : UnaryFunctor<double, int>
    {
        private static uint count = 0;

        public Floor() : base($"Floor{count++}", a => (int) Math.Floor(a)) { }
    }

    public class Round : UnaryFunctor<double, int>
    {
        private static uint count = 0;

        public Round() : base($"Round{count++}", a => (int) Math.Round(a)) { }
    }

}
