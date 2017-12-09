using System;

using CircuitSim.Chips.Input;
using CircuitSim.Chips.Console;
using CircuitSim.Chips;

namespace ReadKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadKey consoleKey = new ReadKey();

            Constant<ConsoleKey> key_dirs = new Constant<ConsoleKey>(new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow });
            key_dirs.Tick();

            Constant<string> key_strs = new Constant<string>(new string[] { "UP", "DOWN", "LEFT", "RIGHT" });
            key_strs.Tick();

            WriteLine writeUp = new WriteLine();
            WriteLine writeDn = new WriteLine();
            WriteLine writeLft = new WriteLine();
            WriteLine writeRt = new WriteLine();

            Equals<ConsoleKey> isUp = new Equals<ConsoleKey>();
            Equals<ConsoleKey> isDn = new Equals<ConsoleKey>();
            Equals<ConsoleKey> isLft = new Equals<ConsoleKey>();
            Equals<ConsoleKey> isRt = new Equals<ConsoleKey>();

            isUp.Inputs.A.Source = key_dirs.Outputs[0];
            isUp.Inputs.B.Source = consoleKey.Outputs.Out;
            writeUp.Inputs.Str.Source = key_strs.Outputs[0];
            writeUp.Inputs.Clk.Source = isUp.Outputs.Out;

            isDn.Inputs.A.Source = key_dirs.Outputs[1];
            isDn.Inputs.B.Source = consoleKey.Outputs.Out;
            writeDn.Inputs.Str.Source = key_strs.Outputs[1];
            writeDn.Inputs.Clk.Source = isDn.Outputs.Out;

            isLft.Inputs.A.Source = key_dirs.Outputs[2];
            isLft.Inputs.B.Source = consoleKey.Outputs.Out;
            writeLft.Inputs.Str.Source = key_strs.Outputs[2];
            writeLft.Inputs.Clk.Source = isLft.Outputs.Out;

            isRt.Inputs.A.Source = key_dirs.Outputs[3];
            isRt.Inputs.B.Source = consoleKey.Outputs.Out;
            writeRt.Inputs.Str.Source = key_strs.Outputs[3];
            writeRt.Inputs.Clk.Source = isRt.Outputs.Out;

            while (true)
            {
                consoleKey.Tick();
            }
        }
    }
}
