namespace Sin
{
    class Program
    {
        static void Main(string[] args)
        {
            var repeat = new CircuitSim.Chips.String.Repeat();

            var sin = new CircuitSim.Chips.FloatingPoint.Arithmetic.Sin();

            var time = new CircuitSim.Chips.Time.Ticks();
            var t2i = new CircuitSim.Chips.Time.Conversion.ToIntLow();

            var add = new CircuitSim.Chips.FloatingPoint.Arithmetic.Add();
            var mul = new CircuitSim.Chips.FloatingPoint.Arithmetic.Mul();
            var f2i = new CircuitSim.Chips.FloatingPoint.Conversion.Round();

            var i2f = new CircuitSim.Chips.Integer.Conversion.ToFloat();

            var writeline = new CircuitSim.Chips.Console.WriteLine();

            t2i.Inputs.A.Source = time.Outputs.Out;
            i2f.Inputs.A.Source = t2i.Outputs.Out;

            sin.Inputs.A.Source = i2f.Outputs.Out;

            mul.Inputs[0].Source = sin.Outputs.Out;
            mul.Inputs[1].Source = new CircuitSim.Chips.Input.Constant<double>(20.0).Outputs[0];

            add.Inputs[0].Source = mul.Out;
            add.Inputs[1].Source = new CircuitSim.Chips.Input.Constant<double>(20.0).Outputs[0];

            f2i.Inputs.A.Source = add.Out;
            
            repeat.Inputs.Num.Source = f2i.Outputs.Out;
            repeat.Inputs.Str.Source = new CircuitSim.Chips.Input.Constant<string>("#").Outputs[0];

            writeline.Inputs.Str.Source = repeat.Outputs.Out;
            writeline.Inputs.Clk.Source = new CircuitSim.Chips.Input.Constant<bool>(true).Outputs[0];

            while(true){
                time.Tick();
            }
        }
    }
}
