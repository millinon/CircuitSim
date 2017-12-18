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

            Random r = new Random();

            Neuron[][] Neurons = new Neuron[2][];
            Neurons[0] = new Neuron[] {
                new Neuron(2, 1.0, r),
                new Neuron(2, 2.0, r),
                new Neuron(2, 1.0, r)
            };
            Neurons[1] = new Neuron[] {
                new Neuron(3, 1.0, r)
            };

            Neurons[0][0].Inputs[0].Source = A.Outputs.Out;
            Neurons[0][0].Weights[0] = 1.0;
            Neurons[0][0].Inputs[1].Source = B.Outputs.Out;
            Neurons[0][0].Weights[1] = 1.0;

            Neurons[0][1].Inputs[0].Source = A.Outputs.Out;
            Neurons[0][1].Weights[0] = 1.0;
            Neurons[0][1].Inputs[1].Source = B.Outputs.Out;
            Neurons[0][1].Weights[1] = 1.0;
            
            Neurons[0][2].Inputs[0].Source = A.Outputs.Out;
            Neurons[0][2].Weights[0] = 1.0;
            Neurons[0][2].Inputs[1].Source = B.Outputs.Out;
            Neurons[0][2].Weights[1] = 1.0;
            
            Neurons[1][0].Inputs[0].Source = Neurons[0][0].Outputs.Out;
            Neurons[1][0].Weights[0] = 1.0;
            Neurons[1][0].Inputs[1].Source = Neurons[0][1].Outputs.Out;
            Neurons[1][0].Weights[1] = -2.0;
            Neurons[1][0].Inputs[2].Source = Neurons[0][2].Outputs.Out;
            Neurons[1][0].Weights[2] = 1.0;

            double LearningRate = 0.1;

            double[] vals = new double[2] { 0.0, 1.0 };

            for(int epoch = 0; epoch < 100000; epoch++){
                for(int a = 0; a <= 1; a++){
                    for(int b = 0; b <= 1; b++){
                        A.Value = vals[a];
                        B.Value = vals[b];
                       
                        ff.BackPropagate(new double[] { Neurons[1][0].Outputs.Out.Value }, LearningRate);
                    }
                }
            }
                
            for(int a = 0; a <= 1; a++){
                for(int b = 0; b <= 1; b++){
                    A.Value = vals[a];
                    B.Value = vals[b];

                    Console.WriteLine($"XOR({a},{b}): expected = {Neurons[1][0].Outputs.Out.Value}, actual = {ff.Outputs[0].Value}");
                }
            }
        }
    }
}
