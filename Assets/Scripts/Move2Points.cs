using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Move2Points : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 5f;

    void Start()
    {
        // Start the movement coroutine
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            // Calculate the distance between the start and end points
            float distance = Vector3.Distance(startPoint.position, endPoint.position);

            // Calculate the current position along the ping-pong path
            float pingPongTime = Mathf.PingPong(Time.time * speed, distance);

            // Set the position of the object based on the ping-pong time
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, pingPongTime / distance);

            // Wait for the next frame
            yield return null;
        }
    }
}
