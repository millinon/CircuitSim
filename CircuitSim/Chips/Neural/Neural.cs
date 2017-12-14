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
        public readonly Func<double, double> DPhi;
         
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

        public Neuron(int NumInputs, Func<double, double> Phi, Func<double, double> DPhi, Random Random) : base($"Neuron{count++}")
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
            this.DPhi = DPhi;

            for(int i = 0; i < NumInputs; i++) Weights[i] = Random.NextDouble() - 0.5;
        }

        public Neuron(int NumInputs, Random Random) : this(NumInputs, a => 1.0 / (1.0 + Math.Exp(-a)), a => Math.Exp(a) / Math.Pow(Math.Exp(a) + 1.0, 2.0) , Random) { }

        public override void Compute()
        {
            double sum = 0.0;

            for(int i = 0; i < NumInputs; i++){
                sum += Inputs[i].Value * Weights[i];
            }

            InputSum = sum + Bias * BiasWeight;

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
    
    public class FeedForward : Component
    {
        public static uint count = 0;

        public readonly InputSet<double> Inputs;

        public readonly Output<double>[] Outputs;

        public readonly int[] Layers;

        public readonly int NumInputs;
        public readonly int NumOutputs;

        private Neuron[][] Neurons;

        public readonly Func<double, double> Phi;
        public readonly Func<double, double> DPhi;

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

        public FeedForward(int[] Layers, Func<double, double> Phi, Func<double, double> DPhi) : base($"FeedForward{count++}")
        {
            if(Layers == null || Layers.Length == 0) throw new ArgumentException("Empty Layers array passed to FeedForward constructor");
            else if(Layers.Length < 2) throw new ArgumentException("FeedForward requires at least two layers");

            Random r = new Random();

            this.Layers = Layers;

            // initialize the inputs and 
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
            }
            
            for(int i = 1; i < Layers.Length; i++){
                Neurons[i] = new Neuron[Layers[i]];

                for(int j = 0; j < Layers[i]; j++){
                    Neurons[i][j] = new Neuron(Layers[i-1], Phi, DPhi, r);

                    for(int k = 0; k < Layers[i-1]; k++){
                        Neurons[i][j].Inputs[k].Source = Neurons[i-1][k].Outputs.Out;
                    }
                }
            }

            this.Phi = Phi;
            this.DPhi = DPhi;
        }

        public FeedForward(int[] Layers) : this(Layers, a => 1.0 / (1 + Math.Exp(-a)), a => Math.Exp(a) / Math.Pow(Math.Exp(a) + 1.0, 2.0) ) { }

        public void BackPropagate(double[] Expected, double LearningRate, Func<double, double, double> Error, Func<double, double, double> DError)
        {
            if(Expected == null || Expected.Length != NumOutputs)
            {
                throw new ArgumentException("Expected array passed to BackPropagate doesn't match NN outputs");
            } else if(Error == null)
            {
                throw new ArgumentException("Error function is required for BackPropagate, but one was not provided");
            } else if(DError == null)
            {
                throw new ArgumentException("DError function is required for BackPropagate, but one was not provided");
            }

            double[] layergradients = new double[Layers.Length];

            for(int i = 0; i < NumOutputs; i++){
                Neuron neuron = Neurons[Layers.Length-1][i];
                double output = neuron.Outputs.Out.Value;

                double gradient = DError(Expected[i], output) * DPhi(neuron.InputSum);
                    
                for(int j = 0; j < neuron.NumInputs; j++){
                    neuron.Weights[j] += -1.0 * LearningRate * output * gradient;
                }
                
                layergradients[Layers.Length - 1] += gradient;
            }

            for(int layer = Layers.Length - 2; layer >= 0; layer--){
                for(int neuron_idx = 0; neuron_idx < Layers[layer]; neuron_idx++){
                    Neuron neuron = Neurons[layer][neuron_idx];
                    double output = neuron.Outputs.Out.Value;

                    double gradient = layergradients[layer+1] * DPhi(neuron.InputSum);

                    for(int j = 0; j < neuron.NumInputs; j++){
                        neuron.Weights[j] += -1.0 * LearningRate * output * gradient;
                    }

                    layergradients[layer] += gradient;
                }
            }
        }

        public void BackPropagate(double[] Expected, double LearningRate)
        {
            BackPropagate(Expected, LearningRate, (a, b) => Math.Pow(Math.Abs(a-b), 2.0) / 2.0, (a, b) => (a-b) * (1.0 - Math.Pow(a, 2.0)));
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
