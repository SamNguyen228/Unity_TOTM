using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using System.Collections;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private FirebaseUser user;

    private bool isReady = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitFirebase();
    }

    // ================= INIT =================

    void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                app.Options.DatabaseUrl = new System.Uri(
                    "https://tombofthemask-5eca1-default-rtdb.asia-southeast1.firebasedatabase.app/"
                );

                FirebaseDatabase db = FirebaseDatabase.GetInstance(app);
                dbRef = db.RootReference;

                auth = FirebaseAuth.GetAuth(app);

                SignIn();
            }
            else
            {
                Debug.LogError("Firebase lỗi: " + task.Result);
            }
        });
    }

    void SignIn()
    {
        if (auth.CurrentUser != null)
        {
            user = auth.CurrentUser;
            isReady = true;

            Debug.Log("Reuse UserID: " + user.UserId);

            UploadPlayerInfo();
            return;
        }

        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                user = auth.CurrentUser;
                isReady = true;

                Debug.Log("New UserID: " + user.UserId);

                UploadPlayerInfo();
            }
        });
    }

    // ================= UPLOAD INFO (LOGIN) =================

    void UploadPlayerInfo()
    {
        string userId = user.UserId;

        string name = PlayerPrefs.GetString("PlayerName", "Player");
        int avatarId = PlayerPrefs.GetInt("AvatarID", 0);
        int score = PlayerPrefs.GetInt("HighScore", 0);

        LeaderboardEntry entry = new LeaderboardEntry(userId, name, score, avatarId);
        string json = JsonUtility.ToJson(entry);

        dbRef.Child("Leaderboard").Child(userId).SetRawJsonValueAsync(json);

        Debug.Log("Uploaded player info after login");
    }

    // ================= SAVE SCORE =================

    public void SaveScore(int score)
    {
        StartCoroutine(SaveScoreWhenReady(score));
    }

    IEnumerator SaveScoreWhenReady(int score)
    {
        while (!isReady || user == null)
            yield return null;

        string userId = user.UserId;

        dbRef.Child("Leaderboard").Child(userId).Child("score").SetValueAsync(score);

        PlayerPrefs.SetInt("HighScore", score);

        Debug.Log("Updated score: " + score);
    }

    // ================= UPDATE INFO =================

    public void UpdateName(string newName)
    {
        PlayerPrefs.SetString("PlayerName", newName);

        if (user != null)
            dbRef.Child("Leaderboard").Child(user.UserId).Child("name").SetValueAsync(newName);
    }

    public void UpdateAvatar(int avatarId)
    {
        PlayerPrefs.SetInt("AvatarID", avatarId);

        if (user != null)
            dbRef.Child("Leaderboard").Child(user.UserId).Child("avatarId").SetValueAsync(avatarId);
    }

    // ================= GET USER ID =================

    public string GetUserId()
    {
        return user != null ? user.UserId : "";
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public string userId;
    public string name;
    public int score;
    public int avatarId;

    public LeaderboardEntry(string userId, string name, int score, int avatarId)
    {
        this.userId = userId;
        this.name = name;
        this.score = score;
        this.avatarId = avatarId;
    }
}