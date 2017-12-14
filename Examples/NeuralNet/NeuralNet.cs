using System;

using CircuitSim.Chips.Input;
using CircuitSim.Chips.Neural;
using CircuitSim.Chips;

namespace Neural
{
    class Program
    {
        static void Main(string[] args)
        {
            FeedForward ff = new FeedForward(new int[] { 2, 3, 1 });
    
            GenericInput<double> A = new GenericInput<double>();
            GenericInput<double> B = new GenericInput<double>();

            ff.Inputs[0].Source = A.Outputs.Out;
            ff.Inputs[1].Source = B.Outputs.Out;

            for(int epoch = 0; epoch < 1000000; epoch++){
                A.Value = 0.0;
                B.Value = 0.0;
                Console.WriteLine($"XOR(0,0) = {ff.Outputs[0].Value}");
                ff.BackPropagate(new double[] { 0.0 }, 0.0001);

                A.Value = 1.0;
                B.Value = 0.0;
                Console.WriteLine($"XOR(1,0) = {ff.Outputs[0].Value}");
                ff.BackPropagate(new double[] { 1.0 }, 0.0001);
                
                A.Value = 0.0;
                B.Value = 1.0;
                Console.WriteLine($"XOR(0,1) = {ff.Outputs[0].Value}");
                ff.BackPropagate(new double[] { 1.0 }, 0.0001);
                
                A.Value = 1.0;
                B.Value = 1.0;
                Console.WriteLine($"XOR(1,1) = {ff.Outputs[0].Value}");
                ff.BackPropagate(new double[] { 0.0 }, 0.0001);
            }
                
            A.Value = 0.0;
            B.Value = 0.0;
            Console.WriteLine($"XOR(0,0) = {ff.Outputs[0].Value}");

            A.Value = 1.0;
            B.Value = 0.0;
            Console.WriteLine($"XOR(1,0) = {ff.Outputs[0].Value}");

            A.Value = 0.0;
            B.Value = 1.0;
            Console.WriteLine($"XOR(0,1) = {ff.Outputs[0].Value}");

            A.Value = 1.0;
            B.Value = 1.0;
            Console.WriteLine($"XOR(1,1) = {ff.Outputs[0].Value}");
        }
    }
}
