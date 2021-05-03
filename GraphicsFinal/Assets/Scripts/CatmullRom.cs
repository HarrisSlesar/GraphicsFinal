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
            durationInv = 1 / segmentDuration;
            int indexStart = currentIndex;
            int indexEnd = (indexStart + 1) % points.Length;
            segmentTime += dt;

            if (segmentTime >= segmentDuration)
            {
                segmentTime -= segmentDuration;
                indexStart = indexEnd;
                indexEnd = (indexStart + 1) % points.Length;
            }
            currentIndex = indexStart;
            float param = segmentTime * durationInv;

            gameObject.transform.position = GetPosition(param, indexStart, indexEnd);

           

            
        }
        gameObject.transform.Rotate(0.0f, -rotateSpeed, 0.0f);

    }

    private void OnDrawGizmos() //in case we want to make the catmull rom curve show up on screen
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < points.Length; i++)
        {
            DisplayCatmullRom(i);
        }
    }

    
    void DisplayCatmullRom(int val)
    {
        Vector3 p0 = points[0].position;
        Vector3 p1 = points[1].position;
        Vector3 p2 = points[2].position;
        Vector3 p3 = points[3].position;
        lastPosition = p1; //p0 is sort of the starting previous position? idk

        //newPosition = GetPosition(p0, p1, p2, p3);

        //Gizmos.DrawLine(points[0].position, points[1].position);
        //Gizmos.DrawLine(points[1].position, points[2].position);
        //Gizmos.DrawLine(points[2].position, points[3].position);
        //Gizmos.DrawLine(points[3].position, points[0].position);

        for (int i = 0; i < maxSegmentAmount - 1; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }

        Gizmos.DrawLine(points[maxSegmentAmount - 1].position, points[0].position);

        //lastPosition = newPosition;
    }
    
    Vector3 GetPosition(float param, int start, int end)
    {
        int num = points.Length;


        Vector3 p0 = points[Mod((start-1),num)].position;
        Vector3 p1 = points[start].position;
        Vector3 p2 = points[end].position;
        Vector3 p3 = points[Mod(end+1,num)].position;

        //time = time / 2; //this was to slow down the orbit for more viewability

        //float t0 = 0.0f;
        //float t1 = t0 + Mathf.Pow(p0.magnitude - p1.magnitude, alpha);
        //float t2 = t1 + Mathf.Pow(p1.magnitude - p2.magnitude, alpha);
        //float t3 = t2 + Mathf.Pow(p2.magnitude - p3.magnitude, alpha);

        //tmp1 = p0 * time * time * time;

        Vector3 currentPosition = ((-0.5f * p0 + 1.5f * p1 - 1.5f * p2 + 0.5f * p3) * (param * param * param)
           + (1f * p0 - 2.5f * p1 + 2f * p2 - 0.5f * p3) * (param * param)
           + (-0.5f * p0 + 0.5f * p2) * param
           + 1f * p1); //this was the old algorithm I was testing

        //Vector3 currentPosition = alpha * ((2 * p1) + (-p0 + p2) * time + (2 * p0 - 5 * p1 + 4 * p2 - p3)
         // * (time * time) + (-p0 + 3 * p1 - 3 * p2 + p3) * (time * time * time)); //current algorithm for catmull rom

        return currentPosition;
    }


    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }
}
