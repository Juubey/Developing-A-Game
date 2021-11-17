using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{

    public GameObject dotPrefab;
    public int dotAmount;
    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;
    public float followSpeed;
    private float m_dotGap;
    private GameObject[] m_dotArray;
    TrailRenderer m_trailRenderer;

    [Space]
    [Header("Pulse Variables")]
    public AnimationCurve expandCurve;
    public float expandAmount;
    public float expandSpeed;

    Vector3 m_startSize;
    Vector3 m_targetSize;
    float m_scrollAmount;


    // Start is called before the first frame update
    void Start()
    {
        m_dotGap = 1f / dotAmount; //percentage of one dot relative to whole
        //Debug.Log(m_dotGap);

        GetComponents();

        SpawnDots();
        InitPulseEffectVariables();
    }

    void GetComponents()
    {
        m_trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    void InitPulseEffectVariables()
    {
        m_startSize = transform.localScale;
        m_targetSize = m_startSize * expandAmount;
    }


    void SpawnDots()
    {
        m_dotArray = new GameObject[dotAmount];

        for(int i = 0; i < dotAmount; i++)
        {
            GameObject _dot = Instantiate(dotPrefab);
            _dot.SetActive(false);
            m_dotArray[i] = _dot;
        }
    }

    public void SetDotPos(Vector3 startPos, Vector3 endPos)
    {
        for(int i = 0; i < dotAmount; i++)
        {
            //Vector3 _dotPos = m_dotArray[i].transform.position; //unnessesary
            Vector3 _dotPos = m_dotArray[i].transform.position;
            Vector3 _targetPos = Vector2.Lerp(startPos, endPos, /*(i + 1)*/ i * m_dotGap);

            float _smoothSpeed = (1f - followCurve.Evaluate(i * m_dotGap)) * followSpeed;

            //m_dotArray[i].transform.position = _targetPos;
            m_dotArray[i].transform.position = Vector2.Lerp(_dotPos, _targetPos, _smoothSpeed * Time.deltaTime);
        }
    }

    public void ChangeDotActiveState(bool state)
    {
        for(int i = 0; i < dotAmount; i++)
        {
            m_dotArray[i].SetActive(state);
        }
    }

    public void SetDotStartPos(Vector3 pos)
    {
        for (int i = 0; i < dotAmount; i++)
        {
            m_dotArray[i].transform.position = pos;
        }
    }

    public void MakeProjectilePulse() //Makes the Projectile have a pulsating VFX
    {
        m_scrollAmount += Time.deltaTime * expandSpeed;
        float _percent = expandCurve.Evaluate(m_scrollAmount);
        transform.localScale = Vector2.Lerp(m_startSize, m_targetSize, _percent);
    }

    public void ResetProjectileSize() // Used to reset the Projectiles size after calling MakeProjectilesPulse
    {
        transform.localScale = m_startSize;
        m_scrollAmount = 0f;
    }

    public void ChangeTrailState(bool emitting, float time)
    {
        m_trailRenderer.emitting = emitting;
        m_trailRenderer.time = time;
    }
}
