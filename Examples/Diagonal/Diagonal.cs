public class Diagonal
{
    public static void Main(string[] args)
    {
        var conpos = new CircuitSim.Chips.Console.SetCursorPosition(); // this chip sets the console cursor position
        var write = new CircuitSim.Chips.Console.WriteLine(); // this chip calls System.Console.WriteLine
        var colors = new CircuitSim.Chips.Console.Colors(); // this chip is a list of all of the console colors
        var setcolor = new CircuitSim.Chips.Console.SetForegroundColor(); // this chip sets the foreground color of the console
        
        var getpos = new CircuitSim.Chips.List.GetPos<System.ConsoleColor>(true); // this chip looks up a value from a list
                                                                                  // the 'true' argument makes it wrap around the end
        var button = new CircuitSim.Chips.Input.Button(); // a generic boolean input
        var str = new CircuitSim.Chips.Input.Constant<string>(new string[] { "X" });

        var acc = new CircuitSim.Chips.Integer.Accumulator();

        write.Inputs.Clk.Source = button.Outputs.Out; // the writeline should be triggered whenever the button's output goes high
        write.Inputs.Str.Source = str.Outputs[0]; // the writeline's input is the constant ("X")

        conpos.Inputs.X.Source = acc.Outputs.Out; // the position's X and Y should come from the output of the accumululator
        conpos.Inputs.Y.Source = acc.Outputs.Out;

        getpos.Inputs.List.Source = colors.Outputs[0]; // getpos is going to fetch a color from the list of colors
        getpos.Inputs.Pos.Source = acc.Outputs.Out;

        setcolor.Inputs.A.Source = getpos.Outputs.Out; // setcolor is going to assign the color emitted by getpos to the console foreground

        for(int i = 0; i < 20; i++){
            acc.Increment(); // increment the accumulator, which will then call Tick() on conpos and getpos. getpos.Tick() will call write.Tick()
            button.State = true; // trigger write
        }
    }
}
