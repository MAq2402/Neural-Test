using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private LayerMask sensorMask;
    private float doesNotProgressWaitTime = 5;
    private Rigidbody rigidBody;
    private const int sensorRange = 3;

    public int Score { get; private set; }

    public NeuralNetwork NeuralNetwork { get; private set; }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        NeuralNetwork = new NeuralNetwork(EvolutionManager.BestNeuralNetwork);
        NeuralNetwork.Mutate();

        StartCoroutine(DoesNotProgress());
    }
    private IEnumerator DoesNotProgress()
    {
        while (true)
        {
            var scoreOnTimeCountStart = Score;
            yield return new WaitForSeconds(doesNotProgressWaitTime);
            if (scoreOnTimeCountStart == Score)
            {
                Kill();
            }
        }
    }
    private void FixedUpdate()
    {
        var networkOutput = FeedForwardNetwork();

        var sidewayMovement = CalculateSidewayMovement(networkOutput);
        var forwardMovement = CalculateForwardMovement(networkOutput, sidewayMovement);

        Move(forwardMovement, sidewayMovement);
    }

    private double[] FeedForwardNetwork()
    {
        var neuralNetworkInput = new double[6];

        neuralNetworkInput[0] = GetDistanceToWall(transform.forward);
        neuralNetworkInput[1] = GetDistanceToWall(-transform.forward);
        neuralNetworkInput[2] = GetDistanceToWall(transform.right);
        neuralNetworkInput[3] = GetDistanceToWall(-transform.right);

        var sqrtHalf = Mathf.Sqrt(0.5f);
        neuralNetworkInput[4] = GetDistanceToWall(transform.right * sqrtHalf + transform.forward * sqrtHalf);
        neuralNetworkInput[5] = GetDistanceToWall(transform.right * sqrtHalf + -transform.forward * sqrtHalf);

        return NeuralNetwork.PropagateForward(neuralNetworkInput);
    }
    private double GetDistanceToWall(Vector3 direction)
    {
        RaycastHit raycastHit;

        return Physics.Raycast(transform.position, direction, out raycastHit, sensorRange, sensorMask) ?
            Vector3.Distance(raycastHit.point, transform.position) : sensorRange;
    }

    private int CalculateForwardMovement(double[] networkOutput, int horizontal)
    {
        if (networkOutput[0] == 0)
        {
            return -1;
        }
        else if (networkOutput[0] >= 1f || horizontal == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private int CalculateSidewayMovement(double[] networkOutput)
    {
        if (networkOutput[1] == 0f)
        {
            return -1;
        }
        else if (networkOutput[1] >= 1f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    private void Move(float forwardMovement, float sidewayMovement)
    {
        rigidBody.velocity = transform.right * forwardMovement * 4;
        rigidBody.angularVelocity = transform.up * sidewayMovement * 3;
    }
    public void IncreaseScore()
    {
        Score++;
    }
    public void Kill()
    {
        EvolutionManager.Instance.OnCarDeath(this);
        gameObject.SetActive(false);
    }
}
