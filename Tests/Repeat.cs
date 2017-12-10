using System;

class RepeatTest {
 
    public static void Main(string[] args)
    {
        var str = new CircuitSim.Chips.Input.Constant<string>("TEST");
        var num = new CircuitSim.Chips.Input.Constant<int>(4);

        var repeat = new CircuitSim.Chips.String.Repeat();
        
        repeat.Inputs.Str.Source = str.Outputs[0];
        repeat.Inputs.Num.Source = num.Outputs[0];

        repeat.Tick();
        
        var str_out = repeat.Outputs.Out.Value;

        Console.Write("StrRepeat works: ");

        if(str_out == "TESTTESTTESTTEST"){
            Console.WriteLine("PASS");
        } else {
            Console.WriteLine("FAIL");
        }
    }
}
