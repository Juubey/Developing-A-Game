using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabObject : MonoBehaviour
{

    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private GameObject grabbedObject;
    private int layerIndex;
    public PointEffector2D pointEffector2D;

    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Tome & Spear");
        pointEffector2D = GetComponent<PointEffector2D>();
        pointEffector2D.enabled = !pointEffector2D.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);

        if (hitInfo.collider!=null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            //GrabObject
            if (Keyboard.current.eKey.wasPressedThisFrame && grabbedObject == null)
            {
                if(pointEffector2D.enabled == false)
                    pointEffector2D.enabled = !pointEffector2D.enabled;
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
                Debug.Log("Trying to grab!");
            }
            //release object
            else if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (pointEffector2D.enabled == true)
                    pointEffector2D.enabled = !pointEffector2D.enabled;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.transform.SetParent(null);
                grabbedObject = null;
                Debug.Log("Releasing!");
            }
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }
}
