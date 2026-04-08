using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseTest : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase OK");
            }
            else
            {
                Debug.LogError("Firebase lỗi");
            }
        });
    }
}