using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRom : MonoBehaviour
{
    public Transform[] points;
    public bool looping = true;

    public bool paused = false;


    float segmentTime = 0;
    float dt = 0.016f;
    public float segmentDuration = 2;
    float durationInv;

    Vector3 lastPosition;
    Vector3 newPosition;
    public int segment = 0;
    int maxSegmentAmount = 8; //maybe this should be 3?
    public float rotateSpeed = 0.0f;

    public int currentIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }
    }

    void FixedUpdate()
    {

        if (paused == false)
        {
            //setting up duration information and indexes
            durationInv = 1 / segmentDuration;
            int indexStart = currentIndex;
            int indexEnd = (indexStart + 1) % points.Length;
            segmentTime += dt;

            //advancing the indexes if the segment time has passed duration
            if (segmentTime >= segmentDuration)
            {
                segmentTime -= segmentDuration;
                indexStart = indexEnd;
                indexEnd = (indexStart + 1) % points.Length;
            }
            currentIndex = indexStart;
            float param = segmentTime * durationInv;

            //setting gameObject position to the position from the algorithm below
            gameObject.transform.position = GetPosition(param, indexStart, indexEnd);

           

            
        }
        gameObject.transform.Rotate(0.0f, -rotateSpeed, 0.0f);

    }

    private void OnDrawGizmos() //in case we want to make the catmull rom curve show up on screen
    {
        Gizmos.color = Color.white;
      
        DisplayCatmullRom();        
    }

    
    void DisplayCatmullRom()
    {
        //this function draws the lines on screen for better visuals with the catmull rom
        Vector3 p0 = points[0].position;
        Vector3 p1 = points[1].position;
        Vector3 p2 = points[2].position;
        Vector3 p3 = points[3].position;
        lastPosition = p1; 

        //newPosition = GetPosition(p0, p1, p2, p3);

        for (int i = 0; i < maxSegmentAmount - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }

        //drawing the final connecting line since the for loop does not include it
        Gizmos.DrawLine(points[maxSegmentAmount - 1].position, points[0].position);
    }
    
    Vector3 GetPosition(float param, int start, int end)
    {
        //getting the number we need to for running modulus
        int num = points.Length;

        //setting up the previous, current, future and next points for the catmull process
        Vector3 p0 = points[Mod((start-1),num)].position;
        Vector3 p1 = points[start].position;
        Vector3 p2 = points[end].position;
        Vector3 p3 = points[Mod(end+1,num)].position;

        //time = time / 2; //this was to slow down the orbit for more viewability

        //this is our current algorithm to run catmull rom 
        Vector3 currentPosition = ((-0.5f * p0 + 1.5f * p1 - 1.5f * p2 + 0.5f * p3) * (param * param * param)
           + (1f * p0 - 2.5f * p1 + 2f * p2 - 0.5f * p3) * (param * param)
           + (-0.5f * p0 + 0.5f * p2) * param
           + 1f * p1);

        //old algorithm for catmull rom
        //Vector3 currentPosition = alpha * ((2 * p1) + (-p0 + p2) * time + (2 * p0 - 5 * p1 + 4 * p2 - p3)
        // * (time * time) + (-p0 + 3 * p1 - 3 * p2 + p3) * (time * time * time));

        //returning the new point for the planet to move to
        return currentPosition;
    }

    //our modulus function since % runs as a remainder not modulus in C#
    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}
