public class Neuron
{
    public double Value { get; private set; }
    public Neuron(double value)
    {
        Value = value;
    }

    public Neuron(double value, NeuralNetwork.ActivationFunction activationFunction)
    {
        Value = activationFunction(value);
    }
}

