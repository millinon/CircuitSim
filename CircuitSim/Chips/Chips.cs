using System;
using System.Collections;
using System.Collections.Generic;

namespace CircuitSim.Chips
{
    // base class for N inputs to one output chips
    //   deriving classes only need to implement Compute and a constructor
    public abstract class NToOne<T> : Component
    {
        private InputSet<T> _inputs;
        public InputSet<T> Inputs
        {
            get
            {
                return _inputs;
            }

            set
            {
                if (_inputs != null)
                {
                    for (int i = 0; i < _inputs.Length; i++)
                    {
                        if (_inputs[i] != null)
                        {
                            _inputs[i].Detach();
                        }
                    }
                }
                _inputs = value;
                _numInputs = value.Length;
                for (int i = 0; i < value.Length; i++)
                {
                    _inputs[i] = new Input<T>(this, $"Input{i}");
                }
            }
        }

        private int _numInputs;
        public int NumInputs
        {
            get
            {
                return _numInputs;
            }
        }

        public readonly Output<T> Out;
        protected T _out;

        public NToOne(int numInputs, string ComponentName) : base(ComponentName)
        {
            Inputs = new InputSet<T>(numInputs);

            Out = new Output<T>(this, "Out");
        }

        public override void Set()
        {
            Out.Value = _out;
        }

        public List<Input<T>> GetInputs() => new List<Input<T>>(Inputs);

        public List<Output<T>> GetOutputs() => new List<Output<T>>()
        {
            Out
        };

        public override void Detach()
        {
            GetInputs().ForEach(input => input.Detach());
            GetOutputs().ForEach(output => output.Detach());
        }
    }

    // base class for chips that take one T as input and output one bool
    public abstract class Predicate<T> : Component
    {
        public readonly Inputs<T> Inputs;

        public readonly Outputs<bool> Outputs;
        private bool _out;

        private Func<T, bool> _func;

        public Predicate(string ComponentName, Func<T, bool> Function) : base(ComponentName)
        {
            if(Function == null) throw new ArgumentException("Predicate requires a function, but one was not provided");
            _func = Function;

            Inputs = new Inputs<T>(this);

            Outputs = new Outputs<bool>(this, new string[] { "Out" });
        }

        public override void Compute()
        {
            try {
                _out = _func(Inputs.A.Value);
            } catch(Exception) {
                HasError = true;
            }
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            Inputs.Detach();
            Outputs.Detach();
        }
    }

    // base class for chips that compare two Ts and output a boolean
    public abstract class BinaryComparator<T> : Component
    {
        public readonly Inputs<T, T> Inputs;

        public readonly Outputs<bool> Outputs;
        private bool _out;

        private Func<T, T, bool> _func;

        public BinaryComparator(string ComponentName, Func<T, T, bool> Function) : base(ComponentName)
        {
            if(Function == null) throw new ArgumentException("BinaryComparator requires a function, but one was not provided");
            _func = Function;

            Inputs = new Inputs<T, T>(this);

            Outputs = new Outputs<bool>(this);
        }

        public override void Compute()
        {
            try {
                _out = _func(Inputs.A.Value, Inputs.B.Value);
            } catch(Exception) {
                HasError = true;
            }
        }

        public override void Detach()
        {
            Inputs.Detach();
            Outputs.Detach();
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }
    }

    // base class for chips that take one T as input and output one T
    public abstract class UnaryFunctor<T, U> : Component
    {
        private Func<T, U> _func;

        public readonly Inputs<T> Inputs;

        public readonly Outputs<U> Outputs;
        protected U _out;

        public UnaryFunctor(string ComponentName, Func<T, U> Function) : base(ComponentName)
        {
            if(Function == null) throw new ArgumentException("UnaryFunctor requires a function, but one was not provided");
            _func = Function;

            Inputs = new Inputs<T>(this);

            Outputs = new Outputs<U>(this);
        }

        public override void Compute()
        {
            try {
            
                _out = _func(Inputs.A.Value);
            } catch(Exception) {
                HasError = true;
            }
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }
    }

    // base class for chips that take one T and one U as input and return one V
    public abstract class BinaryFunctor<T, U, V> : Component
    {
        private Func<T, U, V> _func;

