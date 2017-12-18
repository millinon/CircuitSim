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
         
        public double Bias = 0.0;
        public double BiasWeight = 0.0;

        private double _inputSum;
        public double InputSum
        {
            get
            {
                return _inputSum;
            }

            private set
            {
                _inputSum = value;
            }
        }

        public Neuron(int NumInputs, Func<double, double> Phi, Random Random) : base($"Neuron{count++}")
        {
            if(NumInputs <= 0) throw new Exception("Neuron must have a non-negative number of inputs");

            this.NumInputs = NumInputs;
            Inputs = new InputSet<double>(NumInputs);
            for(int i = 0; i < NumInputs; i++){
                Inputs[i] = new Input<double>(this, $"In{i}");
            }

            Outputs = new Outputs<double>(this);

            Weights = new double[NumInputs];

            this.Phi = Phi;

            for(int i = 0; i < NumInputs; i++) Weights[i] = (Random.NextDouble() - 0.5) * 5;
        }

        public Neuron(int NumInputs, Random Random) : this(NumInputs, a => 1.0 / (1.0 + Math.Exp(-a)), Random) { }
        public Neuron(int NumInputs, double Threshold, Random Random) : this(NumInputs, a => a >= Threshold ? 1.0 : 0.0, Random) { }

        public override void Compute()
        {
            double sum = 0.0;

            for(int i = 0; i < NumInputs; i++){
                sum += Inputs[i].Value * Weights[i];
            }

            InputSum = sum + Bias * BiasWeight;

            _out = this.Phi(InputSum);
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
    
    public class FeedForward : Component
    {
        public static uint count = 0;

        public readonly InputSet<double> Inputs;

        public readonly Output<double>[] Outputs;

        public readonly int[] Layers;
        
        public readonly int NumInputs;
        public readonly int NumOutputs;

        private Neuron[][] Neurons;

        private double _bias = 0.0;
        public double Bias
        {
            get
            {
                return _bias;
            }

            set
            {
                _bias = value;
                for(int i = 1; i < Layers.Length; i++){
                    for(int j = 0; j < Layers[i]; j++){
                        Neurons[i][j].Bias = _bias;
                    }
                }
            }
        }

        public FeedForward(int[] Layers) : base($"FeedForward{count++}")
        {
            if(Layers == null || Layers.Length == 0) throw new ArgumentException("Empty Layers array passed to FeedForward constructor");
            else if(Layers.Length < 2) throw new ArgumentException("FeedForward requires at least two layers");

            Random r = new Random();

            this.Layers = Layers;

            NumInputs = Layers[0];
            Inputs = new InputSet<double>(NumInputs);
            for(int i = 0; i < NumInputs; i++){
                Inputs[i] = new Input<double>(this, $"In{i}");
            }

            NumOutputs = Layers[Layers.Length - 1];
            Outputs = new Output<double>[NumOutputs];
            for(int i = 0; i < NumOutputs; i++){
                Outputs[i] = new Output<double>(this, $"Out{i}");
            }
            
            Neurons = new Neuron[Layers.Length][];

            Neurons[0] = new Neuron[NumInputs];
            for(int j = 0; j < NumInputs; j++){
                Neurons[0][j] = new Neuron(1, r);
                Neurons[0][j].Weights[0] = 1.0;
            }
            
            for(int i = 1; i < Layers.Length; i++){
                Neurons[i] = new Neuron[Layers[i]];

                for(int j = 0; j < Layers[i]; j++){
                    Neurons[i][j] = new Neuron(Layers[i-1], r);

                    for(int k = 0; k < Layers[i-1]; k++){
                        Neurons[i][j].Inputs[k].Source = Neurons[i-1][k].Outputs.Out;
                    }
                }
            }
        }

        public void BackPropagate(double[] Expected, double LearningRate)
        {
            if(Expected == null || Expected.Length != NumOutputs)
            {
                throw new ArgumentException("Expected array passed to BackPropagate doesn't match NN outputs");
            }

            double[][] gradients = new double[Layers.Length][];
            for(int i = 0; i < Layers.Length; i++){
                gradients[i] = new double[Layers[i]];
            }
                        
            double delta;
            double gradient;

            for(int i = 0; i < NumOutputs; i++){
                Neuron neuron = Neurons[Layers.Length-1][i];
                double output = neuron.Outputs.Out.Value;

                gradient = (output - Expected[i]) * output * (1.0 - output);

                for(int j = 0; j < neuron.NumInputs; j++){
                    delta = -1.0 * LearningRate * Neurons[Layers.Length-2][j].Outputs.Out.Value * gradient;
                    neuron.Weights[j] += delta;
                }
                
                gradients[Layers.Length-1][i] = gradient;
            }

            for(int layer = Layers.Length - 2; layer > 0; layer--){
                for(int neuron_idx = 0; neuron_idx < Layers[layer]; neuron_idx++){
                    Neuron neuron = Neurons[layer][neuron_idx];
                    double output = neuron.Outputs.Out.Value;

                    gradient = 0.0;

                    for(int j = 0; j < Layers[layer+1]; j++){
                        gradient += gradients[layer+1][j] * Neurons[layer+1][j].Weights[neuron_idx];
                    }
                    
                    gradient *= output * (1.0 - output);

                    for(int j = 0; j < neuron.NumInputs; j++){
                        if(layer > 0) delta = -1.0 * LearningRate * Neurons[layer-1][j].Outputs.Out.Value * gradient;
                        else delta = -1.0 * LearningRate * Inputs[j].Value * gradient;
                        neuron.Weights[j] += delta;
                    }

                    gradients[layer][neuron_idx] = gradient;
                }
            }

        }

        public override void Compute()
        {
            for(int i = 0; i < NumInputs; i++){
                Neurons[0][i].Inputs[0] = Inputs[i];
                Neurons[0][i].Tick();
            }
        }

        public override void Set()
        {
            for(int i = 0; i < NumOutputs; i++){
                Outputs[i].Value = Neurons[Neurons.Length - 1][i].Outputs.Out.Value;
            }
        }

        public override void Detach()
        {
            for(int i = 0; i < NumInputs; i++){
                Inputs[i].Detach();
            }

            for(int i = 0; i < NumOutputs; i++){
                Outputs[i].Detach();
            }
        }
    }
}
