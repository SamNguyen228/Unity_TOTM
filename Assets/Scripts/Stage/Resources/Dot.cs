using UnityEngine;

public class Dot : MonoBehaviour
{
    private bool isCollected = false;

    [Header("Audio")]
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            GameManager.Instance.CollectDot();

            if (collectSound != null)
            {
                SoundManager.PlaySFXAtPoint(collectSound, Camera.main.transform.position);
            }

            Destroy(gameObject);
        }
    }
}