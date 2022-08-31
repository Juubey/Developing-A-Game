using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileBehaviour : MonoBehaviour
{
    public LayerMask layerToCollideWith;
    public float moveSpeed = 5;
    Vector3 m_clickedPos;
    Vector3 m_releasedPos;
    Vector3 m_dir;

    //[SerializeField] Transform _FirePoint;

    [SerializeField]
    private Rigidbody2D m_rigid2D;
    public Transform Spear;
    public float offset;
    Camera m_cam;
    public Camera boundsCam;
    public float ProjectileForce;
    public float throwSpeedMult = 4.0f;
    PlayerVFX m_playerVFX;

    public float defaultTrailTime = 0.5f;
    bool m_hitBlock;
    bool hasHit;

    public static float slowdownFactor = 0.1f;
    public static float defaultTimeScale = 1f;
    public float timeScale = 0.1f;
    public float slowdownLength = 1f;

    void Start()
    {
        GetComponent();

    }

    void GetComponent()
    {
        m_rigid2D = GetComponent<Rigidbody2D>();
        m_playerVFX = GetComponent<PlayerVFX>();
        Spear = GetComponent<Transform>();

        //m_cam = FindObjectOfType<Camera>();
        m_cam = Camera.main;
        //boundsCam = GameObject.Find("BoundsCam").GetComponent<Camera>();
        boundsCam = GameObject.FindWithTag("BoundsCam").GetComponent<Camera>();
    }

    void Update()
    {
        HandleMovement();
        AttackSpeedGen(ProjectileForce);

        Time.timeScale += (1f / slowdownLength) * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * slowdownFactor;
    }

    public void UndoSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    void RotateToMousePosition()
    {
        Vector3 difference = m_cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
    }

    void HandleMovement()
    {
        //TO DO: Create a draw circle which limits the area in which a bullet can be spawned which is attached to player!!!

        //if (inputData.isPressed == true)
        if (Input.GetMouseButtonDown(0))
        {
            transform.parent = GameObject.FindWithTag("Holder").GetComponent<Transform>();
            m_hitBlock = CheckIfHitObstacle(); // Will store our check

            if (CheckIfHitObstacle())
                return;
            //Debug.Log(Input.mousePosition);
            //m_clickedPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_clickedPos = boundsCam.ScreenToWorldPoint(m_cam.ScreenToWorldPoint(Input.mousePosition)); //Can be done Better. TO:DO Set Bounds more accurately.
            m_clickedPos = new Vector3(m_clickedPos.x, m_clickedPos.y, 0f);
            RotateToMousePosition();


            DoSlowMotion();
            ResetPlayerPos();

            m_playerVFX.SetDotStartPos(m_clickedPos);
            m_playerVFX.ChangeDotActiveState(true);
            m_playerVFX.ChangeTrailState(false, 0f);

            //Debug.Log(m_clickedPos);
            //TO DO: && is area around player
        }

        //if (inputData.isHeld == true)
        if (Input.GetMouseButton(0))
        {
            m_rigid2D.bodyType = RigidbodyType2D.Static;
            if (m_hitBlock) // Optimized Check
                return;
            m_playerVFX.SetDotPos(m_clickedPos, m_cam.ScreenToWorldPoint(Input.mousePosition));
            m_playerVFX.MakeProjectilePulse();
            RotateToMousePosition();
        }

        //if (inputData.isReleased == true)
        if (Input.GetMouseButtonUp(0))
        {
            m_rigid2D.bodyType = RigidbodyType2D.Dynamic;
            if (m_hitBlock) // Optimized Check
                return;
            m_releasedPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_releasedPos = new Vector3(m_releasedPos.x, m_releasedPos.y, 0f);
            //Debug.Log(m_releasedPos);

            m_playerVFX.ChangeDotActiveState(false);
            m_playerVFX.ResetProjectileSize();
            m_playerVFX.ChangeTrailState(true, defaultTrailTime);
            CalculateDirection();
            MovePlayerInDirection();
            UndoSlowMotion();

            //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, hitRadius/*, enemyLayers*/);
            /*
            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);
            }
            */
        }
    }

    void CalculateDirection()
    {
        m_dir = (m_releasedPos - m_clickedPos).normalized;

    }

    void MovePlayerInDirection()
    {
        m_rigid2D.velocity = m_dir * (moveSpeed + AttackSpeedGen(ProjectileForce));
    }

    float AttackSpeedGen(float ProjectileForce)
    {
        if (ProjectileForce < 100)
        {
            ProjectileForce += Time.deltaTime * throwSpeedMult;
        }
        if (Input.GetMouseButtonUp(0))
        {
            ProjectileForce = 0f;
        }

        return (ProjectileForce);
    }

    void ResetPlayerPos()
    {
        Spear.transform.position = m_clickedPos; // position of mouse click
        m_rigid2D.velocity = Vector3.zero;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.name);

        if (other.gameObject.CompareTag("Obstacle"))
        {
            //If its a tome Reflect
            Vector2 _wallNormal = other.contacts[0].normal; //saves the point of contact
            m_dir = Vector2.Reflect(m_rigid2D.velocity, _wallNormal); // reflects the object while keeping the velocity
            
            /* If its a spear Stick
            m_rigid2D.velocity = m_dir; // perfect speed *chef kiss*
            hasHit = true;
            m_rigid2D.velocity = Vector2.zero;
            m_rigid2D.isKinematic = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            transform.parent = other.transform;
            */
            //Instantiate(impactEffect, transform.position, transform.rotation);
        }
        if (other.gameObject.CompareTag("Enemy")) //if object hit has enemy tab show hitmarker
        {
            //Enemy enemy = other.GetComponent<Enemy>();
            //enemy.GetComponent<Enemy>().TakeDamage(damage);

            m_rigid2D.velocity = m_dir; // perfect speed *chef kiss*
            hasHit = true;
            m_rigid2D.velocity = Vector2.zero;
            m_rigid2D.isKinematic = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            transform.parent = other.transform;
            //Instantiate(impactEffect, transform.position, transform.rotation);
        }
        if (other.gameObject.CompareTag("Player")) 
        {
            //gameObject.GetComponent<Collider2D>().enabled = false;
        }
        if (other.gameObject.CompareTag("HardSurface"))
        {
            //If its a tome Reflect
            Vector2 _wallNormal = other.contacts[0].normal; //saves the point of contact
            m_dir = Vector2.Reflect(m_rigid2D.velocity, _wallNormal); // reflects the object while keeping the velocity

            //Instantiate(impactEffect, transform.position, transform.rotation);
        }
        if (other.gameObject.CompareTag("SoftSurface"))
        {
            // If its a spear Stick
            m_rigid2D.velocity = m_dir; // perfect speed *chef kiss*
            hasHit = true;
            m_rigid2D.velocity = Vector2.zero;
            m_rigid2D.isKinematic = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            transform.parent = other.transform;
            //Instantiate(impactEffect, transform.position, transform.rotation);
        }
        else
        {
            //Destroy(Instantiate(impactEffect, transform.position, transform.rotation));
            //Destroy(gameObject); //Destroys the projectile
        }

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


}
