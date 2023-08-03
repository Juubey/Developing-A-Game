using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;
    public float expandAmount;
    public float expandSpeed;

    Vector3 m_startSize;
    Vector3 m_targetSize;
    float m_scrollAmount;

    void Start()
    {
        InitPulseEffectVariables();

    }

    void InitPulseEffectVariables()
    {
        m_startSize = transform.localScale;
        m_targetSize = m_startSize * expandAmount;
    }

    public void MakePlayerPulse() //Makes the Player have a pulsating VFX
    {
        m_scrollAmount += Time.deltaTime * expandSpeed;
        float _percent = expandCurve.Evaluate(m_scrollAmount);
        transform.localScale = Vector2.Lerp(m_startSize, m_targetSize, _percent);
    }

    public void ResetPlayerSize() // Used to reset the Player size after calling MakePlayerPulse
    {
        transform.localScale = m_startSize;
        m_scrollAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
