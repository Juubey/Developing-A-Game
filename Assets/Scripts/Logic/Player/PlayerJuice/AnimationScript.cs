using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public ParticleSystem dustParticle;

    private Animator anim;
    private Movement move;
    
    private Collision coll;
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<Movement>();
        sr = GetComponent<SpriteRenderer>();

        dustParticle.Stop(true);
    }

    void Update()
    {
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("wallGrab", move.wallGrab);
        anim.SetBool("wallSlide", move.wallSlide);
        anim.SetBool("canMove", move.canMove);
        anim.SetBool("isDashing", move.isDashing);

    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {
        
        if (move.wallGrab || move.wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
            {
                return;

            }
        }
        if (move.canMove)
        {
            var _psm_emission = dustParticle.emission;

            if (side == -1 && sr.flipX && dustParticle.isEmitting == false)
            {
                dustParticle.Play(true);
                _psm_emission.enabled = true;
                StartCoroutine(StopDust());
            }

            if (side == 1 && !sr.flipX && dustParticle.isEmitting == false)
            {
                dustParticle.Play(true);
                _psm_emission.enabled = true;
                StartCoroutine(StopDust());
            }

            else
                StartCoroutine(StopDust());
        }

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
    IEnumerator StopDust()
    {
        // Stops particle effect
        yield return new WaitForSeconds(.1f);
        dustParticle.Stop();
    }
}
