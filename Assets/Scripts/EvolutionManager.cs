using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject carPrefab;

    [SerializeField]
    private Text generationNumberText;

    private int numberOfCars = 100; 

    private int generationCounter = 0; 

    private ICollection<Car> cars = new List<Car>();

    public static NeuralNetwork BestNeuralNetwork = null;

    private int bestScore = int.MinValue;
    public static EvolutionManager Instance { get; private set; }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        BestNeuralNetwork = new NeuralNetwork( 6, 4, 3, 2 );

        StartGeneration(); 
    }
    private void StartGeneration()
    {
        generationCounter++;
        generationNumberText.text = "Generacja: " + generationCounter;

        for (int i = 0; i < numberOfCars; i++)
        {
            cars.Add(Instantiate(carPrefab, transform.position, Quaternion.identity, transform).GetComponent<Car>());    
        }
    }
    public void OnCarDeath(Car car)
    {
        cars.Remove(car);
        Destroy(car.gameObject); 

        if (car.Score > bestScore)
        {
            BestNeuralNetwork = car.NeuralNetwork; 
            bestScore = car.Score; 
        }

        if (!cars.Any())
        {
            StartGeneration(); 
        }   
    }
}
