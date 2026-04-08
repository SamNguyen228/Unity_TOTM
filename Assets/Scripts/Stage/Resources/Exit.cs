using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool isTriggered = false;

    [Header("Audio")]
    public AudioClip finishSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;

            if (finishSound != null)
            {
                SoundManager.PlaySFXAtPoint(finishSound, Camera.main.transform.position);
            }

            GameManager.Instance.FinishLevel();
        }
    }
}