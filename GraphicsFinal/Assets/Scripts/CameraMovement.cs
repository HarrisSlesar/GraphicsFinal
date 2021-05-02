using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;

    float xVal = 0.0f;
    float yVal = 0.0f;
    float zVal = 0.0f;
    float rotVal = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //gameObject.transform.position = 
            gameObject.transform.Translate(0.0f, 0.0f, zVal + 0.25f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            //gameObject.transform.position = 
            gameObject.transform.Translate(0.0f, 0.0f, zVal - 0.25f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            //gameObject.transform.position = 
            gameObject.transform.Translate(xVal + 0.25f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //gameObject.transform.position = 
            gameObject.transform.Translate(xVal - 0.25f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.E))
        {      
            gameObject.transform.Rotate(0.0f, rotVal + 0.25f, 0.0f);
        }

        if (Input.GetKey(KeyCode.Q))
        { 
            gameObject.transform.Rotate(0.0f, rotVal - 0.25f, 0.0f);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            gameObject.transform.Translate(0.0f, yVal + 0.25f, 0.0f);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            gameObject.transform.Translate(0.0f, yVal - 0.25f, 0.0f);
        }
    }
}
