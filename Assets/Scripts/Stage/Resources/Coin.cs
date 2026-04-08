using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private bool isCollected = false;

    [Header("Audio")]
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            GameManager.Instance.CollectCoin(value);

            if (collectSound != null)
            {
                SoundManager.PlaySFXAtPoint(collectSound, Camera.main.transform.position);
            }

            Destroy(gameObject);
        }
    }
}