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
        Time.fixedDeltaTime = 0.02f;
        GameIsPaused = false;
    }

    void Pause (){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        GameIsPaused = true;
        Debug.Log("Escape key was pressed and the game is paused");
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // Load the Main Menu
        SceneManager.LoadSceneAsync(0);
        Debug.Log("Loading menu...");

    }

    public void QuitGame(){
        Debug.Log("Quitting game...");
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // quit the App
        Application.Quit();
    }
}
