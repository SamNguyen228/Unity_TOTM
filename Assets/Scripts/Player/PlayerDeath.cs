using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // public GameObject deathEffect;
    public Animator anim;
    public AudioSource deathSound;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null)
        {
            deathSound.Play();
        }

        // reset rotation nằm ngang
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        var control = GetComponent<PlayerMovement>();
        if (control != null)
            control.enabled = false;
        // if (deathEffect != null)
        // {
        //     Instantiate(deathEffect, transform.position, Quaternion.identity);
        // }

        if (anim != null)
        {
            anim.SetTrigger("Death");
        }

        Destroy(gameObject, 1f);
    }
}