using UnityEngine;
using System;
using CodeMonkey.Utils;

public class Tome : MonoBehaviour
{

    public InputData inputData;
    public LayerMask layerToCollideWith;
    public Rigidbody2D playerBody;
    public float moveSpeed = 5;
    Vector3 m_clickedPos;
    Vector3 m_releasedPos;
    Vector3 m_dir;
    Vector3 originalPos;

    
    [SerializeField]
    private Rigidbody2D m_rigid2D;
    
    [SerializeField] private TomeLogic tomeLogic;
    [SerializeField] private HitMarker hitMarker;
    public float offset;
    Camera m_cam;
    PlayerVFX m_playerVFX;
    float calcSpeed;
    public float defaultTrailTime = 0.5f;
    bool m_hitBlock;
    [Space]
    [Header("Damage Calc")]
    public bool attack;
    public Transform attackBox;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public float hitRadius = 0.5f;
    
    public GameObject impactEffect;
    public int damage = 30;
    public Transform firePoint;

    void Start()
    {
        GetComponent();
    }

    private void Awake() 
    { 
        originalPos = gameObject.transform.position;
    }

    void GetComponent()
    {
        m_rigid2D = GetComponentInParent<Rigidbody2D>();
        m_playerVFX = GetComponent<PlayerVFX>();
        m_cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        HandleMovement();
    }



    void HandleMovement()
    {
        //TO DO: Create a draw circle which limits the area in which a bullet can be spawned which is attached to player!!!

        if(inputData.isPressed == true)
        {
            if (tomeLogic.IsWithPlayer()){
            //m_rigid2D.bodyType = RigidbodyType2D.Dynamic;
            m_rigid2D.position = transform.position;
            Vector3 throwDir = m_dir.normalized;
            float throwForce = 100f;
            m_rigid2D.AddForce(throwDir * throwForce, ForceMode2D.Impulse);

            m_hitBlock = CheckIfHitObstacle(); // Will store our check

            if(CheckIfHitObstacle())
                return;

            m_clickedPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_clickedPos = new Vector3(m_clickedPos.x, m_clickedPos.y, 0f); 
            //resetPlayerPos();
            
            tomeLogic.ThrowShield(throwDir);

            m_playerVFX.SetDotStartPos(m_clickedPos);
            m_playerVFX.ChangeDotActiveState(true);
            m_playerVFX.ChangeTrailState(false, 0f);

            //Debug.Log(m_clickedPos);
            //TO DO: && is area around player
            } else {
                tomeLogic.Recall();
            }
        }

        if(inputData.isHeld == true)
        {
            //m_rigid2D.bodyType = RigidbodyType2D.Static;
            if(m_hitBlock) // Optimized Check
                return;
            m_playerVFX.SetDotPos(m_clickedPos, m_cam.ScreenToWorldPoint(Input.mousePosition));
            m_playerVFX.MakeProjectilePulse();
        }

        if(inputData.isReleased == true)
        {
            //m_rigid2D.bodyType = RigidbodyType2D.Dynamic;
            if(m_hitBlock) // Optimized Check
                return;
            m_releasedPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_releasedPos = new Vector3(m_releasedPos.x, m_releasedPos.y, 0f);
            //Debug.Log(m_releasedPos);

            m_playerVFX.ChangeDotActiveState(false);
            m_playerVFX.ResetProjectileSize();
            m_playerVFX.ChangeTrailState(true, defaultTrailTime);
            CalculateDirection();
            MovePlayerInDirection();
            GetPosition();
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, hitRadius/*, enemyLayers*/);

            foreach(Collider2D enemy in hitEnemies)
            {
                
                Debug.Log("We hit " + enemy.name);
                //hitMarker.GetComponent<HitMarker>().HitEnable();
            }
        } 
    }

    void CalculateDirection()
    {
        m_dir = (m_releasedPos - m_clickedPos).normalized;

    }

    void MovePlayerInDirection()
    {
        m_rigid2D.velocity = m_dir * (moveSpeed);

        calcSpeed = moveSpeed;
    }

    private void resetPlayerPos()
    {
        tomeLogic.transform.position = m_clickedPos; // position of mouse click
        m_rigid2D.velocity = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            Vector2 _wallNormal = other.contacts[0].normal; //saves the point of contact
            m_dir = Vector2.Reflect(m_rigid2D.velocity, _wallNormal); // reflects the object while keeping the velocity

            m_rigid2D.velocity = m_dir; // perfect speed *chef kiss*
        }

        

        if(other.gameObject.CompareTag("Enemy")) //if object hit has enemy tab show hitmarker
        {
            //Enemy enemy = other.GetComponent<Enemy>();
            //Debug.Log(other.name);
            
            //enemy.GetComponent<Enemy>().TakeDamage(damage);
            hitMarker.GetComponent<HitMarker>().HitEnable();
        }

        Instantiate(impactEffect, transform.position, transform.rotation);
        //Destroy(gameObject); //Destroys the projectile
    }

    bool CheckIfHitObstacle()
    {
        Ray _ray = m_cam.ScreenPointToRay(Input.mousePosition); // creates a ray
        RaycastHit2D _hitBlock = Physics2D.Raycast(_ray.origin, _ray.direction, 100f, layerToCollideWith);

        return _hitBlock; //Will return true or false, Thanks Unity!
    }

    bool CheckIfInsideBounds()//<------------FIXXXX
    {
        Ray _ray = m_cam.ScreenPointToRay(Input.mousePosition); // creates a ray
        RaycastHit2D _outsideBounds = Physics2D.Raycast(_ray.origin, _ray.direction, 100f, layerToCollideWith);//<------------FIXXXX

        return _outsideBounds; //Will return true or false
    }

    void OnBecameInvisible()
    {
        enabled = false;
        Destroy(gameObject);
    }

    public Vector3 GetPosition()
    {
        m_rigid2D.position = playerBody.position; 
        return m_rigid2D.position;
    }
    public int GetSortingOrder() {
        return transform.Find("Player").GetComponent<MeshRenderer>().sortingOrder;
    }
}