        public readonly Inputs<T, U> Inputs;

        public readonly Outputs<V> Outputs;
        private V _out;

        public BinaryFunctor(string ComponentName, Func<T, U, V> Function) : base(ComponentName)
        {
            if(Function == null) throw new ArgumentException("BinaryFunctor requires a function, but one was not provided");
            _func = Function;

            Inputs = new Inputs<T, U>(this);

            Outputs = new Outputs<V>(this);
        }

        public override void Compute()
        {
            try {
                _out = _func(Inputs.A.Value, Inputs.B.Value);
            } catch(Exception) {
                HasError = true;
            }
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }
    }

    public abstract class Random<T> : Component
    {
        protected Random r;

        public readonly Outputs<T> Outputs;
        protected T _out;

        public Random(string ComponentName, int seed = -1) : base(ComponentName)
        {
            if (seed == -1) r = new Random();
            else r = new Random(seed);

            Outputs = new Outputs<T>(this);
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

    public abstract class Accumulator<T> : Component
    {
        protected T state;
        protected T increment;

        public readonly Outputs<T> Outputs;
        protected T _out;

        public Accumulator(string ComponentName, T increment, T start = default(T)) : base(ComponentName)
        {
            this.increment = increment;
            state = start;

            Outputs = new Outputs<T>(this);
        }

        abstract public void Increment();

        public override void Compute()
        {
            _out = state;
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

    public abstract class Counter<T> : Accumulator<T>
    {
        protected T decrement;

        public Counter(string ComponentName, T increment, T decrement, T start = default(T)) : base(ComponentName, increment, start)
        {
            this.decrement = decrement;
        }

        abstract public void Decrement();
    }

    public class Buffer<T> : Component
    {
        public static uint count = 0;

        public readonly Inputs<T> Inputs;

        public readonly Outputs<T> Outputs;
        protected T val;

        public Buffer() : base($"Buffer{count++}")
        {
            Inputs = new Inputs<T>(this);
            Outputs = new Outputs<T>(this);
        }

        public override void Compute()
        {
            val = Inputs.A.Value;
        }

        public override void Set()
        {
            Outputs.Out.Value = val;
        }

        public override void Detach()
        {
            Inputs.Detach();
            Outputs.Detach();
        }
    }

    public abstract class Multiplexer<T> : Component
    {
        private InputSet<T> _inputs;
        public InputSet<T> Inputs
        {
            get
            {
                return _inputs;
            }

            private set
            {
                if (_inputs != null)
                {
                    for (int i = 0; i < _inputs.Length; i++)
                    {
                        if (_inputs[i] != null)
                        {
                            _inputs[i].Detach();
                        }
                    }
                }
                _inputs = value;
                for (int i = 0; i < value.Length; i++)
                {
                    _inputs[i] = new Input<T>(this, $"Input{i}");
                }
            }
        }

        private InputSet<bool> _select;
        public InputSet<bool> Select
        {
            get
            {
                return _select;
            }

            private set
            {
                if (_select != null)
                {
                    for (int i = 0; i < _select.Length; i++)
                    {
                        if (_select[i] != null)
                        {
                            _select[i].Detach();
                        }
                    }
                }
                _select = value;
                for (int i = 0; i < value.Length; i++)
                {
                    _select[i] = new Input<bool>(this, $"Select{i}");
                }
            }
        }

        public readonly Output<T> Out;
        private T _out;

        public Multiplexer(int numInputs, string ComponentName) : base(ComponentName)
        {
            if (numInputs < 2 || numInputs > 256)
            {
                throw new Exception("Multiplexer requires between 2 and 256 inputs");
            }
            else if ((numInputs & (numInputs - 1)) != 0)
            {
                throw new Exception("Multiplexer's number of inputs must be a power of two");
            }

            Inputs = new InputSet<T>(numInputs);

            Select = new InputSet<bool>((int)Math.Log(numInputs, 2));

            Out = new Output<T>(this, "Out");
        }

        public override void Compute()
        {
            int selected_input = 0;
            for (int i = Select.Length - 1; i >= 0; i--)
            {
                selected_input = 2 * selected_input + (Select[i].Value ? 1 : 0);
            }

            _out = Inputs[selected_input].Value;
        }

        public override void Set()
        {
            Out.Value = _out;
        }

        public override void Detach()
        {
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i].Detach();
            }

            Out.Detach();
        }
    }

    public class DigitalMux : Multiplexer<bool>
    {
        private static uint count = 0;

        public DigitalMux(int numInputs) : base(numInputs, $"DigtialMux{count++}") { }
    }

    public class IntMux : Multiplexer<int>
    {
        private static uint count = 0;

        public IntMux(int numInputs) : base(numInputs, $"IntMux{count++}") { }
    }

    public class FloatMux : Multiplexer<double>
    {
        private static uint count = 0;

        public FloatMux(int numInputs) : base(numInputs, $"FltMux{count++}") { }
    }

    public class StringMux : Multiplexer<string>
    {
        private static uint count = 0;

        public StringMux(int numInputs) : base(numInputs, $"StringMux{count++}") { }
    }

    public class Equals<T> : BinaryComparator<T>
    {
        public Equals() : base("Equals", (a, b) => a.Equals(b)) { }
    }

    public class HalfAdder : Component
    {
        private static uint count = 0;

        public readonly Inputs<bool, bool> Inputs;

        public class HAOutputs : Outputs
        {
            public readonly Output<bool> S;
            public readonly Output<bool> C;

            public HAOutputs(Component component) : base(2)
            {
                S = new Output<bool>(component, "S");
                C = new Output<bool>(component, "C");
            }

            public override void Detach()
            {
                S.Detach();
                C.Detach();
            }
        }

        public readonly HAOutputs Outputs;

        private Digital.XOR xor = new Digital.XOR();
        private Digital.AND and = new Digital.AND();

        public HalfAdder() : base($"HalfAdder{count++}")
        {
            Inputs = new Inputs<bool, bool>(this);

            Outputs = new HAOutputs(this);
        }

        public override void Compute()
        {
            xor.Inputs[0] = Inputs.A;
            xor.Inputs[1] = Inputs.B;

            and.Inputs[0] = Inputs.A;
            and.Inputs[1] = Inputs.B;

            and.Tick();
            xor.Tick();
        }

        public override void Set()
        {
            Outputs.S.Value = xor.Out.Value;
            Outputs.C.Value = and.Out.Value;
        }

        public override void Detach()
        {
            Inputs.Detach();
            Outputs.Detach();
        }
    }

    public class FullAdder : Component
    {
        private static uint count = 0;

        public class FAInputs : Inputs
        {
            private Input<bool> _a;
            public Input<bool> A
            {
                get
                {
                    return _a;
                }

                set
                {
                    if (value != _a)
                    {
                        if (_a != null) _a.Detach();
                        _a = value;
                    }
                }
            }

            private Input<bool> _b;
            public Input<bool> B
            {
                get
                {
                    return _b;
                }

                set
                {
                    if (value != _b)
                    {
                        if (_b != null) _b.Detach();
                        _b = value;
                    }
                }
            }

            private Input<bool> _cin;
            public Input<bool> Cin
            {
                get
                {
                    return _cin;
                }

                set
                {
                    if (value != _cin)
                    {
                        if (_cin != null) _cin.Detach();
                        _cin = value;
                    }
                }
            }

            public FAInputs(Component component) : base(3)
            {
                A = new Input<bool>(component, "A");
                B = new Input<bool>(component, "B");
                Cin = new Input<bool>(component, "Cin");
            }

            public override void Detach()
            {
                A.Detach();
                B.Detach();
            }
        }

        public readonly FAInputs Inputs;

        public class FAOutputs : Outputs
        {
            public readonly Output<bool> S;
            public readonly Output<bool> Cout;

            public FAOutputs(Component component) : base(2)
            {
                S = new Output<bool>(component, "S");
                Cout = new Output<bool>(component, "C");
            }

            public override void Detach()
            {
                S.Detach();
                Cout.Detach();
            }
        }

        public readonly FAOutputs Outputs;

        private Digital.XOR xor1 = new Digital.XOR();
        private Digital.XOR xor2 = new Digital.XOR();

        private Digital.AND and1 = new Digital.AND();
        private Digital.AND and2 = new Digital.AND();

        private Digital.OR or = new Digital.OR();

        public FullAdder() : base($"FullAdder{count++}")
        {
            Inputs = new FAInputs(this);

            Outputs = new FAOutputs(this);

            xor2.Inputs[0].Source = xor1.Out;
            and2.Inputs[0].Source = xor1.Out;
            or.Inputs[0].Source = and1.Out;
            or.Inputs[1].Source = and2.Out;
        }

        public override void Compute()
        {
            xor1.Inputs[0] = Inputs.A;
            and1.Inputs[0] = Inputs.A;
            xor1.Inputs[1] = Inputs.B;
            and1.Inputs[1] = Inputs.B;
            xor2.Inputs[1] = Inputs.Cin;
            and2.Inputs[1] = Inputs.Cin;

            xor1.Tick();
            xor2.Tick();
            and1.Tick();
            and2.Tick();
            or.Tick();
        }

        public override void Set()
        {
            Outputs.S.Value = xor2.Out.Value;
            Outputs.Cout.Value = or.Out.Value;
        }

        public override void Detach()
        {
            Inputs.Detach();

            Outputs.Detach();
        }
    }

    public class ByteAdder : Component
    {
        private static uint count = 0;

        private InputSet<bool> _a;
        public InputSet<bool> A
        {
            get
            {
                return _a;
            }

            set
            {
                if (_a != null)
                {
                    for (int i = 0; i < _a.Length; i++)
                    {
                        if (_a[i] != null)
                        {
                            _a[i].Detach();
                        }
                    }
                }
                _a = value;
                for (int i = 0; i < value.Length; i++)
                {
                    _a[i] = new Input<bool>(this, $"A{i}");
                }
            }
        }

        private InputSet<bool> _b;
        public InputSet<bool> B
        {
            get
            {
                return _b;
            }

            private set
            {
                if (_b != null)
                {
                    for (int i = 0; i < _b.Length; i++)
                    {
                        if (_b[i] != null)
                        {
                            _b[i].Detach();
                        }
                    }
                }
                _b = value;
                for (int i = 0; i < value.Length; i++)
                {
                    _b[i] = new Input<bool>(this, $"B{i}");
                }
            }
        }

        private Input<bool> _carry;
        public Input<bool> Carry
        {
            get
            {
                return _carry;
            }

            set
            {
                if (_carry != null && _carry != value) Carry.Detach();
                _carry = value;
            }
        }

        public readonly Output<bool>[] Out;

        public readonly Output<bool> Overflow;

        private Input.Constant<bool> carry_const;

        private FullAdder[] fa = new FullAdder[]
        {
            new FullAdder(),
                new FullAdder(),
                new FullAdder(),
                new FullAdder(),
                new FullAdder(),
                new FullAdder(),
                new FullAdder(),
                new FullAdder()
        };

        public ByteAdder() : base($"ByteAdder{count++}")
        {
            A = new InputSet<bool>(8);
            B = new InputSet<bool>(8);

            Carry = new Input<bool>(this, "Carry");

            Out = new Output<bool>[8];
            for (int i = 0; i < 8; i++)
            {
                Out[i] = new Output<bool>(this, $"Out{i}");
            }

            Overflow = new Output<bool>(this, "Overflow");

            carry_const = new Input.Constant<bool>(false);
            Carry.Source = carry_const.Outputs[0];
        }

        public override void Compute()
        {
            fa[0].Inputs.A = A[0];
            fa[0].Inputs.B = B[0];
            fa[0].Inputs.Cin = Carry;

            for (int i = 1; i < 8; i++)
            {
                fa[i].Inputs.A = A[i];
                fa[i].Inputs.B = B[i];
                fa[i].Inputs.Cin.Source = fa[i - 1].Outputs.Cout;
            }

            for (int i = 0; i < 8; i++)
            {
                fa[i].Tick();
            }
        }

        public override void Set()
        {
            for (int i = 0; i < 8; i++)
            {
                Out[i].Value = fa[i].Outputs.S.Value;
            }

            Overflow.Value = fa[7].Outputs.Cout.Value;
        }

        public override void Detach()
        {
            for (int i = 0; i < 8; i++)
            {
                A[i].Detach();
                B[i].Detach();
                Out[i].Detach();
            }
        }
    }
}
