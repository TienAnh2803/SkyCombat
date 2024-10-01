using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject botPrefab;
    public int numberOfBots = 10;
    public Transform[] spawnPoints;
    private List<GameObject> bots = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        SpawnBots();
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    // Hàm tạo bot
    void SpawnBots()
    {
        for (int i = 0; i < numberOfBots; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject bot = Instantiate(botPrefab, spawnPoint.position, spawnPoint.rotation, transform);

            bots.Add(bot);
        }
    }

    void FixedUpdate()
    {
        ManageBots();
    }

    void ManageBots()
    {
        bots.RemoveAll(bot => bot == null);
    }

    public GameObject GetRandomBot()
    {
        int randomIndex = Random.Range(0, bots.Count + players.Count);
        if (randomIndex >= bots.Count)
        {
            return players[randomIndex - bots.Count];
        }
        else
        {
            return bots[randomIndex];
        }
    }
}
