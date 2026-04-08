using UnityEngine;

public class AvatarDatabase : MonoBehaviour
{
    public static AvatarDatabase Instance;

    public Sprite[] avatars;

    void Awake()
    {
        Instance = this;
    }

    public Sprite GetAvatar(int id)
    {
        if (id >= 0 && id < avatars.Length)
            return avatars[id];

        return avatars[0];
    }
}