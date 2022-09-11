using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient lightColor;
    [SerializeField] private Light2D light;
    [SerializeField] private bool set10xSpeed = false;

    public TextMeshProUGUI timeDisplay;
    private int days;
    public int Days => days;
    private float time = 50;
    private bool canChangeDay = true;
    public delegate void OnDayChanged();

    public OnDayChanged DayChanged;

    public bool activateLights; // checks if lights are on
    public GameObject[] lights; // all the lights we want on when its dark
    public GameObject[] dayLights; // all the lights we want on when its day
    public SpriteRenderer[] stars; // star sprites 

    void Awake(){
        lights = GameObject.FindGameObjectsWithTag("Lights");
        dayLights = GameObject.FindGameObjectsWithTag("DayTimeLights");
    }

    void Update()
    {
        CalcTime();
        DisplayTime();

        if (set10xSpeed == true)
            Time.timeScale = 10;
        if (set10xSpeed == false)
            Time.timeScale = 1;

        //Debug.Log(time);
    }

    public void CalcTime()
    {
        if (time > 500)
        {
            time = 0;
        }

        if ((int)time == 250 && canChangeDay)
        {
            canChangeDay = false;
            //DayChanged();
            days++;
        }
        if ((int)time == 255)
            canChangeDay = true;

        time += Time.deltaTime;
        light.GetComponent<Light2D>().color = lightColor.Evaluate(time * 0.002f);


        if (time >= 210 && time < 220) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
           for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, (float)time / 60); // change the alpha value of the stars so they become visible
            }

            if (activateLights == false) // if lights havent been turned on
            {

                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true); // turn them all on
                    }
                    activateLights = true;
                
            }
        }

        if (time >= 0 && time < 10) // 19:00 / 7pm    -   until 11:00 / 11am 
        {
            for (int i = 0; i < dayLights.Length; i++)
            {
                dayLights[i].SetActive(false); // off
            }
        }

        if (time >= 310 && time < 320) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1 - (float)time / 60); // make stars invisible
            }
            if (activateLights == true) // if lights are on
            {

                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].SetActive(false); // shut them off
                }
                activateLights = false;
               
            }
        }
        if (time >= 380 && time < 390) // 12:00 / 12pm    -   until 18:00 / 6pm
        {
            for (int i = 0; i < dayLights.Length; i++)
            {
                dayLights[i].SetActive(true); // on
            }
        }
    }

    public void DisplayTime() // Shows time and day in ui
    {

        //timeDisplay.text = string.Format("{0:00}:{1:00}", time/*hours, mins*/); // The formatting ensures that there will always be 0's in empty spaces

    }
}
