using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public GameObject hitMarker;
    private Vector3 target;
    [SerializeField] private Tome tome;
    [SerializeField] private PlayerController PC;

    
    void Start()
    {
        hitMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
        hitMarker.transform.position = new Vector2(target.x, target.y);
    }

    public void HitEnable()
    {
        hitMarker.SetActive(true);
        Invoke("HitDisable", 0.1f);
        Debug.Log("HitMarker: True");
    }
    private void HitDisable()
    {
        hitMarker.SetActive(false);
        Debug.Log("HitMarker: False");
    }
}
