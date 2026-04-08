using UnityEngine;
using System.Collections;

public class SpinWheelController : MonoBehaviour
{
    [Header("References")]
    public Transform wheel;
    public Transform pointer;

    [Header("Config")]
    public int segmentCount = 8;
    public float spinTime = 3f;

    [Header("Rewards")]
    public WheelReward[] rewards;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip spinSound;
    public AudioClip tickSound;
    public AudioClip winSound;

    private float currentAngle = 0f;
    private bool isSpinning = false;
    public RewardPopupUI rewardPopup;
    private int lastSegment = -1;

    // ===== BUTTON CLICK =====
    public void Spin()
    {
        if (isSpinning) return;

        StartCoroutine(SpinRoutine());
    }

    // ===== SPIN LOGIC =====
    IEnumerator SpinRoutine()
    {
        if (spinSound != null)
            audioSource.PlayOneShot(spinSound);

        isSpinning = true;

        float startAngle = currentAngle;

        float targetAngle = currentAngle + Random.Range(720f, 1080f);

        float time = 0;

        while (time < spinTime)
        {
            time += Time.deltaTime;

            float t = time / spinTime;

            float angle = Mathf.Lerp(startAngle, targetAngle, Mathf.SmoothStep(0, 1, t));

            wheel.eulerAngles = new Vector3(0, 0, angle);

            HandlePointerEffect(angle);

            yield return null;
        }

        currentAngle = targetAngle % 360;

        int rewardIndex = GetRewardIndex();

        if (winSound != null)
            audioSource.PlayOneShot(winSound);

        GiveReward(rewardIndex);

        Debug.Log("Trúng ô: " + rewardIndex);

        isSpinning = false;
    }

    // ===== POINTER EFFECT (tạch tạch) =====
    void HandlePointerEffect(float angle)
    {
        float normalized = angle % 360;

        float anglePerSegment = 360f / segmentCount;

        int currentSegment = (int)(normalized / anglePerSegment);

        if (currentSegment != lastSegment)
        {
            lastSegment = currentSegment;

            StartCoroutine(PointerBounce());

            if (tickSound != null)
                audioSource.PlayOneShot(tickSound);
        }
    }

    IEnumerator PointerBounce()
    {
        float duration = 0.05f;

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float angle = Mathf.Lerp(0, -25, t / duration);
            pointer.localEulerAngles = new Vector3(0, 0, angle);

            yield return null;
        }

        t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float angle = Mathf.Lerp(-25, 0, t / duration);
            pointer.localEulerAngles = new Vector3(0, 0, angle);

            yield return null;
        }
    }

    // ===== CALCULATE RESULT =====
    int GetRewardIndex()
    {
        int closestIndex = 0;
        float minAngle = 999f;

        Vector2 pointerDir = Vector2.up;

        for (int i = 0; i < rewards.Length; i++)
        {
            Vector2 sliceDir = rewards[i].slice.up;

            float angle = Vector2.Angle(pointerDir, sliceDir);

            if (angle < minAngle)
            {
                minAngle = angle;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // ===== GIVE REWARD =====
    void GiveReward(int index)
    {
        WheelReward reward = rewards[index];

        if (reward.type == RewardType.Random)
        {
            int rand = Random.Range(0, reward.randomPool.Length);
            reward = reward.randomPool[rand];
        }

        rewardPopup.Show(reward);
    }

    void GiveFinalReward(WheelReward reward)
    {
        switch (reward.type)
        {
            case RewardType.Coin:
                PlayerData.AddCoin(reward.amount);
                break;

            case RewardType.Energy:
                PlayerData.AddEnergy(reward.amount);
                break;

            case RewardType.Shield:
                PlayerData.AddShield(reward.amount);
                break;
        }
    }
}

public enum RewardType
{
    Coin,
    Energy,
    Shield,
    Random

}

[System.Serializable]
public class WheelReward
{
    public RewardType type;
    public int amount;
    public Transform slice;
    public WheelReward[] randomPool;
}