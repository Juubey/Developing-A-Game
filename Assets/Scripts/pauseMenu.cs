using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class pauseMenu : MonoBehaviour
{
   public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    

    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }   
    }

    public void Resume (){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause (){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Debug.Log("Escape key was pressed and the game is paused");
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Debug.Log("Loading menu...");
    }

    public void QuitGame(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
