using System;

using CircuitSim.Chips.Input;
using CircuitSim.Chips.Console;

namespace ReadKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            var button = new Button();
            var beeper = new Beep();

            beeper.Inputs.Clk.Source = button.Outputs.Out;

            while(true){
                 Console.ReadLine();
                 button.State = true;
            }
        }
    }
}
