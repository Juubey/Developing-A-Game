using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicatorScript : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    new Camera camera;
    LineRenderer lr;
    Vector3 camOffset = new Vector3( 0, 0, 10);

    [SerializeField] AnimationCurve animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (lr == null)
            {
                lr = gameObject.GetComponent<LineRenderer>();
            }
            lr.enabled = true;
            lr.positionCount = 2;
            startPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(0, startPos);
            lr.useWorldSpace = true;
            lr.widthCurve = animationCurve;
            lr.numCapVertices = 10;
        }
        if(Input.GetMouseButton(0))
        {
            endPos = camera.ScreenToWorldPoint(Input.mousePosition) + camOffset;
            lr.SetPosition(1, endPos);
        }
        if(Input.GetMouseButtonUp(0))
        {
            lr.enabled = false;
        }
    }
}
