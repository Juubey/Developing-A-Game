using UnityEngine;

using TMPro;
using System;
using UnityEngine.UI;

[ExecuteAlways]
public class FPSDisplay : MonoBehaviour
{
    public float fps { get; private set; }      // Frames per second (interval average).
    public float frameMS { get; private set; }  // Milliseconds per frame (interval average).
    
    int framesPassed = 0;
    float minFPSValue = Mathf.Infinity;
    float maxFPSValue = 0f;


    GUIStyle style = new GUIStyle();

    public int size = 16;

    [Space]

    public Vector2 position = new Vector2(16.0f, 16.0f);

    public enum Alignment { Left, Right }
    public Alignment alignment = Alignment.Left;

    [Space]

    public Color colour = Color.green;

    [Space]

    public float updateInterval = 0.5f;

    float elapsedIntervalTime;
    int intervalFrameCount;

    [Space]

    [Tooltip("Optional. Will render using GUI if not assigned.")]
    public TextMeshProUGUI textMesh;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Get average FPS and frame delta (ms) for current interval (so far, if called early).
    public float GetIntervalFPS()
    {
        // 1 / time.unscaledDeltaTime for same-frame results.
        // Same as above, but uses accumulated frameCount and deltaTime.

        return intervalFrameCount / elapsedIntervalTime;
    }

    public float GetIntervalFrameMS()
    { 
        // Calculate average frame delta during interval (time / frames).
        // Same as Time.unscaledDeltaTime * 1000.0f, using accumulation.

        return (elapsedIntervalTime * 1000.0f) / intervalFrameCount;
    }

    void Update()
    {
        float fpsMinMax = 1 / Time.unscaledDeltaTime;

        intervalFrameCount++;
        elapsedIntervalTime += Time.unscaledDeltaTime;
        framesPassed++;

        if (elapsedIntervalTime >= updateInterval)
        {
            fps = GetIntervalFPS();
            frameMS = GetIntervalFrameMS();

            fps = (float)Math.Round(fps, 2);
            frameMS = (float)Math.Round(frameMS, 2);

            intervalFrameCount = 0;
            elapsedIntervalTime = 0.0f;
        }

        if (fpsMinMax < minFPSValue && framesPassed > 10)
        {
            minFPSValue = fpsMinMax;
        }

        if (fpsMinMax > maxFPSValue && framesPassed > 10)
        {
            maxFPSValue = fpsMinMax;

        }

        if (textMesh)
        {
            textMesh.text = GetFPSText();
        }
        else
        {
            style.fontSize = size;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = colour;
        }
    }

    string GetFPSText()
    {
        return $"FPS: {fps:.00} ({frameMS:.00} ms)" +
            $" {"Maximum: " + maxFPSValue}" +
            $" {"Minimum: " + minFPSValue}";
    }

    void OnGUI()
    {
        string fpsText = GetFPSText();

        if (!textMesh)
        {
            float x = position.x;

            if (alignment == Alignment.Right)
            {
                x = Screen.width - x - style.CalcSize(new GUIContent(fpsText)).x;
            }

            GUI.Label(new Rect(x, position.y, 200, 100), fpsText, style);
        }
    }
}