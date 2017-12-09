//Copyright 2017 Phillip Goldfarb
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CircuitSim
{
    using DigitalInput = Input<bool>;
    using DigitalOutput = Output<bool>;
    using DigitalInputSet = InputSet<bool>;

    using AnalogInput = Input<double>;
    using AnalogOutput = Output<double>;
    using AnalogInputSet = InputSet<double>;

    using IntegerInput = Input<int>;
    using IntegerOutput = Output<int>;
    using IntegerInputSet = InputSet<int>;

    using StringInput = Input<string>;
    using StringOutput = Output<string>;
    using StringInputSet = InputSet<string>;

    public abstract class Component
    {
        private bool _visited = false;

        // WARNING: this is a hack to make recursive chips work. You generally should not ever set this to true.
        public bool AllowRecursion = false;

        public readonly string Name;

        public Component(string ComponentName)
        {
            Name = ComponentName;
        }

        // compute the internal state of the component
        public abstract void Compute();

        // update the external state of the component
        public abstract void Set();

        // disconnect the inputs and outputs of the component
        public abstract void Detach();

        public void Tick()
        {
            if (_visited && !AllowRecursion) throw new Exception($"recursive Tick() detected on component {Name}");

            var visited = _visited;

            _visited = true;
            Compute();
            Set();
            _visited = visited;
        }
    }

    public class Input<T>
    {
        public readonly Component Component;
        private readonly string Name;

        public object Lock;

        private Output<T> _source = null;
        public Output<T> Source
        {
            get { return _source; }
            set
            {
                lock (Lock)
                {
                    if (value != _source)
                    {
                        if (_source != null) _source.Sinks.Remove(this); _source = value; if (_source != null) _source.Sinks.Add(this);
                    }
                }
            }
        }

        public T Value
        {
            get
            {
                if (Source != null) return Source.Value;
                else throw new Exception($"input {Name} of component {Component.Name} is not connected to any output");
            }
        }

        public Input(Component Component, string Name)
        {
            this.Component = Component;
            this.Name = Name;
            Lock = new Object();
        }

        public void Detach() => Source = null;

        public bool Attached
        {
            get
            {
                return Source == null;
            }
        }
    }

    public abstract class Inputs
    {
        public readonly uint Count;

        public Inputs(uint count)
        {
            this.Count = count;
        }

        public abstract void Detach();
    }
    
    public class Inputs<T> : Inputs
    {
        private Input<T> _a;
        public Input<T> A
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

        public Inputs(Component component, string[] names = null) : base(1)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument length must match input count");

                A = new Input<T>(component, names[0]);
            }
            else
            {
                A = new Input<T>(component, "A");
            }
        }

        public override void Detach()
        {
            A.Detach();
        }
    }

    public class Inputs<T, U> : Inputs
    {
        private Input<T> _a;
        public Input<T> A
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

        private Input<U> _b;
        public Input<U> B
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

        public Inputs(Component component, string[] names = null) : base(2)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument length must match input count");

                A = new Input<T>(component, names[0]);
                B = new Input<U>(component, names[1]);
            }
            else
            {
                A = new Input<T>(component, "A");
                B = new Input<U>(component, "B");
            }
        }

        public override void Detach()
        {
            A.Detach();
            B.Detach();
        }
    }

    public class Inputs<T, U, V> : Inputs
    {
        private Input<T> _a;
        public Input<T> A
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

        private Input<U> _b;
        public Input<U> B
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

        private Input<V> _c;
        public Input<V> C
        {
            get
            {
                return _c;
            }

            set
            {
                if (value != _c)
                {
                    if (_c != null) _c.Detach();
                    _c = value;
                }
            }
        }

        public Inputs(Component component, string[] names = null) : base(3)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument length must match input count");

                A = new Input<T>(component, names[0]);
                B = new Input<U>(component, names[1]);
                C = new Input<V>(component, names[2]);
            }
            else
            {
                A = new Input<T>(component, "A");
                B = new Input<U>(component, "B");
                C = new Input<V>(component, "C");
            }
        }

        public override void Detach()
        {
            A.Detach();
            B.Detach();
            C.Detach();
        }
    }

    public class Inputs<T, U, V, W> : Inputs
    {
        private Input<T> _a;
        public Input<T> A
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

        private Input<U> _b;
        public Input<U> B
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

        private Input<V> _c;
        public Input<V> C
        {
            get
            {
                return _c;
            }

            set
            {
                if (value != _c)
                {
                    if (_c != null) _c.Detach();
                    _c = value;
                }
            }
        }

        private Input<W> _d;
        public Input<W> D
        {
            get
            {
                return _d;
            }

            set
            {
                if (value != _d)
                {
                    if (_d != null) _d.Detach();
                    _d = value;
                }
            }
        }

        public Inputs(Component component, string[] names = null) : base(4)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument length must match input count");

                A = new Input<T>(component, names[0]);
                B = new Input<U>(component, names[1]);
                C = new Input<V>(component, names[2]);
                D = new Input<W>(component, names[3]);
            }
            else
            {
                A = new Input<T>(component, "A");
                B = new Input<U>(component, "B");
                C = new Input<V>(component, "C");
                D = new Input<W>(component, "D");
            }
        }

        public override void Detach()
        {
            A.Detach();
            B.Detach();
            C.Detach();
            D.Detach();
        }
    }

    public class Inputs<S, T, U, V, W, X, Y, Z> : Inputs
    {
        private Input<S> _a;
        public Input<S> A
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

        private Input<T> _b;
        public Input<T> B
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

        private Input<U> _c;
        public Input<U> C
        {
            get
            {
                return _c;
            }

            set
            {
                if (value != _c)
                {
                    if (_c != null) _c.Detach();
                    _c = value;
                }
            }
        }

        private Input<V> _d;
        public Input<V> D
        {
            get
            {
                return _d;
            }

            set
            {
                if (value != _d)
                {
                    if (_d != null) _d.Detach();
                    _d = value;
                }
            }
        }

        private Input<W> _e;
        public Input<W> E
        {
            get
            {
                return _e;
            }

            set
            {
                if (value != _e)
                {
                    if (_e != null) _e.Detach();
                    _e = value;
                }
            }
        }

        private Input<X> _f;
        public Input<X> F
        {
            get
            {
                return _f;
            }

            set
            {
                if (value != _f)
                {
                    if (_f != null) _f.Detach();
                    _f = value;
                }
            }
        }

        private Input<Y> _g;
        public Input<Y> G
        {
            get
            {
                return _g;
            }

            set
            {
                if (value != _g)
                {
                    if (_g != null) _g.Detach();
                    _g = value;
                }
            }
        }

        private Input<Z> _h;
        public Input<Z> H
        {
            get
            {
                return _h;
            }

            set
            {
                if (value != _h)
                {
                    if (_h != null) _h.Detach();
                    _h = value;
                }
            }
        }

        public Inputs(Component component, string[] names = null) : base(8)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument length must match input count");

                A = new Input<S>(component, names[0]);
                B = new Input<T>(component, names[1]);
                C = new Input<U>(component, names[2]);
                D = new Input<V>(component, names[3]);
                E = new Input<W>(component, names[4]);
                F = new Input<X>(component, names[5]);
                G = new Input<Y>(component, names[6]);
                H = new Input<Z>(component, names[7]);
            }
            else
            {
                A = new Input<S>(component, "A");
                B = new Input<T>(component, "B");
                C = new Input<U>(component, "C");
                D = new Input<V>(component, "D");
                E = new Input<W>(component, "E");
                F = new Input<X>(component, "F");
                G = new Input<Y>(component, "G");
                H = new Input<Z>(component, "H");
            }
        }

        public override void Detach()
        {
            A.Detach();
            B.Detach();
            C.Detach();
            D.Detach();
            E.Detach();
            F.Detach();
            G.Detach();
            H.Detach();
        }
    }

    public class InputSet<T> : IEnumerable<Input<T>>
    {
        private Input<T>[] _inputs;

        public InputSet(int capacity)
        {
            _inputs = new Input<T>[capacity];
        }

        public int Length
        {
            get
            {
                return _inputs.Length;
            }
        }

        public Input<T> this[int i]
        {
            get
            {
                return _inputs[i];
            }
            set
            {
                if (_inputs[i] != null && _inputs[i] != value) _inputs[i].Detach();
                _inputs[i] = value;
            }
        }

        public IEnumerator<Input<T>> GetEnumerator()
        {
            return ((IEnumerable<Input<T>>)_inputs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Input<T>>)_inputs).GetEnumerator();
        }
    }


    public class Output<T>
    {
        public readonly Component Component;
        public readonly string Name;

        public List<Input<T>> Sinks = new List<Input<T>>();

        // this is the key to CircuitSim - AutoTick makes each component recursively update
        //   other components that are attached to its outputs
        public bool AutoTick;

        public bool AlwaysUpdate = true;

        private T _value;
        private T _lastValue;
        private bool _first = true;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (AutoTick && (_first || (!EqualityComparer<T>.Default.Equals(value, _lastValue) || AlwaysUpdate)))
                {
                    Sinks.ForEach(sink =>
                    {
                        lock (sink.Lock)
                        {
                            sink.Component.Tick();
                        }
                    });
                    _first = false;
                }
                _lastValue = value;
            }
        }

        public Output(Component Component, string Name, bool autotick = true)
        {
            this.Component = Component;
            this.Name = Name;
            AutoTick = autotick;
        }

        public void Detach()
        {
            var sinks = new List<Input<T>>(Sinks);
            sinks.ForEach(s => s.Detach());
        }
    }

    public abstract class Outputs
    {
        public readonly uint Count;

        public Outputs(uint count)
        {
            this.Count = count;
        }

        public abstract void Detach();
    }

    public class Outputs<T> : Outputs
    {
        public readonly Output<T> Out;

        public Outputs(Component component, string[] names = null) : base(1)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument must match the number of inputs");

                Out = new Output<T>(component, names[0]);
            }
            else
            {
                Out = new Output<T>(component, "Out1");
            }
        }

        public override void Detach()
        {
            Out.Detach();
        }
    }

    public class Outputs<T, U> : Outputs
    {
        public readonly Output<T> Out1;
        public readonly Output<U> Out2;

        public Outputs(Component component, string[] names = null) : base(2)
        {
            if (names != null)
            {
                if (names.Length != Count) throw new Exception("names argument must match the number of inputs");

                Out1 = new Output<T>(component, names[0]);
                Out2 = new Output<U>(component, names[1]);
            }
            else
            {
                Out1 = new Output<T>(component, "Out1");
                Out2 = new Output<U>(component, "Out2");
            }
        }

        public override void Detach()
        {
            Out1.Detach();
            Out2.Detach();
        }
    }

    namespace Chips
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
                _func = Function ?? throw new ArgumentException("Predicate requires a function, but one was not provided");

                Inputs = new Inputs<T>(this);

                Outputs = new Outputs<bool>(this, new string[] { "Out" });
            }

            public override void Compute()
            {
                _out = _func(Inputs.A.Value);
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
                _func = Function ?? throw new ArgumentException("BinaryComparator requires a function, but one was not provided");

                Inputs = new Inputs<T, T>(this);

                Outputs = new Outputs<bool>(this);
            }

            public override void Compute()
            {
                _out = _func(Inputs.A.Value, Inputs.B.Value);
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
                _func = Function ?? throw new ArgumentException("UnaryFunctor requires a function, but one was not provided");

                Inputs = new Inputs<T>(this);

                Outputs = new Outputs<U>(this);
            }

            public override void Compute()
            {
                _out = _func(Inputs.A.Value);
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
                _func = Function ?? throw new ArgumentException("UnaryFunctor requires a function, but one was not provided");

                Inputs = new Inputs<T, U>(this);

                Outputs = new Outputs<V>(this);
            }

            public override void Compute()
            {
                _out = _func(Inputs.A.Value, Inputs.B.Value);
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

            private DigitalInputSet _select;
            public DigitalInputSet Select
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
                        _select[i] = new DigitalInput(this, $"Select{i}");
                    }
                }
            }

            public readonly Output<T> Out;
            private T _out;

            public Multiplexer(int numInputs, string ComponentName) : base(ComponentName)
            {
                if (numInputs < 2 || numInputs > 256)
                {
                    throw new Exception("Multiplexer requires between 2 and 255 inputs");
                }
                else if ((numInputs & (numInputs - 1)) != 0)
                {
                    throw new Exception("Multiplexer's number of inputs must be a power of two");
                }

                Inputs = new InputSet<T>(numInputs);

                Select = new DigitalInputSet((int)Math.Log(numInputs, 2));

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

            public List<Output<T>> GetOutputs() => new List<Output<T>>() { Out };
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

        namespace Input
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

                public readonly DigitalOutput A;
                public readonly DigitalOutput B;
                public readonly DigitalOutput C;
                public readonly DigitalOutput D;
                public readonly DigitalOutput E;
                public readonly DigitalOutput F;
                public readonly DigitalOutput G;
                public readonly DigitalOutput H;

                public ByteInput() : base($"ByteInput{count++}")
                {
                    A = new DigitalOutput(this, "A");
                    B = new DigitalOutput(this, "B");
                    C = new DigitalOutput(this, "C");
                    D = new DigitalOutput(this, "D");
                    E = new DigitalOutput(this, "E");
                    F = new DigitalOutput(this, "F");
                    G = new DigitalOutput(this, "G");
                    H = new DigitalOutput(this, "H");

                    Value = 0;
                }

                public List<DigitalOutput> GetOutputs() => new List<DigitalOutput>()
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
                    A.Value = (_val & 0b00000001) == 0b00000001;
                    B.Value = (_val & 0b00000010) == 0b00000010;
                    C.Value = (_val & 0b00000100) == 0b00000100;
                    D.Value = (_val & 0b00001000) == 0b00001000;
                    E.Value = (_val & 0b00010000) == 0b00010000;
                    F.Value = (_val & 0b00100000) == 0b00100000;
                    G.Value = (_val & 0b01000000) == 0b01000000;
                    H.Value = (_val & 0b10000000) == 0b10000000;
                }
            }
        }

        namespace Digital
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

        namespace Signal
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

                private DigitalInput _a;
                public DigitalInput A
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

                public readonly DigitalOutput Out;
                private bool _out;

                public Hold(TimeSpan span) : base($"Hold{count++}")
                {
                    Out = new DigitalOutput(this, "Out");

                    type = Type.Time;
                    duration = span;
                }

                public Hold(ulong cycles) : base($"Hold{count++}")
                {
                    Out = new DigitalOutput(this, "Out");

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

                public List<DigitalInput> GetInputs() => new List<DigitalInput>() { A };

                public List<DigitalOutput> GetOutputs() => new List<DigitalOutput>() { Out };
            }
        }

        namespace String
        {
            public class EQ : BinaryComparator<string>
            {
                private static uint count = 0;

                public EQ() : base($"StrEQ{count++}", (a, b) => a == b) { }
            }

            public class NEQ : BinaryComparator<string>
            {
                private static uint count = 0;

                public NEQ() : base($"StrNEQ{count++}", (a, b) => a != b) { }
            }

            public class Empty : Predicate<string>
            {
                private static uint count = 0;

                public Empty() : base($"Empty{count++}", string.IsNullOrEmpty) { }
            }

            public class Concat : BinaryFunctor<string, string, string>
            {
                private static uint count = 0;

                public Concat() : base($"Concat{count++}", string.Concat) { }
            }

            public class ToLower : UnaryFunctor<string, string>
            {
                private static uint count = 0;

                public ToLower() : base($"ToLower{count++}", (s) => s.ToLower()) { }
            }

            public class ToUpper : UnaryFunctor<string, string>
            {
                private static uint count = 0;

                public ToUpper() : base($"ToUpper{count++}", (s) => s.ToUpper()) { }
            }

            public class StartsWith : BinaryComparator<string>
            {
                private static uint count = 0;

                public StartsWith() : base($"StartsWith{count++}", (a, b) => a.StartsWith(b)) { }
            }

            public class EndsWith : BinaryComparator<string>
            {
                private static uint count = 0;

                public EndsWith() : base($"EndsWith{count++}", (a, b) => a.EndsWith(b)) { }
            }

            public class Accumulator : Accumulator<string>
            {
                private static uint count = 0;

                public Accumulator(string increment, string start = "") : base($"StrAccumulator{count++}", increment, start) { }

                public override void Increment()
                {
                    state += increment;
                    Tick();
                }
            }
        }

        namespace Integer
        {
            public class Random : Random<int>
            {
                private static uint count = 0;

                public Random(int seed = -1) : base($"IntRandom{count++}", seed) { }

                public override void Compute()
                {
                    _out = r.Next();
                }
            }

            public class RisingEdge : Signal.EdgeDetector<int>
            {
                private static int count = 0;

                public RisingEdge() : base($"IntRisingEdge{count++}") { }

                public override void Compute()
                {
                    edge = Inputs.A.Value > last;
                    last = Inputs.A.Value;
                }
            }

            public class FallingEdge : Signal.EdgeDetector<int>
            {
                private static int count = 0;

                public FallingEdge() : base($"IntFallingEdge{count++}") { }

                public override void Compute()
                {
                    edge = Inputs.A.Value < last;
                    last = Inputs.A.Value;
                }
            }

            public class Accumulator : Accumulator<int>
            {
                private static uint count = 0;

                public Accumulator(int start = 0, int increment = 1) : base($"IntAccumulator{count++}", increment, start) { }

                public override void Increment()
                {
                    state += increment;
                    Tick();
                }
            }

            public class Counter : Counter<int>
            {
                private static uint count = 0;

                public Counter(int start = 0, int increment = 1, int decrement = 1) : base($"IntCounter{count++}", increment, decrement, start) { }

                public override void Increment()
                {
                    state += increment;
                    Tick();
                }

                public override void Decrement()
                {
                    state -= decrement;
                    Tick();
                }
            }

            public class IntSum : Component
            {
                private static uint count = 0;

                public Inputs<List<int>> Inputs;

                public readonly Outputs<int> Outputs;
                private int _out;

                private List.Reduce<int, int> reducer = new List.Reduce<int, int>("sum");
                private Arithmetic.Add adder = new Arithmetic.Add();
                private Input.Constant<int> zero = new Chips.Input.Constant<int>(0);

                public IntSum() : base($"IntSum{count++}")
                {
                    Inputs = new Inputs<List<int>>(this);

                    Outputs = new Outputs<int>(this);

                    zero.Tick();
                }

                public override void Compute()
                {
                    reducer.Inputs.A = Inputs.A;
                    reducer.Inputs.Func.Source = adder.Out;
                    reducer.Inputs.StartingValue.Source = zero.Outputs[0];
                    adder.Inputs[0].Source = reducer.Outputs.CurrentVal;
                    adder.Inputs[1].Source = reducer.Outputs.FuncInput;

                    reducer.Tick();
                    _out = reducer.Outputs.Out.Value;
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

            namespace Comparison
            {
                public class EQ : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public EQ() : base($"IntEQ{count++}", (a, b) => a == b) { }
                }

                public class GT : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public GT() : base($"IntGT{count++}", (a, b) => a > b) { }
                }

                public class GTE : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public GTE() : base($"IntGTE{count++}", (a, b) => a >= b) { }
                }

                public class LT : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public LT() : base($"IntLT{count++}", (a, b) => a < b) { }
                }

                public class LTE : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public LTE() : base($"IntGTE{count++}", (a, b) => a >= b) { }
                }

                public class NEQ : BinaryComparator<int>
                {
                    private static uint count = 0;

                    public NEQ() : base($"IntNEQ{count++}", (a, b) => a != b) { }
                }
            }

            namespace Arithmetic
            {
                public class Inv : UnaryFunctor<int, int>
                {
                    private static uint count = 0;

                    public Inv() : base($"IntInv{count++}", a => 1 / a) { }
                }

                public class Abs : UnaryFunctor<int, int>
                {
                    private static uint count = 0;

                    public Abs() : base($"IntAbs{count++}", Math.Abs) { }
                }

                public class Neg : UnaryFunctor<int, int>
                {
                    private static uint count = 0;

                    public Neg() : base($"IntNeg{count++}", a => -a) { }
                }

                public class Sign : UnaryFunctor<int, int>
                {
                    private static uint count = 0;

                    public Sign() : base($"IntSign{count++}", a => Math.Sign(a)) { }
                }

                public class Add : NToOne<int>
                {
                    private static uint count = 0;

                    public Add(int numInputs = 2) : base(numInputs, $"IntAdd{count++}") { }

                    public override void Compute()
                    {
                        _out = 0;
                        for (int i = 0; i < NumInputs; i++)
                        {
                            _out += Inputs[i].Value;
                        }
                    }
                }

                public class Sub : BinaryFunctor<int, int, int>
                {
                    private static uint count = 0;

                    public Sub() : base($"IntSub{count++}", (a, b) => a - b) { }
                }

                public class Mul : NToOne<int>
                {
                    private static uint count = 0;

                    public Mul(int numInputs = 2) : base(numInputs, $"IntMul{count++}") { }

                    public override void Compute()
                    {
                        _out = 1;
                        for (int i = 0; i < NumInputs; i++)
                        {
                            _out *= Inputs[i].Value;
                        }
                    }
                }

                public class Div : BinaryFunctor<int, int, int>
                {
                    private static uint count = 0;

                    public Div() : base($"IntDiv{count++}", (a, b) => a / b) { }
                }

                public class Mod : BinaryFunctor<int, int, int>
                {
                    private static uint count = 0;

                    public Mod() : base($"IntMod{count++}", (a, b) => a % b) { }
                }

                public class Min : BinaryFunctor<int, int, int>
                {
                    private static uint count = 0;

                    public Min() : base($"IntMin{count++}", Math.Min) { }
                }

                public class Max : BinaryFunctor<int, int, int>
                {
                    private static uint count = 0;

                    public Max() : base($"IntMax{count++}", Math.Max) { }
                }
            }

            public class IsEven : Predicate<int>
            {
                private static uint count = 0;

                public IsEven() : base($"IsEven{count++}", a => a % 2 == 0) { }
            }

            public class IsOdd : Predicate<int>
            {
                private static uint count = 0;

                public IsOdd() : base($"IsOdd{count++}", a => a % 2 == 1) { }
            }
        }

        namespace FloatingPoint
        {
            public class Random : Random<double>
            {
                private static uint count = 0;

                public Random(int seed = -1) : base($"FltRandom{count++}", seed) { }

                public override void Compute()
                {
                    _out = r.NextDouble();
                }
            }

            public class RisingEdge : Signal.EdgeDetector<double>
            {
                private static int count = 0;

                public RisingEdge() : base($"FltRisingEdge{count++}") { }

                public override void Compute()
                {
                    edge = Inputs.A.Value > last;
                    last = Inputs.A.Value;
                }
            }

            public class FallingEdge : Signal.EdgeDetector<double>
            {
                private static int count = 0;

                public FallingEdge() : base($"FltFallingEdge{count++}") { }

                public override void Compute()
                {
                    edge = Inputs.A.Value < last;
                    last = Inputs.A.Value;
                }
            }

            public class Accumulator : Accumulator<double>
            {
                private static uint count = 0;

                public Accumulator(double start = 0.0, double increment = 1.0) : base($"FltAccumulator{count++}", increment, start) { }

                public override void Increment()
                {
                    state += increment;
                    Tick();
                }
            }

            public class Counter : Counter<double>
            {
                private static uint count = 0;

                public Counter(double start = 0, double increment = 1, double decrement = 1) : base($"FltCounter{count++}", increment, decrement, start) { }

                public override void Increment()
                {
                    state += increment;
                    Tick();
                }

                public override void Decrement()
                {
                    state -= decrement;
                    Tick();
                }
            }

            namespace Comparison
            {
                public class EQ : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public EQ(double Epsilon = 0.0) : base($"FltEQ{count++}", (a, b) => Math.Abs(a - b) <= Epsilon) { }
                }

                public class GT : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public GT() : base($"FltGT{count++}", (a, b) => a > b) { }
                }

                public class GTE : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public GTE() : base($"FltGTE{count++}", (a, b) => a >= b) { }
                }

                public class LT : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public LT() : base($"FltLT{count++}", (a, b) => a < b) { }
                }

                public class LTE : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public LTE() : base($"FltGTE{count++}", (a, b) => a >= b) { }
                }

                public class NEQ : BinaryComparator<double>
                {
                    private static uint count = 0;

                    public NEQ() : base($"FltNEQ{count++}", (a, b) => a != b) { }
                }
            }

            namespace Arithmetic
            {
                public class Inv : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Inv() : base($"FltInv{count++}", a => 1.0 / a) { }
                }

                public class Sin : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Sin() : base($"Sin{count++}", Math.Sin) { }
                }

                public class Cos : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Cos() : base($"Cos{count++}", Math.Cos) { }
                }

                public class Tan : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Tan() : base($"Tan{count++}", Math.Tan) { }
                }

                public class Sinh : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Sinh() : base($"Sinh{count++}", Math.Sinh) { }
                }

                public class Cosh : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Cosh() : base($"Cosh{count++}", Math.Cosh) { }
                }

                public class Tanh : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Tanh() : base($"Tanh{count++}", Math.Tanh) { }
                }

                public class Asin : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Asin() : base($"Asin{count++}", Math.Asin) { }
                }

                public class Acos : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Acos() : base($"Acos{count++}", Math.Acos) { }
                }

                public class Atan : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Atan() : base($"Atan{count++}", Math.Atan) { }
                }

                public class Exp : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Exp() : base($"Exp{count++}", Math.Exp) { }
                }

                public class Log : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Log() : base($"Exp{count++}", Math.Log) { }
                }

                public class Abs : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Abs() : base($"FltAbs{count++}", Math.Abs) { }
                }

                public class Neg : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Neg() : base($"FltNeg{count++}", a => -a) { }
                }

                public class Ceil : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Ceil() : base($"Ceil{count++}", Math.Ceiling) { }
                }

                public class Floor : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Floor() : base($"Floor{count++}", Math.Floor) { }
                }

                public class Sign : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Sign() : base($"FltSign{count++}", a => Math.Sign(a)) { }
                }

                public class Round : UnaryFunctor<double, double>
                {
                    private static uint count = 0;

                    public Round() : base($"Round{count++}", Math.Round) { }
                }

                public class Add : NToOne<double>
                {
                    private static uint count = 0;

                    public Add(int numInputs = 2) : base(numInputs, $"FltAdd{count++}") { }

                    public override void Compute()
                    {
                        _out = 0.0;
                        for (int i = 0; i < NumInputs; i++)
                        {
                            _out += Inputs[i].Value;
                        }
                    }
                }

                public class Sub : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Sub() : base($"FltSub{count++}", (a, b) => a - b) { }
                }

                public class Mul : NToOne<double>
                {
                    private static uint count = 0;

                    public Mul(int numInputs = 2) : base(numInputs, $"FltMul{count++}") { }

                    public override void Compute()
                    {
                        _out = 1.0;
                        for (int i = 0; i < NumInputs; i++)
                        {
                            _out *= Inputs[i].Value;
                        }
                    }
                }

                public class Div : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Div() : base($"FltDiv{count++}", (a, b) => a / b) { }
                }

                public class Mod : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Mod() : base($"FltMod{count++}", (a, b) => a % b) { }
                }

                public class Pow : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Pow() : base($"Pow{count++}", Math.Pow) { }
                }

                public class Min : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Min() : base($"FltMin{count++}", Math.Min) { }
                }

                public class Max : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public Max() : base($"FltMax{count++}", Math.Max) { }
                }

                public class LogN : BinaryFunctor<double, double, double>
                {
                    private static uint count = 0;

                    public LogN() : base($"LogN{count++}", Math.Log) { }
                }
            }
        }

        namespace Conversion
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
                    S = new DigitalOutput(component, "S");
                    C = new DigitalOutput(component, "C");
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
                    S = new DigitalOutput(component, "S");
                    Cout = new DigitalOutput(component, "C");
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

            private DigitalInputSet _a;
            public DigitalInputSet A
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
                        _a[i] = new DigitalInput(this, $"A{i}");
                    }
                }
            }

            private DigitalInputSet _b;
            public DigitalInputSet B
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
                        _b[i] = new DigitalInput(this, $"B{i}");
                    }
                }
            }

            private DigitalInput _carry;
            public DigitalInput Carry
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

            public readonly DigitalOutput[] Out;

            public readonly DigitalOutput Overflow;

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
                A = new DigitalInputSet(8);
                B = new DigitalInputSet(8);

                Carry = new DigitalInput(this, "Carry");

                Out = new DigitalOutput[8];
                for (int i = 0; i < 8; i++)
                {
                    Out[i] = new DigitalOutput(this, $"Out{i}");
                }

                Overflow = new DigitalOutput(this, "Overflow");

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

        namespace List
        {
            public class Map<T, U> : Component
            {
                public class MapInputs : Inputs
                {
                    private Input<List<T>> _a;
                    public Input<List<T>> A
                    {
                        get
                        {
                            return _a;
                        }

                        set
                        {
                            if (_a != value)
                            {
                                if (_a != null) _a.Detach();
                                _a = value;
                            }
                        }
                    }

                    private Input<U> _func;
                    public Input<U> Func
                    {
                        get
                        {
                            return _func;
                        }

                        set
                        {
                            if(_func != value)
                            {
                                if (_func != null) _func.Detach();
                                _func = value;
                            }
                        }
                    }

                    public MapInputs(Component component) : base(2)
                    {
                        A = new Input<List<T>>(component, "A");
                        Func = new Input<U>(component, "Func");
                    }

                    public override void Detach()
                    {
                        A.Detach();
                        Func.Detach();
                    }
                }

                public readonly MapInputs Inputs;


                public class MapOutputs : Outputs
                {
                    public readonly Output<List<U>> Out;
                    public readonly Output<T> FuncInput;

                    public MapOutputs(Component component) : base(2)
                    {
                        Out = new Output<List<U>>(component, "Out");
                        FuncInput = new Output<T>(component, "FuncInput");
                    }

                    public override void Detach()
                    {
                        Out.Detach();
                        FuncInput.Detach();
                    }
                }

                public readonly MapOutputs Outputs;
                private List<U> _out;

                private bool processing = false;
                private int index = 0;

                public Map(string ComponentName) : base(ComponentName)
                {
                    Inputs = new MapInputs(this);

                    Outputs = new MapOutputs(this);

                    AllowRecursion = true;
                }

                public override void Compute()
                {
                    if (Inputs.A.Value == null)
                    {
                        _out = null;
                        return;
                    }

                    if (processing)
                    {
                        _out.Add(Inputs.Func.Value);

                        if (index < Inputs.A.Value.Count - 1)
                        {
                            Outputs.FuncInput.Value = Inputs.A.Value[++index];
                        }
                    }
                    else
                    {
                        index = 0;
                        _out = new List<U>();

                        processing = true;

                        lock (Inputs.A.Lock)
                        {
                            Inputs.Func.Source.Component.AllowRecursion = true;

                            Outputs.FuncInput.Value = Inputs.A.Value[index];

                            Inputs.Func.Source.Component.AllowRecursion = false;
                        }

                        processing = false;
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

            public class Reduce<T, U> : Component
            {
                public class ReduceInputs : Inputs
                {
                    private Input<List<T>> _a;
                    public Input<List<T>> A
                    {
                        get
                        {
                            return _a;
                        }

                        set
                        {
                            if (_a != value)
                            {
                                if (_a != null) _a.Detach();
                                _a = value;
                            }
                        }
                    }

                    private Input<U> _func;
                    public Input<U> Func
                    {
                        get
                        {
                            return _func;
                        }

                        set
                        {
                            if (_func != value)
                            {
                                if (_func != null) _func.Detach();
                                _func = value;
                            }
                        }
                    }

                    private Input<U> _startingValue;
                    public Input<U> StartingValue
                    {
                        get
                        {
                            return _startingValue;
                        }

                        set
                        {
                            if (_startingValue != value)
                            {
                                if (_startingValue != null) _startingValue.Detach();
                                _startingValue = value;
                            }
                        }
                    }

                    public ReduceInputs(Component component) : base(3)
                    {
                        A = new Input<List<T>>(component, "A");
                        Func = new Input<U>(component, "Func");
                        StartingValue = new Input<U>(component, "StartingValue");
                    }

                    public override void Detach()
                    {
                        A.Detach();
                        Func.Detach();
                        StartingValue.Detach();
                    }
                }

                public readonly ReduceInputs Inputs;

                public class ReduceOutputs : Outputs
                {
                    public readonly Output<T> FuncInput;
                    public readonly Output<U> CurrentVal;
                    public readonly Output<U> Out;

                    public ReduceOutputs(Component component) : base(3)
                    {
                        FuncInput = new Output<T>(component, "FuncInput");
                        CurrentVal = new Output<U>(component, "CurrentVal");
                        Out = new Output<U>(component, "Out");
                    }

                    public override void Detach()
                    {
                        FuncInput.Detach();
                        CurrentVal.Detach();
                        Out.Detach();
                    }
                }

                public readonly ReduceOutputs Outputs;
                private U _out;

                public Reduce(string ComponentName) : base(ComponentName)
                {
                    Inputs = new ReduceInputs(this);

                    Outputs = new ReduceOutputs(this);
                }

                public override void Compute() // TODO: make this recursive
                {
                    if (Inputs.A.Value == null)
                    {
                        _out = default(U);
                        return;
                    }

                    _out = Inputs.StartingValue.Value;

                    Inputs.Func.Source.AutoTick = false;
                    for (int i = 0; i < Inputs.A.Value.Count; i++)
                    {
                        Outputs.FuncInput.Value = Inputs.A.Value[i];
                        Outputs.CurrentVal.Value = _out;
                        _out = Inputs.Func.Value;
                    }
                    Inputs.Func.Source.AutoTick = true;
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

            public class Filter<T> : Component
            {
                public class FilterInputs : Inputs
                {
                    private Input<List<T>> _a;
                    public Input<List<T>> A
                    {
                        get
                        {
                            return _a;
                        }

                        set
                        {
                            if (_a != value)
                            {
                                if (_a != null) _a.Detach();
                                _a = value;
                            }
                        }
                    }

                    private Input<bool> _func;
                    public Input<bool> Func
                    {
                        get
                        {
                            return _func;
                        }

                        set
                        {
                            if(_func != value)
                            {
                                if (_func != null) _func.Detach();
                                _func = value;
                            }
                        }
                    }

                    public FilterInputs(Component component) : base(2)
                    {
                        A = new Input<List<T>>(component, "A");
                        Func = new Input<bool>(component, "Cond");
                    }

                    public override void Detach()
                    {
                        A.Detach();
                        Func.Detach();
                    }
                }

                public readonly FilterInputs Inputs;

                public class FilterOutputs : Outputs
                {
                    public Output<List<T>> Out;
                    public Output<T> Element;

                    public FilterOutputs(Component component) : base(2)
                    {
                        Out = new Output<List<T>>(component, "Out");
                        Element = new Output<T>(component, "Element");
                    }

                    public override void Detach()
                    {
                        Out.Detach();
                        Element.Detach();
                    }
                }

                public FilterOutputs Outputs;
                private List<T> _out;

                private bool processing = false;
                private int index;

                public Filter(string ComponentName) : base(ComponentName)
                {
                    Inputs = new FilterInputs(this);
                    Outputs = new FilterOutputs(this);

                    AllowRecursion = true;
                }

                public override void Compute()
                {
                    if (Inputs.A.Value == null)
                    {
                        _out = null;
                        return;
                    }

                    if (processing)
                    {
                        if (Inputs.Func.Value) _out.Add(Inputs.A.Value[index]);

                        if (index < Inputs.A.Value.Count - 1)
                        {
                            Outputs.Element.Value = Inputs.A.Value[++index];
                        }
                    }
                    else
                    {
                        index = 0;
                        _out = new List<T>();

                        processing = true;

                        lock (Inputs.A.Lock)
                        {
                            Inputs.Func.Source.Component.AllowRecursion = true;

                            Outputs.Element.Value = Inputs.A.Value[index];

                            Inputs.Func.Source.Component.AllowRecursion = false;
                        }

                        processing = false;
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

            public class GetPos<T> : Component
            {
                private static uint count = 0;

                public class GPInputs : Inputs
                {
                    private Input<List<T>> _list;
                    public Input<List<T>> List
                    {
                        get
                        {
                            return _list;
                        }

                        set
                        {
                            if(_list != value)
                            {
                                if (_list != null) _list.Detach();
                                _list = value;
                            }
                        }
                    }

                    private Input<int> _pos;
                    public Input<int> Pos
                    {
                        get
                        {
                            return _pos;
                        }

                        set
                        {
                            if (_pos != value)
                            {
                                if (_pos != null) _pos.Detach();
                                _pos = value;
                            }
                        }
                    }

                    public GPInputs(Component component) : base(2)
                    {
                        List = new Input<List<T>>(component, "List");
                        Pos = new IntegerInput(component, "Pos");
                    }

                    public override void Detach()
                    {
                        List.Detach();
                        Pos.Detach();
                    }
                }

                public readonly GPInputs Inputs;

                public Outputs<T> Outputs;
                private T _out;

                private bool wrap;

                public GetPos(bool wrap = false) : base($"GetPos{count++}")
                {
                    Inputs = new GPInputs(this);

                    Outputs = new Outputs<T>(this);

                    this.wrap = wrap;
                }

                public override void Compute()
                {
                    if(Inputs.List.Value == null)
                    {
                        _out = default(T);
                    }
                    else if (wrap)
                    {
                        _out = Inputs.List.Value[Inputs.Pos.Value % Inputs.List.Value.Count];

                        
                    }
                    else if (Inputs.Pos.Value >= Inputs.List.Value.Count)
                    {
                        _out = default(T);
                    }
                    else
                    {
                        _out = Inputs.List.Value[Inputs.Pos.Value];
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
        }

        namespace Console
        {
            public class SetCursorPosition : Component
            {
                private static uint count = 0;

                public class SPInputs : Inputs
                {
                    private Input<int> _x;
                    public Input<int> X
                    {
                        get
                        {
                            return _x;
                        }
                        
                        set
                        {
                            if(_x != value)
                            {
                                if (_x != null) _x.Detach();
                                _x = value;
                            }
                        }
                    }

                    private Input<int> _y;
                    public Input<int> Y
                    {
                        get
                        {
                            return _y;
                        }

                        set
                        {
                            if (_y != value)
                            {
                                if (_y != null) _y.Detach();
                                _y = value;
                            }
                        }
                    }

                    public SPInputs(Component component) : base(2)
                    {
                        X = new IntegerInput(component, "X");
                        Y = new IntegerInput(component, "Y");
                    }

                    public override void Detach()
                    {
                        X.Detach();
                        Y.Detach();
                    }
                }

                public readonly SPInputs Inputs;

                private int _x, _y;

                public SetCursorPosition() : base($"ConsoleSetCursorPos{count++}")
                {
                    Inputs = new SPInputs(this);
                }

                public override void Compute()
                {
                    _x = Inputs.X.Value;
                    _y = Inputs.Y.Value;
                }

                public override void Set()
                {
                    System.Console.SetCursorPosition(_x, _y);
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class WriteLine : Component
            {
                private static uint count = 0;

                public class WLInputs : Inputs
                {
                    private Input<string> _str;
                    public Input<string> Str
                    {
                        get
                        {
                            return _str;
                        }
                        set
                        {
                            if(value != _str)
                            {
                                if (_str != null) _str.Detach();
                                _str = value;
                            }
                        }
                    }

                    private Input<bool> _clk;
                    public Input<bool> Clk
                    {
                        get
                        {
                            return _clk;
                        }
                        
                        set
                        {
                            if(value != _clk)
                            {
                                if (_clk != null) _clk.Detach();
                                _clk = value;
                            }
                        }
                    }

                    public WLInputs(Component component) : base(2)
                    {
                        Str = new StringInput(component, "Str");
                        Clk = new DigitalInput(component, "Clk");
                    }

                    public override void Detach()
                    {
                        Str.Detach();
                        Clk.Detach();
                    }
                }

                public readonly WLInputs Inputs;

                public WriteLine() : base($"WriteLine{count++}")
                {
                    Inputs = new WLInputs(this);
                }

                public override void Compute() { }

                public override void Set()
                {
                    if (Inputs.Clk.Value)
                    {
                        System.Console.WriteLine(Inputs.Str.Value);
                    }
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class ReadLine : Component
            {
                private static uint count = 0;

                public readonly Outputs<string> Outputs;

                private string _str;

                public ReadLine() : base($"ConsoleReadLine{count++}")
                {
                    Outputs = new Outputs<string>(this);
                }

                public override void Compute()
                {
                    _str = System.Console.ReadLine();
                }

                public override void Set()
                {
                    Outputs.Out.Value = _str;
                }

                public override void Detach()
                {
                    Outputs.Detach();
                }
            }

            public class GetWidth : Component
            {
                private static uint count = 0;

                public readonly Outputs<int> Outputs;

                private int _out;

                public GetWidth() : base($"ConsoleGetWidth{count++}")
                {
                    Outputs = new Outputs<int>(this);
                }

                public override void Compute()
                {
                    _out = System.Console.WindowWidth;
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

            public class SetWidth : Component
            {
                private static uint count = 0;

                public readonly Inputs<int> Inputs;

                private int _width;

                public SetWidth() : base($"ConsoleSetWidth{count++}")
                {
                    Inputs = new Inputs<int>(this);
                }

                public override void Compute()
                {
                    _width = Inputs.A.Value;
                }

                public override void Set()
                {
                    if (_width <= System.Console.LargestWindowWidth)
                    {
                        System.Console.WindowWidth = _width;
                    }
                    //System.Console.BufferWidth = _width;
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class GetHeight : Component
            {
                private static uint count = 0;

                public readonly Outputs<int> Outputs;

                private int _out;

                public GetHeight() : base($"ConsoleGetHeight{count++}")
                {
                    Outputs = new Outputs<int>(this);
                }

                public override void Compute()
                {
                    _out = System.Console.WindowWidth;
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

            public class SetHeight : Component
            {
                private static uint count = 0;

                public readonly Inputs<int> Inputs;

                private int _height;

                public SetHeight() : base($"ConsoleSetHeight{count++}")
                {
                    Inputs = new Inputs<int>(this);
                }

                public override void Compute()
                {
                    _height = Inputs.A.Value;
                }

                public override void Set()
                {
                    if(_height <= System.Console.LargestWindowHeight)
                    {
                        System.Console.WindowHeight = _height;
                    }
                    
                    //System.Console.BufferHeight = _height;
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class Colors : Input.Constant<List<ConsoleColor>>
            {
                public Colors() : base(new List<ConsoleColor>((ConsoleColor[]) Enum.GetValues(typeof(ConsoleColor)))) { }
            }

            public class GetForegroundColor : Component
            {
                private static uint count = 0;

                public Outputs<ConsoleColor> Outputs;

                private ConsoleColor _color;

                public GetForegroundColor() : base($"ConsoleGetFG{count++}")
                {
                    Outputs = new Outputs<ConsoleColor>(this);
                }

                public override void Compute()
                {
                    _color = System.Console.ForegroundColor;
                }

                public override void Set()
                {
                    Outputs.Out.Value = _color;
                }

                public override void Detach()
                {
                    Outputs.Detach();
                }
            }

            public class SetForegroundColor : Component
            {
                private static uint count = 0;

                public Inputs<ConsoleColor> Inputs;

                private ConsoleColor _color;

                public SetForegroundColor() : base($"ConsoleSetFG{count++}")
                {
                    Inputs = new Inputs<ConsoleColor>(this);
                }

                public override void Compute()
                {
                    _color = Inputs.A.Value;
                }

                public override void Set()
                {
                    System.Console.ForegroundColor = Inputs.A.Value;
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class GetBackgroundColor : Component
            {
                private static uint count = 0;

                public Outputs<ConsoleColor> Outputs;

                private ConsoleColor _color;

                public GetBackgroundColor() : base($"ConsoleGetBG{count++}")
                {
                    Outputs = new Outputs<ConsoleColor>(this);
                }

                public override void Compute()
                {
                    _color = System.Console.BackgroundColor;
                }

                public override void Set()
                {
                    Outputs.Out.Value = _color;
                }

                public override void Detach()
                {
                    Outputs.Detach();
                }
            }

            public class SetBackgroundColor : Component
            {
                private static uint count = 0;

                public Inputs<ConsoleColor> Inputs;

                private ConsoleColor _color;

                public SetBackgroundColor() : base($"ConsoleSetBG{count++}")
                {
                    Inputs = new Inputs<ConsoleColor>(this);
                }

                public override void Compute()
                {
                    _color = Inputs.A.Value;
                }

                public override void Set()
                {
                    System.Console.BackgroundColor = Inputs.A.Value;
                }

                public override void Detach()
                {
                    Inputs.Detach();
                }
            }

            public class ReadKey : Component
            {
                private static uint count = 0;

                public Outputs<ConsoleKey> Outputs;

                private ConsoleKey _out;

                public ReadKey() : base($"ConsoleReadKey{count++}")
                {
                    Outputs = new Outputs<ConsoleKey>(this);
                }

                public override void Compute()
                {
                    _out = System.Console.ReadKey(true).Key;
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
        }
    }
}