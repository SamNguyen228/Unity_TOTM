using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public Animator anim;
    public AudioSource deathSound;
    public float popupDelay = 0.8f;
    private bool isDead = false;
    private Vector3 revivePosition;
    private Quaternion reviveRotation;

    public void Die()
    {
        if (isDead) return;

        if (TryConsumeShield())
            return;

        isDead = true;

        if (deathSound != null)
            deathSound.Play();

        transform.rotation = Quaternion.Euler(0, 0, 0);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        var move = GetComponent<PlayerMovement>();
        if (move != null)
        {
            revivePosition = move.safePosition;
            reviveRotation = move.safeRotation;
            move.enabled = false;
        }

        if (anim != null)
            anim.SetTrigger("Death");

        GameManager.Instance.ShowRevivePopup(this);
    }

    public void Revive()
    {
        isDead = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        transform.position = revivePosition;
        transform.rotation = reviveRotation * Quaternion.Euler(0, 0, 180f);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }

        var control = GetComponent<PlayerMovement>();
        if (control != null)
        {
            control.enabled = true;
            control.ResetState();
        }
        if (anim != null)
        {
            anim.Play("Idle");
        }

        Time.timeScale = 1f;

        StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.1f);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }

    bool TryConsumeShield()
    {
        int shield = PlayerData.GetShield();
        if (shield <= 0)
            return false;

        PlayerData.UseShield(1);

        if (ShieldUI.Instance != null)
            ShieldUI.Instance.UpdateUI();

        return true;
    }
}
