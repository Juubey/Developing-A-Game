using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dissolve : MonoBehaviour
{
    Material material;
    bool isDissolving = false;
    float fade = 1f;

    public Player Player;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.isAlive == false)
        {
            isDissolving = true;
        }
        if(isDissolving)
        {
            fade -= Time.deltaTime;
            if(fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
