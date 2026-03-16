using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Nhận input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animation
        if (anim != null)
        {
            anim.SetBool("isMoving", movement != Vector2.zero);
        }

        // Flip sprite khi đi trái phải
        if (movement.x != 0)
        {
            sr.flipX = movement.x < 0;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }
}