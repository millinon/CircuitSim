using System;

namespace CircuitSim.Chips.Digital
{
    public class Random : Random<bool>
    {
        private static uint count = 0;

        public Random(int seed = -1) : base($"DigitalRandom{count++}", seed) { }

        public override void Compute()
        {
            _out = r.NextDouble() <= 0.5;
        }
    }

    public class RisingEdge : Signal.EdgeDetector<bool>
    {
        private static int count = 0;

        public RisingEdge() : base($"DigitalRisingEdge{count++}") { }

        public override void Compute()
        {
            edge = Inputs.A.Value && !last;
            last = Inputs.A.Value;
        }
    }

    public class FallingEdge : Signal.EdgeDetector<bool>
    {
        private static int count = 0;

        public FallingEdge() : base($"DigitalFallingEdge{count++}") { }

        public override void Compute()
        {
            edge = !Inputs.A.Value && last;
            last = Inputs.A.Value;
        }
    }

    public class AND : NToOne<bool>
    {
        private static uint count = 0;

        public AND(int numInputs = 2) : base(numInputs, $"AND{count++}")
        {
            if (numInputs < 2)
            {
                throw new ArgumentException($"component {Name} requires at least 2 inputs");
            }
        }

        public override void Compute()
        {
            var tmp = true;

            for (int i = 0; i < Inputs.Length; i++)
            {
                if (!Inputs[i].Value)
                {
                    tmp = false;
                }
            }

            _out = tmp;
        }
    }

    public class OR : NToOne<bool>
    {
        private static uint count = 0;

        public OR(int numInputs = 2) : base(numInputs, $"OR{count++}")
        {
            if (numInputs < 2)
            {
                throw new ArgumentException($"component {Name} requires at least 2 inputs");
            }
        }

        public override void Compute()
        {
            var tmp = false;
            for (int i = 0; i < NumInputs; i++)
            {
                if (Inputs[i].Value)
                {
                    tmp = true;
                }
            }

            _out = tmp;
        }
    }

    public class NAND : NToOne<bool>
    {
        private static uint count = 0;

        private AND and;
        private NOT not;

        public NAND(int numInputs = 2) : base(numInputs, $"NAND{count++}")
        {
            if (numInputs < 2)
            {
                throw new ArgumentException($"component {Name} requires at least 2 inputs");
            }

            and = new AND(numInputs);
            not = new NOT();
        }

        public override void Compute()
        {
            for (int i = 0; i < NumInputs; i++)
            {
                and.Inputs[i] = Inputs[i];
            }

            not.Inputs.A.Source = and.Out;

            and.Tick();
            not.Tick();

            _out = not.Outputs.Out.Value;
        }
    }

    public class NOR : NToOne<bool>
    {
        private static uint count = 0;

        private OR or;
        private NOT not;

        public NOR(int numInputs = 21) : base(numInputs, $"NOR{count++}")
        {
            if (numInputs < 2)
            {
                throw new ArgumentException($"component {Name} requires at least 2 inputs");
            }

            or = new OR(numInputs);
            not = new NOT();

            not.Inputs.A.Source = or.Out;
        }

        public override void Compute()
        {
            for (int i = 0; i < NumInputs; i++)
            {
                or.Inputs[i] = Inputs[i];
            }

            or.Tick();
            not.Tick();

            _out = not.Outputs.Out.Value;
        }
    }

    public class XOR : NToOne<bool>
    {
        private static uint count = 0;

        private bool _strictMode;

        public XOR(int numInputs = 2, bool strictMode = false) : base(numInputs, $"XOR{count++}")
        {
            _strictMode = strictMode;
        }

        public override void Compute()
        {
            bool parity = false;

            // if _strictMode is enabled, then XOR outputs true if exactly one of its inputs are true
            if (_strictMode)
            {
                for (int i = 0; i < NumInputs; i++)
                {
                    if (Inputs[i].Value)
                    {
                        if (parity)
                        {
                            parity = false;
                            break;
                        }
                        else
                        {
                            parity = true;
                        }
                    }
                }
            }
            else // otherwise it computes the parity of the input bits, which seems to be the generally expected behavior
            {
                for (int i = 0; i < NumInputs; i++)
                {
                    if (Inputs[i].Value)
                    {
                        parity = !parity;
                    }
                }
            }

            _out = parity;
        }
    }

    public class NOT : Predicate<bool>
    {
        private static uint count = 0;

        public NOT() : base($"NOT{count++}", a => !a) { }
    }
}
