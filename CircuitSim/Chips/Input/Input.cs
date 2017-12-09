using System.Collections.Generic;

namespace CircuitSim.Chips.Input
{
    // base class for a set of constant values
    public class Constant<T> : Component
    {
        private static uint count = 0;

        public readonly Output<T>[] Outputs;
        public readonly int NumOutputs;
        private T[] _vals;

        public Constant(T[] Values) : base($"Constant{count++}")
        {
            _vals = Values;
            NumOutputs = Values.Length;

            Outputs = new Output<T>[NumOutputs];
            for (int i = 0; i < NumOutputs; i++)
            {
                Outputs[i] = new Output<T>(this, $"Output{i}");
            }
        }

        public Constant(T Value) : this(new T[] { Value }) { }

        public override void Compute() { }

        public override void Set()
        {
            for (int i = 0; i < NumOutputs; i++)
            {
                Outputs[i].Value = _vals[i];
            }
        }

        public List<Output<T>> GetOutputs()
        {
            return new List<Output<T>>(Outputs);
        }

        public override void Detach()
        {
            GetOutputs().ForEach(output => output.Detach());
        }
    }

    // a generic boolean input
    public class Button : Component
    {
        private static uint count = 0;

        public readonly Outputs<bool> Outputs;

        private bool _state = false;
        public bool State
        {
            get { return _state; }
            set { _state = value; Tick(); }
        }

        public void Toggle()
        {
            _state = !_state;
            Tick();
        }

        public Button() : base($"Button{count++}")
        {

            Outputs = new Outputs<bool>(this);
        }

        public override void Compute() { }

        public override void Set() => Outputs.Out.Value = _state;

        public override void Detach() => Outputs.Detach();
    }

    // a generic input
    public class GenericInput<T> : Component
    {
        private static uint count = 0;

        public readonly Outputs<T> Outputs;
        private T _out;

        private T _val;
        public T Value
        {
            get
            {
                return _val;
            }

            set
            {
                _val = value; Tick();
            }
        }

        public GenericInput() : base($"Input{count++}")
        {
            Outputs = new Outputs<T>(this);
        }

        public override void Compute()
        {
            _out = _val;
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            Outputs.Detach();
        }
    }

    public class ByteInput : Component
    {
        private static uint count = 0;

        private byte _val;
        public byte Value
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value; Tick();
            }
        }

        public readonly Output<bool> A;
        public readonly Output<bool> B;
        public readonly Output<bool> C;
        public readonly Output<bool> D;
        public readonly Output<bool> E;
        public readonly Output<bool> F;
        public readonly Output<bool> G;
        public readonly Output<bool> H;

        public ByteInput() : base($"ByteInput{count++}")
        {
            A = new Output<bool>(this, "A");
            B = new Output<bool>(this, "B");
            C = new Output<bool>(this, "C");
            D = new Output<bool>(this, "D");
            E = new Output<bool>(this, "E");
            F = new Output<bool>(this, "F");
            G = new Output<bool>(this, "G");
            H = new Output<bool>(this, "H");

            Value = 0;
        }

        public List<Output<bool>> GetOutputs() => new List<Output<bool>>()
        {
            A, B, C, D, E, F, G, H
        };

        public override void Detach()
        {
            GetOutputs().ForEach(output => output.Detach());
        }

        public override void Compute() { }

        public override void Set()
        {
            A.Value = (_val & 1) == 1;
            B.Value = (_val & (1 << 1)) == (1 << 1);
            C.Value = (_val & (1 << 2)) == (1 << 2);
            D.Value = (_val & (1 << 3)) == (1 << 3);
            E.Value = (_val & (1 << 4)) == (1 << 4);
            F.Value = (_val & (1 << 5)) == (1 << 5);
            G.Value = (_val & (1 << 6)) == (1 << 6);
            H.Value = (_val & (1 << 7)) == (1 << 7);
        }
    }
}
