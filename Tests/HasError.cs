using System;

class HasErrorTest {
 
    public static void Main(string[] args)
    {
        var div = new CircuitSim.Chips.Integer.Arithmetic.Div();
        var add = new CircuitSim.Chips.Integer.Arithmetic.Add();

        var consts = new CircuitSim.Chips.Input.Constant<int>(new int[] { 1, 0, 2});

        div.Inputs.A.Source = consts.Outputs[0];
        div.Inputs.B.Source = consts.Outputs[1];

        add.Inputs[0].Source = div.Outputs.Out;
        add.Inputs[1].Source = consts.Outputs[2];

        div.Tick();

        Console.Write("HasError works: ");

        if(add.HasError){
            Console.WriteLine("PASS");
        } else {
            Console.WriteLine("FAIL");
        }
    }
}
