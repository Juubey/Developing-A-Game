using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class levelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;

    public TMP_Text progressText;

    public Animator transition;
    public float transitionTime = 1f;

     public void PlayGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //Play animation
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load Scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone){

            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);
            
            loadingSlider.value = progress;
            progressText.text = progress.ToString("P#", System.Globalization.CultureInfo.InvariantCulture);

            yield return null;
        }
    }
}
