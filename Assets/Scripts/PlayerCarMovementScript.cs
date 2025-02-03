using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //FIXME: Test Code, remove me once movement along segmented path is taken care of by another component
        if (Input.GetKey(KeyCode.Space) 
            || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.W))
        {
            moveCarForward(Time.deltaTime, 2);
        } if (Input.GetKey(KeyCode.S)
              || Input.GetKey(KeyCode.DownArrow))
        {
            moveCarForward(Time.deltaTime, -2);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnCar(Time.deltaTime, -80);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnCar(Time.deltaTime, 80);
        }
    }

    /// <summary>
    /// Parametrically turns the car a certain angle in a given duration (default 1s)
    /// </summary>
    /// <param name="deltaTime">time since last update for the turn to be consistent wrt fps</param>
    /// <param name="angle">turn angle in degrees</param>
    /// <param name="durationInSeconds"></param>
    void turnCar(float deltaTime, float angle, float durationInSeconds = 1)
    {
        transform.RotateAround(transform.position, Vector3.up, angle * (deltaTime/durationInSeconds));
    }

    /// <summary>
    /// Parametrically moves the car forward
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="durationInSeconds"></param>
    void moveCarForward(float deltaTime, float speed, float durationInSeconds = 1)
    {
        transform.Translate(Vector3.forward * (speed * (deltaTime/durationInSeconds)));
    }

    public void setColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
