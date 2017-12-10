using System;

namespace CircuitSim.Chips.String.Conversion
{
    public class ToDigital : Component
    {
        private static uint count = 0;

        private string _high;
        private string _low;
        private bool _caseInsensitive;

        public readonly Inputs<string> Inputs;

        public readonly Outputs<bool> Outputs;

        private bool _out;

        public ToDigital(string high = "true", string low = "false", bool caseInsensitive = true) : base($"StrToDigital{count++}")
        {
            _high = high;
            _low = low;
            _caseInsensitive = caseInsensitive;

            Inputs = new Inputs<string>(this);
            Outputs = new Outputs<bool>(this);
        }

        public override void Compute()
        {
            if(string.Compare(Inputs.A.Value, _high, _caseInsensitive) == 0){
                _out = true;
            } else if(string.Compare(Inputs.A.Value, _low, _caseInsensitive) == 0){
                _out = false;
            } else {
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

    public class ToFloat : Component
    {
        private static uint count = 0;

        public readonly Inputs<string> Inputs;

        public readonly Outputs<double> Outputs;
        private double _out;
        
        public ToFloat() : base($"StrToFloat{count++}")
        {
            Inputs = new Inputs<string>(this);

            Outputs = new Outputs<double>(this);
        }

        public override void Compute()
        {
            try {
                _out = double.Parse(Inputs.A.Value);
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
    
    public class ToInt : Component
    {
        private static uint count = 0;

        public readonly Inputs<string> Inputs;

        public readonly Outputs<int> Outputs;
        private int _out;
        
        public ToInt() : base($"StrToInt{count++}")
        {
            Inputs = new Inputs<string>(this);

            Outputs = new Outputs<int>(this);
        }

        public override void Compute()
        {
            try {
                _out = int.Parse(Inputs.A.Value);
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
    
    public class ToTime : Component
    {
        private static uint count = 0;

        public readonly Inputs<string> Inputs;

        public readonly Outputs<long> Outputs;
        private long _out;
        
        public ToTime() : base($"StrToTime{count++}")
        {
            Inputs = new Inputs<string>(this);

            Outputs = new Outputs<long>(this);
        }

        public override void Compute()
        {
            try {
                _out = long.Parse(Inputs.A.Value);
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
}
