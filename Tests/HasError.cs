using System;

class HasErrorTest {
 
    public static void Main(string[] args)
    {
        var inv = new CircuitSim.Chips.Integer.Arithmetic.Inv();
        var add = new CircuitSim.Chips.Integer.Arithmetic.Add();

        inv.Inputs.A.Source = new CircuitSim.Chips.Input.Constant<int>(new int[] { 0 }).Outputs[0];
        
        add.Inputs[0].Source = inv.Outputs.Out;
        add.Inputs[1].Source = new CircuitSim.Chips.Input.Constant<int>(new int[] { 2 }).Outputs[0];

        inv.Tick();

        Console.Write("HasError works: ");

        if(add.HasError){
            Console.WriteLine("PASS");
        } else {
            Console.WriteLine("FAIL");
        }
    }
}
