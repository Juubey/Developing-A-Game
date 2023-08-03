using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    [SerializeField] private GameObject ghost;
    public bool makeAfterImage = false;
    public SpriteRenderer VisualRef;

    public static AfterImage instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amoutToPool = 4;
    [SerializeField] private GameObject parent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;

        for (int i = 0; i < amoutToPool; i++)
        {
            GameObject obj1 = Instantiate(ghost, transform.position, transform.rotation);

            obj1.SetActive(false);
            pooledObjects.Add(obj1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        if (makeAfterImage)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                for (int i = 0; i < amoutToPool; i++)
                {
                    //Generate a ghost
                    //GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                    GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation );
                    Sprite currentSprite = VisualRef.GetComponent<SpriteRenderer>().sprite;
                    //currentGhost.transform.localScale = this.transform.localScale;
                    currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                    if (x > 0)
                        currentGhost.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    if (x < 0)
                        currentGhost.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

                    ghostDelaySeconds = ghostDelay;
                    
                    currentGhost.SetActive(true);
                    pooledObjects.Add(currentGhost);
                    Destroy(currentGhost, 1f);
                }
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

}
