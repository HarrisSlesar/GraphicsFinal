using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRom : MonoBehaviour
{
    public Transform[] points;
    public bool looping = true;
    public float alpha = 0.5f;

    Vector3 lastPosition;
    Vector3 newPosition;
    public int segment = 0;
    int maxSegmentAmount = 4; //maybe this should be 3?
    public float time = 0.0f;
    float maxTime = 2.0f;
    float rotateVal = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (segment < maxSegmentAmount)
        {
            time += Time.deltaTime;

            lastPosition = newPosition;
            newPosition = GetPosition(time, segment);

            gameObject.transform.position = newPosition;

            if (time >= maxTime)
            {
                segment++;
                time = 0.0f;
            }
        }

        if (segment == maxSegmentAmount)
        {
            segment = 0;
        }

        gameObject.transform.Rotate(0.0f, rotateVal + 0.25f, 0.0f);

        if (rotateVal >= 360f)
        {
            rotateVal = 0.0f;
        }
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

        //Gizmos.DrawLine(lastPosition, newPosition);
        Gizmos.DrawLine(points[0].position, points[1].position);
        Gizmos.DrawLine(points[1].position, points[2].position);
        Gizmos.DrawLine(points[2].position, points[3].position);
        Gizmos.DrawLine(points[3].position, points[0].position);
        
        //lastPosition = newPosition;
    }

    Vector3 GetPosition(float time, int segment)
    {
        Vector3 p0 = points[segment % maxSegmentAmount].position;
        Vector3 p1 = points[(segment + 1) % maxSegmentAmount].position;
        Vector3 p2 = points[(segment + 2) % maxSegmentAmount].position;
        Vector3 p3 = points[(segment + 3) % maxSegmentAmount].position;
        //Vector3 tmp1, tmp2, tmp3, tmp4;

        time = time / 2;

        //float t0 = 0.0f;
        //float t1 = t0 + Mathf.Pow(p0.magnitude - p1.magnitude, alpha);
        //float t2 = t1 + Mathf.Pow(p1.magnitude - p2.magnitude, alpha);
        //float t3 = t2 + Mathf.Pow(p2.magnitude - p3.magnitude, alpha);

        //tmp1 = p0 * time * time * time;

        //Vector3 currentPosition = ((-0.5f * p0 + 1.5f * p1 - 1.5f * p2 + 0.5f * p3) * (time * time * time)
        //     + (1f * p0 - 2.5f * p1 + 2f * p2 - 0.5f * p3) * (time * time)
        //     + (-0.5f * p0 + 0.5f * p2) * time
        //     + 1f * p1);

        Vector3 currentPosition = alpha * ((2 * p1) + (-p0 + p2) * time + (2 * p0 - 5 * p1 + 4 * p2 - p3)
            * (time * time) + (-p0 + 3 * p1 - 3 * p2 + p3) * (time * time * time));

        //q(t) = alpha * ((2*P1) + (-P0+P2) * t + (2*P0–5*P1 + 4*P2 — P3) * t² + (-P0 + 3*P1–3*P2 + P3) * t³))

        return currentPosition;
    }
}
