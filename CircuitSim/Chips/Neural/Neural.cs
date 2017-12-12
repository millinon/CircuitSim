using System;

namespace CircuitSim.Chips.Neural
{
    public class Neuron : Component
    {
        private static uint count = 0;

        public readonly InputSet<double> Inputs;
        public readonly Outputs<double> Outputs;
        private double _out;

        public readonly int NumInputs;
        public readonly double[] Weights;
        public readonly Func<double, double> Phi;

        public Neuron(int NumInputs, Func<double, double> Phi, Random Random) : base($"Neuron{count++}")
        {
            if(NumInputs <= 0) throw new Exception("Neuron must have a non-negative number of inputs");

            this.NumInputs = NumInputs;
            Inputs = new InputSet<double>(NumInputs);
            Weights = new double[NumInputs];

            this.Phi = Phi;

            for(int i = 0; i < NumInputs; i++) Weights[i] = Random.NextDouble();
        }

        public Neuron(int NumInputs, double Threshold, Random Random) : this(NumInputs, a => a >= Threshold ? 1.0 : 0.0, Random) { }

        public override void Compute()
        {
            double sum = 0.0;

            for(int i = 0; i < NumInputs; i++){
                sum += Inputs[i].Value * Weights[i];
            }

            _out = this.Phi(sum);
        }

        public override void Set()
        {
            Outputs.Out.Value = _out;
        }

        public override void Detach()
        {
            for(int i = 0; i < NumInputs; i++){
                Inputs[i].Detach();
            }
        }
    }
}
