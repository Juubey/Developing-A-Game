/*
 * Movement.cs
 * Author(s): Albert Njubi
 * Based on code from Mix & Jam/André Cardoso https://github.com/mixandjam/Celeste-Movement
 * & TaroDev https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller
 * & Dawnosaur https://www.youtube.com/watch?v=KKGdDBFcu0Q
 * Date Created: 7/1/19
 */
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
/// <summary>
/// This class handles the GameObject player movement, 
/// including its animations and stats.
/// </summary>
public class Movement : MonoBehaviour, IDataPersistence/*, EnemyHandler.IEnemyTargetable*/
{
    #region public variables
    public static Movement instance;
    [HideInInspector] public Rigidbody2D rb;
    public AfterImage afterImage;
    public PlayerRunData Data;
    [Space]
    public int side = 1;
    #endregion

    #region Public Floats
    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    private float jumpTimer;
    public float groundedTimer = 0;
    #endregion

    #region Public Bools
    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    #endregion

    #region Public ParticleSystem
    [Space]
    [Header("Polish")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    public ParticleSystem dustParticle;
    public ParticleSystem PowerUpParticle;
    #endregion

    #region private variables
    private Collision coll;
    [HideInInspector] private AnimationScript anim;
    public BoxCollider2D hitbox;
    public GameObject shotPoint;

    [Space]
    private bool hasDashed;
    Playerinputs player;
    Vector2 move;
    Vector2 jump;
    Vector2 dash;

    private bool interactPressed = false;
    private bool submitPressed = false;

    [Header("Attributes SO")]
    [SerializeField] private AttributesScriptableObject playerAttributesSO;
    #endregion

    #region private functions
    /// <summary>
    /// The start method retrives the player components, 
    /// being the Collider, Rigidbody2D and 
    /// AnimationScript Class.
    /// </summary>
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }
    private void Awake()
    {
        player = new Playerinputs();

        if (instance != null)
        {
            Debug.LogError("Found more than one Movement Manager in the scene.");
        }
        instance = this;
    }

    public static Movement GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// This Update method handles the input, and boolen
    /// modifiers which determine how the player moves.
    /// BetterJumping class is referenced. 
    /// </summary>
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        move = player.Player.Move.ReadValue<Vector2>();
        jump = player.Player.Jump.ReadValue<Vector2>(); //TODO: Add input logic for jump
        dash = player.Player.Dash.ReadValue<Vector2>();//TODO: Add input logic for dash

        Vector2 dir = new Vector2(move.x + x, move.y + y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        jumpTimer -= Time.deltaTime;
        groundedTimer -= Time.deltaTime;

        var isGrounded = false;
        isGrounded |= coll.onGround;
        groundTouch = isGrounded;

        var isGroundedOnSpear = false;
        isGroundedOnSpear |= coll.onSpear;
        groundTouchSpear = isGroundedOnSpear;

        float vel = rb.velocity.x;

        if ((rb.velocity.x < -14 || rb.velocity.x > 14) && !isDashing)
        {
            afterImage.makeAfterImage = true;
        }
        else
        {
            afterImage.makeAfterImage = false;
        }

        if (coll.onWall && Keyboard.current.ctrlKey.wasPressedThisFrame && canMove)
        {
            if (side != coll.wallSide)
                anim.Flip(side * -1);

            wallGrab = true;
            wallSlide = false;
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame) //Jump
        {
            anim.SetTrigger("jump");
            jumpTimer = 0.2f;

            if (groundTouch || groundTouchSpear && jumpTimer > 0)

                jump = Jump(Vector2.up, false);
            jumpTimer = 0;
            groundedTimer = 0;

            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
            anim.SetTrigger("land");
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (coll.onSpear && !groundTouchSpear)
        {
            GroundTouchSpear();
            groundTouchSpear = true;
            anim.SetTrigger("land");
            //anim.SetTrigger("wobble"); --------------------------TODO
        }

        if (!coll.onSpear && groundTouchSpear)
        {
            groundTouchSpear = false;
        }

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;


        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
            //CreateDust();
            hitbox.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            shotPoint.transform.localPosition = new Vector3(2, 0, 0);

        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
            //CreateDust();
            hitbox.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            shotPoint.transform.localPosition = new Vector3(-2, 0, 0);
        }

        if (side == 1 || side == -1)
        {
            bool flipped;
            if (rb.velocity.x < -14 || rb.velocity.x > 14)
            {
                flipped = true;
                if (flipped == true)
                    CreateDust();
                return;
            }
            else
                flipped = false;
        }
        
        if(DialogueManager.GetInstance().dialogueIsPlaying)
        {
            vel = 0;
            canMove = false;
        }

    }

    /// <summary>
    /// Plays the jumpParticle if able to jump
    /// </summary>
    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    void GroundTouchSpear()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    void CreateDust()
    {
        dustParticle.Play();
    }

