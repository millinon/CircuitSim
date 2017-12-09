using System;
using System.Collections.Generic;

namespace CircuitSim.Chips.Signal
{
    public abstract class EdgeDetector<T> : Component
    {
        public readonly Inputs<T> Inputs;

        public readonly Outputs<bool> Outputs;

        protected T last;
        protected bool edge = false;

        public EdgeDetector(string ComponentName) : base(ComponentName)
        {
            Inputs = new Inputs<T>(this);

            Outputs = new Outputs<bool>(this);
        }

        public override void Set()
        {
            Outputs.Out.Value = edge;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }
    }

    public class Hold : Component
    {
        private static uint count = 0;

        public enum Type
        {
            Cycles,
            Time
        }

        private readonly Type type;

        private readonly TimeSpan duration;
        private DateTime end = DateTime.Now;
        private readonly ulong cycles;
        private ulong remaining = 0;

        private Input<bool> _a;
        public Input<bool> A
        {
            get
            {
                return _a;
            }

            set
            {
                if (_a != null && _a != value) _a.Detach();
                _a = value;
            }
        }

        public readonly Output<bool> Out;
        private bool _out;

        public Hold(TimeSpan span) : base($"Hold{count++}")
        {
            Out = new Output<bool>(this, "Out");

            type = Type.Time;
            duration = span;
        }

        public Hold(ulong cycles) : base($"Hold{count++}")
        {
            Out = new Output<bool>(this, "Out");

            type = Type.Cycles;
            this.cycles = cycles;
        }

        public override void Compute()
        {
            switch (type)
            {
                case Type.Cycles:
                    if (A.Value)
                    {
                        remaining = cycles;
                        _out = true;
                    }
                    else
                    {
                        if (remaining > 0) remaining--;
                        _out = remaining > 0;
                    }
                    break;

                case Type.Time:
                    if (A.Value)
                    {
                        end = DateTime.Now + duration;
                        _out = true;
                    }
                    else
                    {
                        _out = DateTime.Now < end;
                    }

                    break;
            }
        }

        public override void Set()
        {
            Out.Value = _out;
        }

        public override void Detach()
        {
            A.Detach();

            Out.Detach();
        }

        public List<Input<bool>> GetInputs() => new List<Input<bool>>() { A };

        public List<Output<bool>> GetOutputs() => new List<Output<bool>>() { Out };
    }
}
