using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineOfSight : MonoBehaviour
{
    public float rotationSpeed;
    public float distance;
    public LineRenderer sightLine;
    public Gradient redColor;
    public Gradient greenColor;

    void Start(){
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);
        if(hitInfo.collider != null){
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            sightLine.SetPosition(1, hitInfo.point);
            sightLine.colorGradient = greenColor;

            if(hitInfo.collider.CompareTag("Player")){
               Debug.Log("Spotted Player = " + hitInfo.collider.CompareTag("Player"));
               sightLine.colorGradient = redColor; 
            }
        }else {
            Debug.DrawLine(transform.position, transform.position + transform.right * distance, Color.green);
            sightLine.SetPosition(1, transform.position + transform.right * distance);
            sightLine.colorGradient = greenColor;

        }

        sightLine.SetPosition(0, transform.position);

    }
}
