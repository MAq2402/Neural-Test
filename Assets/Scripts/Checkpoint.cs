using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private ICollection<Car> cars = new List<Car>();
    private void OnTriggerEnter(Collider other)
    {
        var car = other.transform.parent.GetComponent<Car>();
        if (car != null && !cars.Any(c => c == car))
        {
            cars.Add(car);
            car.IncreaseScore();
        }
    }
}
