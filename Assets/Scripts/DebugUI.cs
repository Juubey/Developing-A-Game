using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text text;
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }
    
    private float CalculateFps() 
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray) {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }
    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        text.text = Mathf.RoundToInt(CalculateFps()).ToString();
    }
}
