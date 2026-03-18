using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public AudioSource moveSound;
    public AudioSource landSound;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator anim;
    private SpriteRenderer sr;

    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement = Vector2.left;
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement = Vector2.right;
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement = Vector2.up;
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement = Vector2.down;
            StartMove();
        }
    }

    void StartMove()
    {
        isMoving = true;
        anim.SetBool("isMoving", true);

        // 🔊 sound di chuyển
        if (moveSound)
            moveSound.Play();

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = movement * speed;
        }
    }

    void StopMove(Vector2 normal)
    {
        isMoving = false;
        rb.velocity = Vector2.zero;

        anim.SetBool("isMoving", false);

        // 🔊 sound đáp đất
        if (landSound)
            landSound.Play();

        // đẩy player ra khỏi collider
        transform.position += (Vector3)(normal * 0.1f);

        Vector2 faceDir = -normal;
        float angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMoving) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            PlayerDeath death = GetComponent<PlayerDeath>();
            if (death != null)
            {
                death.Die();
            }
            return;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            StopMove(normal);
        }
    }
}