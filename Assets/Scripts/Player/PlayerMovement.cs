using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    public AudioSource moveSound;
    public AudioSource landSound;

    private Vector2 touchStartPos;
    private bool isSwiping = false;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator anim;
    private SpriteRenderer sr;

    private bool isMoving = false;

    public Vector3 safePosition;
    public Quaternion safeRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;

        safePosition = transform.position;
        safeRotation = transform.rotation;
    }

    void Update()
    {
        if (isMoving) return;

        // ===== PC INPUT =====
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

        // ===== MOBILE TOUCH (OPTIMIZED) =====
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Bắt đầu chạm
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isSwiping = true;
            }

            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                Vector2 delta = touch.position - touchStartPos;

                if (delta.sqrMagnitude > 900f) 
                {
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                        movement = delta.x > 0 ? Vector2.right : Vector2.left;
                    else
                        movement = delta.y > 0 ? Vector2.up : Vector2.down;

                    StartMove();
                    isSwiping = false;
                }
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isSwiping = false;
            }
        }
    }

    void StartMove()
    {
        isMoving = true;

        if (!anim.GetBool("isMoving"))
            anim.SetBool("isMoving", true);

        if (moveSound && !moveSound.isPlaying)
            moveSound.Play();

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 targetVelocity = movement * speed;

            if (rb.velocity != targetVelocity)
                rb.velocity = targetVelocity;
        }
    }

    void StopMove(Vector2 normal)
    {
        isMoving = false;
        rb.velocity = Vector2.zero;

        if (anim.GetBool("isMoving"))
            anim.SetBool("isMoving", false);

        if (landSound)
            landSound.PlayOneShot(landSound.clip);

        transform.position += (Vector3)(normal * 0.1f);

        safePosition = transform.position;
        safeRotation = transform.rotation;

        Vector2 faceDir = -normal;
        float angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    public void ResetState()
    {
        isMoving = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (anim != null && anim.GetBool("isMoving"))
        {
            anim.SetBool("isMoving", false);
        }
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