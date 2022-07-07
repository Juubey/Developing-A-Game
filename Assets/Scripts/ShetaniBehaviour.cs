using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShetaniBehaviour : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    public Movement side;
    public GameObject shetani;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (x > 0)
        {
            shetani.GetComponent<AstarSmoothFollow2>().staticOffset = false;
        }
        if (x < 0)
        {
            shetani.GetComponent<AstarSmoothFollow2>().staticOffset = true;
        }
    }
}
