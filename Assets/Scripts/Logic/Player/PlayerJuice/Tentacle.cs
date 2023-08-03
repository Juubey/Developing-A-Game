using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targerDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;


    private void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    private void Update()
    {

        segmentPoses[0] = targerDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targerDir.right * targetDist, ref segmentV[i], smoothSpeed + i / trailSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}
