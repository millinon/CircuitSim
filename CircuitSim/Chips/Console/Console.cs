using System;
using System.Collections.Generic;

namespace CircuitSim.Chips.Console
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
                X = new Input<int>(component, "X");
                Y = new Input<int>(component, "Y");
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
                Str = new Input<string>(component, "Str");
                Clk = new Input<bool>(component, "Clk");
            }

            public override void Detach()
            {
                Str.Detach();
                Clk.Detach();
            }
        }

        public readonly WLInputs Inputs;

        private string _str;

        public WriteLine() : base($"WriteLine{count++}")
        {
            Inputs = new WLInputs(this);
        }

        public override void Compute()
        {
            _str = Inputs.Str.Value;
        }

        public override void Set()
        {
            if (Inputs.Clk.Value)
            {
                System.Console.WriteLine(_str);
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
            System.Console.ForegroundColor = _color;
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
            System.Console.BackgroundColor = _color;
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
