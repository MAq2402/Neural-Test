using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var car = collision.transform.GetComponent<Car>();

        if(car != null)
        {
            car.Kill();
        }
    }
}