    /// <summary>
    /// Plays the RippleEffect and Dash animation,
    /// then calls coroutine until the next dash is ready. 
    /// </summary>
    private void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    /// <summary>
    /// Flips the player sprite then starts a coroutine
    /// that disables movement for a brief moment until
    /// the player can jump again.
    /// </summary>
    private void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump(Vector2.up / 1.5f + wallDir / 1.5f, true);

        wallJumped = true;
    }

    /// <summary>
    /// Flips the player sprite based on the side the wall is on,
    /// then returns if the player cannot move.
    /// If the players moving horizontally then the player is 
    /// pushing a wall
    /// </summary>
    private void WallSlide()
    {
        if (coll.wallSide != side)
            anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    /// <summary>
    /// This function checks if the player is able to move
    /// and sets the rigidbody velocity.
    /// </summary>
    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            //rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
            //Calculate the direction we want to move in and our desired velocity
            //float targetSpeed = move.x * Data.runMaxSpeed;
            float targetSpeed = dir.x * Data.runMaxSpeed;
            #region Calculate AccelRate
            float accelRate;

            //Gets an acceleration value based on if we are accelerating (includes turning) 
            //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
            if (groundedTimer > 0)
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
            else
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
            #endregion

            #region Conserve Momentum
            //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
            if (Data.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && groundedTimer < 0)
            {
                //Prevent any deceleration from happening, or in other words conserve are current momentum
                //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
                accelRate = 0;
            }
            #endregion

            //Calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - rb.velocity.x;
            //Calculate force along x-axis to apply to thr player

            float movement = speedDif * accelRate;

            //Convert this to a vector and apply to rigidbody
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    /// <summary>
    /// This function modifies the players verticle velocity.
    /// Then plays the jump particle effect.
    /// </summary>
    private Vector2 Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
        particle.Play();
        return rb.velocity;
    }


    /// <summary>
    /// Gets the coefficient of the rigidbodys drag
    /// </summary>
    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

    /// <summary>
    /// Plays the slide particle if the player is on a wall
    /// and sliding.
    /// </summary>
    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    /// <summary>
    /// Returns which side the particle should be on.
    /// </summary>
    int ParticleSide()
    {
        int particleSide = coll.onRightWall ? 1 : -1;
        return particleSide;
    }
    #endregion

    #region public functions
    /// <summary>
    /// Sets a timer for how long the player has touched
    /// the ground and returns that variable.
    /// </summary
    public bool groundTouch
    {
        get
        {
            return groundedTimer > 0;
        }
        set
        {
            groundedTimer = value ? 10f : groundedTimer;
        }
    }
    public bool groundTouchSpear
    {
        get
        {
            return groundedTimer > 0;
        }
        set
        {
            groundedTimer = value ? 10f : groundedTimer;
        }
    }
    #endregion

    #region coroutines
    /// <summary>
    /// This coroutine starts a timer after the player has dashed.
    /// During this time the players gravity is modified.
    /// </summary
    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    /// <summary>
    /// Returns a float for a waiting time until
    /// the player can ground dash again.
    /// </summary
    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.onGround)
            hasDashed = false;
    }
    IEnumerator GroundDust()
    {
        yield return new WaitForSeconds(.15f);
    }

    /// <summary>
    /// Disables player movement.
    /// </summary
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    #endregion

    public void LoadData(GameData data)
    {
        //this.transform.position = data.playerPosition;
        // load the values from our game data into the scriptable object
        playerAttributesSO.WrathStr = data.playerAttributesData.WrathStr;
        playerAttributesSO.PrideAgi = data.playerAttributesData.PrideAgi;
        playerAttributesSO.GreedLuck = data.playerAttributesData.GreedLuck;
        playerAttributesSO.LustChar = data.playerAttributesData.LustChar;
        playerAttributesSO.EnvyPer = data.playerAttributesData.EnvyPer;
        playerAttributesSO.GluttonyInt = data.playerAttributesData.GluttonyInt;
        playerAttributesSO.SlothEnd = data.playerAttributesData.SlothEnd;
        playerAttributesSO.VaingloryRand = data.playerAttributesData.VaingloryRand;
        playerAttributesSO.IngratitudeNeg = data.playerAttributesData.IngratitudeNeg;
    }

    public void SaveData(GameData data)
    {
        //data.playerPosition = this.transform.position;
        // store the values from our scriptable object into the game data
        data.playerAttributesData.WrathStr = playerAttributesSO.WrathStr;
        data.playerAttributesData.PrideAgi = playerAttributesSO.PrideAgi;
        data.playerAttributesData.GreedLuck = playerAttributesSO.GreedLuck;
        data.playerAttributesData.LustChar = playerAttributesSO.LustChar;
        data.playerAttributesData.EnvyPer = playerAttributesSO.EnvyPer;
        data.playerAttributesData.GluttonyInt = playerAttributesSO.GluttonyInt;
        data.playerAttributesData.SlothEnd = playerAttributesSO.SlothEnd;
        data.playerAttributesData.VaingloryRand = playerAttributesSO.VaingloryRand;
        data.playerAttributesData.IngratitudeNeg = playerAttributesSO.IngratitudeNeg;
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
}
