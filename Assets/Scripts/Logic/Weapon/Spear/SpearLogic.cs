using UnityEngine;

public class SpearLogic : MonoBehaviour
{
    [SerializeField] private Spear Spear;
    private State state;
    private Rigidbody2D t_rigidb2d;
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;
    private const float GRAB_DISTANCE = 20F;



    private enum State
    {
        withPlayer,
        thrown,
        Recalling,
    }
    private void Awake()
    {
        t_rigidb2d = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = State.Recalling;
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.thrown:
                TryGrabTome();
                t_rigidb2d.isKinematic = false;
                break;
            case State.Recalling:
                Vector3 dirToPlayer = (Spear.GetPosition() - transform.position).normalized;
                float recallSpeed = 20f;
                t_rigidb2d.velocity = dirToPlayer * recallSpeed;
                t_rigidb2d.isKinematic = true;
                TryGrabTome();

                break;
        }
    }

    private void lateUpdate()
    {
        switch (state)
        {
            case State.withPlayer:
                transform.position = Spear.GetPosition();
                if (spriteRenderer.sortingOrder >= 0)
                {
                    spriteRenderer.sortingOrder = Spear.GetSortingOrder() - 10;
                }
                else
                {
                    spriteRenderer.sortingOrder = Spear.GetSortingOrder() + 10;
                }

                break;
        }
    }
    private void TryGrabTome()
    {
        //float grabDistance = 5f;
        if (Vector3.Distance(transform.position, Spear.GetPosition()) <= GRAB_DISTANCE)
        {
            state = State.withPlayer;
            //TrailRenderer.enabled = false;
            t_rigidb2d.velocity = Vector2.zero;
            t_rigidb2d.isKinematic = true;
            //t_rigidb2d.bodyType = RigidbodyType2D.isKinematic;
        }
    }
    public void ThrowShield(Vector3 throwDir)
    {
        transform.position = Spear.GetPosition() /*+ (throwDir * GRAB_DISTANCE)*/; //Broken???
        float throwForce = 100f;
        t_rigidb2d.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
        //t_rigidb2d.isKinematic = false;
        t_rigidb2d.bodyType = RigidbodyType2D.Dynamic;
        //TrailRenderer.enabled = true;
        state = State.thrown;
    }

    public void Recall()
    {
        state = State.Recalling;
    }

    public bool IsWithPlayer()
    {
        return state == State.withPlayer;
    }

}
