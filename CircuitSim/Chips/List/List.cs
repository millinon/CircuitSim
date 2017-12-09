using System.Collections.Generic;

namespace CircuitSim.Chips.List
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
                Pos = new Input<int>(component, "Pos");
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
