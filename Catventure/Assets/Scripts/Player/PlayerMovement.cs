using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Max speed, in units per second, that the character moves.")]
    public float speed = 9;

    [Tooltip("Acceleration while grounded.")]
    public float walkAcceleration = 75;

    [Tooltip("Acceleration while in the air.")]
    public float airAcceleration = 30;

    [Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    public float groundDeceleration = 70;

    [Tooltip("Max height the character will jump regardless of gravity")]
    public float jumpHeight = 2.5F;

    private BoxCollider2D boxCollider;

    public Vector2 velocity;

    public bool grounded;

    public Animator anim;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
        {
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.ResetTrigger("Walk");
        }
        if (grounded)
        {
            velocity.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("Jump");

                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.ResetTrigger("Jump");
            }
        }

        float acceleration = grounded ? walkAcceleration : airAcceleration;
        float deceleration = grounded ? groundDeceleration : 0;

        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);

        grounded = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider || hit.isTrigger)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    grounded = true;
                }
            }
        }
    }
}