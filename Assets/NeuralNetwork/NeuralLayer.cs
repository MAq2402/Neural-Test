using System;
using System.Linq;

public class NeuralLayer
{
    private Neuron[] _neurons;
    private Random _random;

    private double[][] _weights;
    private double[] _biases;

    private NeuralNetwork.ActivationFunction _activationFunction;
    public NeuralLayer(int ammountOfNeurons, Random random, NeuralNetwork.ActivationFunction activationFunction)
    {
        if (ammountOfNeurons <= 0)
        {
            throw new ArgumentException("Amount of neurons has to be greater than zero.");
        }
        
        if (random == null)
        {
            throw new ArgumentNullException("Random has not been provided");
        }

        if (activationFunction == null)
        {
            throw new ArgumentNullException("Activation function has not been provided");
        }


        _activationFunction = activationFunction;
        _random = random;
        _neurons = new Neuron[ammountOfNeurons];
        InitializeWeights();
        InitializeBiases();
    }
    public NeuralLayer(NeuralLayer neuralLayer)
    {
        if (neuralLayer == null)
        {
            throw new ArgumentNullException("Neural layer has not been provided");
        }

        CloneNeuralLayer(neuralLayer);
    }
    public int GetAmmountOfNeurons()
    {
        return _neurons.Count();
    }
    private void InitializeBiases()
    {
        _biases = new double[_neurons.Length];
        for (int i = 0; i < _biases.Length; i++)
        {
            _biases[i] = _random.NextDouble() - 0.5;
        }
    }
    private void InitializeWeights()
    {
        _weights = new double[_neurons.Length][];

        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = new double[_weights.Length];
            for (int j = 0; j < _weights.Length; j++)
            {
                _weights[i][j] = _random.NextDouble() - 0.5;
            }
        }
    }
    private void CloneNeuralLayer(NeuralLayer neuralLayer)
    {
        _activationFunction = neuralLayer._activationFunction;
        _random = neuralLayer._random;
        _weights = new double[neuralLayer._weights.Length][];
        _neurons = new Neuron[neuralLayer._neurons.Length];
        _biases = new double[neuralLayer._biases.Length];
        for (int i = 0; i < _weights.Length; i++)
        {
            _biases[i] = neuralLayer._biases[i];
            _weights[i] = new double[neuralLayer._weights[0].Length];
            for (int j = 0; j < _weights[i].Length; j++)
            {
                _weights[i][j] = neuralLayer._weights[i][j];
            }
        }
    }
    public double[] PropagateForward(double[] neurons)
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            var neuronValue = 0.0;
            for (int j = 0; j < _weights[i].Length; j++)
            {
                neuronValue += _weights[i][j] * neurons[j];
            }
            neuronValue += _biases[i];
            var neuron = new Neuron(neuronValue, _activationFunction);

            _neurons[i] = neuron;
        }

        return _neurons.Select(n => n.Value).ToArray();
    }
    public void Mutate(double mutationProbabilty, double mutationRange)
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            for (int j = 0; j < _weights[i].Length; j++)
            {
                if (_random.NextDouble() < mutationProbabilty)
                    _weights[i][j] = _random.NextDouble() * (mutationRange * 2) - mutationRange;
            }
        }
    }
}