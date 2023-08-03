using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSAverage : MonoBehaviour
{
    // Display UI
    [SerializeField]
    TMP_Text currentFPS;
    [SerializeField]
    TMP_Text averageFPS;
    [SerializeField]
    TMP_Text maxFPS;
    [SerializeField]
    TMP_Text minFPS;
    [SerializeField]
    TMP_Text debugFPS;

    int framesPassed = 0;
    float fpsTotal = 0f;
    float minFPSValue = Mathf.Infinity;
    float maxFPSValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // Current FPS value
        float fps = 1 / Time.unscaledDeltaTime;
        currentFPS.text = "Cur. FPS: " + (int)fps;

        // Current FPS value
        float fps2 = 1 / Time.unscaledDeltaTime;
        debugFPS.text = "Deb. FPS: " + (int)fps;

        // Calculate average
        fpsTotal += fps;
        framesPassed++;
        averageFPS.text = "Ave. FPS: " + (int)(fpsTotal / framesPassed);

        // Max FPS
        if (fps > maxFPSValue && framesPassed > 10)
        {
            maxFPSValue = fps;
            maxFPS.text = "Max FPS: " + (int)maxFPSValue;
        }

        // Min FPS
        if (fps < minFPSValue && framesPassed > 10)
        {
            minFPSValue = fps;
            minFPS.text = "Min. FPS: " + (int)minFPSValue;
        }
    }
}