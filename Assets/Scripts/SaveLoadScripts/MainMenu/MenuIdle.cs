using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuIdle : MonoBehaviour
{
    [SerializeField] string whichLevel;
    float currentTime = 0f;
    float startingTime = 30f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("IdleStart", 30f);
        currentTime = startingTime;
    }
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentTime = 30f;
            CancelInvoke("IdleStart");
        }
        else if( currentTime <= 0f)
        {
            Invoke("IdleStart", 0f);
        }
    }

    void IdleStart()
    {
        SceneManager.LoadScene(whichLevel);
    }
}
