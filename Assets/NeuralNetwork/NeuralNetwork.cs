using System;
using System.Collections.Generic;
using System.Linq;

public class NeuralNetwork
{
    public delegate double ActivationFunction(double value);
    private ICollection<NeuralLayer> _layers = new List<NeuralLayer>();
    private Random _random;
    private const double mutationProbabilty = 0.2;
    private const double mutationAmount = 2.0;
    public int NumberOfLayers { get; private set; }
    public NeuralNetwork(params int[] numberOfNeuronsInLayers)
    {
        if (numberOfNeuronsInLayers.Length < 2)
        {
            throw new ArgumentException("Netowrk has to have atleast three layers.");
        }

        NumberOfLayers = numberOfNeuronsInLayers.Length;

        _random = new Random();

        foreach (var numberOfNeuronsInLayer in numberOfNeuronsInLayers)
        {
            _layers.Add(new NeuralLayer(numberOfNeuronsInLayer, _random, ReLU));
        }
    }
    public int GetAmmountOfNeuronsInLayer(int index)
    {
        return _layers.ToArray()[index].GetAmmountOfNeurons();
    }
    public NeuralNetwork(NeuralNetwork network)
    {
        if (network == null)
        {
            throw new ArgumentNullException("Network has not been provided");
        }

        _random = new Random(network._random.Next());

        NumberOfLayers = network.NumberOfLayers;

        foreach (var layer in network._layers)
        {
            _layers.Add(new NeuralLayer(layer));
        }
    }
    public double[] PropagateForward(double[] input)
    {
        if (input == null)
        {
            throw new ArgumentNullException("Input has not been provided.");
        }
        
        if (input.Length != GetAmmountOfNeuronsInLayer(0))
        {
            throw new ArgumentException("Input does not have same ammount of neurons as first layer.");
        }
           
        var result = input;

        foreach (var layer in _layers)
        {
            result = layer.PropagateForward(result);
        }

        return result;
    }
    public void Mutate()
    {
        foreach (var layer in _layers)
        {
            layer.Mutate(mutationProbabilty, mutationAmount);
        }
    }
    private double ReLU(double value)
    {
        return value < 0 ? 0 : value;
    }
    private double Sigmoid(double value)
    {
        return 1 / (1 + Math.Exp(-value));
    }
}