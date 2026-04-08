using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] playerPrefabs;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int selected = PlayerData.GetSelectedChar();

        if (selected < 0 || selected >= playerPrefabs.Length)
            selected = 0;

        GameObject player = Instantiate(
            playerPrefabs[selected],
            spawnPoint.position,
            Quaternion.identity
        );

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.target = player.transform;
        }
    }
}