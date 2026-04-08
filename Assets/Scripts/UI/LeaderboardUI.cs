using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI")]
    public Transform content;
    public GameObject itemPrefab;
    public GameObject myRankItem;
    public GameObject panel;
    public GameObject loading;

    // ================= OPEN / CLOSE =================

    public void Open()
    {
        panel.SetActive(true);

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        ShowMyRankLocal();

        if (loading != null)
            loading.SetActive(true);

        LoadLeaderboard();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    // ================= LOAD DATA =================

    void LoadLeaderboard()
    {
        FirebaseDatabase db = FirebaseDatabase
            .GetInstance("https://tombofthemask-5eca1-default-rtdb.asia-southeast1.firebasedatabase.app/");

        db.GetReference("Leaderboard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (loading != null)
                loading.SetActive(false);

            if (task.IsCompleted && !task.IsFaulted)
            {
                DataSnapshot snapshot = task.Result;

                List<LeaderboardEntry> list = new List<LeaderboardEntry>();

                foreach (var child in snapshot.Children)
                {
                    string json = child.GetRawJsonValue();
                    LeaderboardEntry entry = JsonUtility.FromJson<LeaderboardEntry>(json);

                    entry.userId = child.Key; 

                    list.Add(entry);
                }

                list.Sort((a, b) => b.score.CompareTo(a.score));

                ShowUI(list);
            }
            else
            {
                Debug.LogError("Load leaderboard failed: " + task.Exception);
            }
        });
    }

    // ================= SHOW UI =================

    void ShowUI(List<LeaderboardEntry> list)
    {
        string myId = FirebaseManager.Instance != null 
            ? FirebaseManager.Instance.GetUserId() 
            : "";

        int myRank = -1;
        LeaderboardEntry myEntry = null;

        for (int i = 0; i < list.Count; i++)
        {
            var entry = list[i];

            GameObject item = Instantiate(itemPrefab, content);

            item.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            item.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = entry.name;
            item.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = entry.score.ToString();

            item.transform.Find("Avatar").GetComponent<Image>().sprite =
                AvatarDatabase.Instance.GetAvatar(entry.avatarId);

            if (!string.IsNullOrEmpty(myId) && entry.userId == myId)
            {
                myRank = i + 1;
                myEntry = entry;
            }
        }

        // ================= MY RANK =================

        if (myEntry != null)
        {
            myRankItem.SetActive(true);

            myRankItem.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = myRank.ToString();
            myRankItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = myEntry.name;
            myRankItem.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = myEntry.score.ToString();

            myRankItem.transform.Find("Avatar").GetComponent<Image>().sprite =
                AvatarDatabase.Instance.GetAvatar(myEntry.avatarId);
        }
    }

    // ================= LOCAL FALLBACK =================

    void ShowMyRankLocal()
    {
        string myName = PlayerPrefs.GetString("PlayerName", "Player");
        int myScore = PlayerPrefs.GetInt("HighScore", 0);
        int avatarId = PlayerPrefs.GetInt("AvatarID", 0);

        myRankItem.SetActive(true);

        myRankItem.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = "-";
        myRankItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = myName;
        myRankItem.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = myScore.ToString();

        myRankItem.transform.Find("Avatar").GetComponent<Image>().sprite =
            AvatarDatabase.Instance.GetAvatar(avatarId);
    }
}