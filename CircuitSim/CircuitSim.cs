using System;
using System.Collections;
using System.Collections.Generic;

namespace CircuitSim
{
    public abstract class Component
    {
        private bool _visited = false;

        // WARNING: this is a hack to make recursive chips work. You generally should not ever set this to true.
        public bool AllowRecursion = false;

        private bool _error = false;
        public bool HasError {
            get {
                return _error;
            }

            set {
                _error = value;
            }
        }

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
            HasError = false;
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
                if(Source.Component.HasError) Component.HasError = true;
                
                if (Source != null) return Source.Value;
                else
                {
                    throw new Exception($"input {Name} of component {Component.Name} is not connected to any output");
                }
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
}
