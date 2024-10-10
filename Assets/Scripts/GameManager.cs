using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject botPrefab;
    public int numberOfBots = 10;
    public Transform[] spawnPoints;
    private List<GameObject> spawnedBots = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            SpawnBots();
        }

        StartCoroutine(SearchPlayer());
    }

    private IEnumerator SearchPlayer()
    {
        while (true)
        {
            players = GameObject.FindGameObjectsWithTag("Player").ToList();
            yield return new WaitForSeconds(2f);
        }
    }

    // Hàm tạo bot
    private void SpawnBots()
    {
        for (int i = 0; i < numberOfBots; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            NetworkObject bot = Runner.Spawn(botPrefab, spawnPoint.position, spawnPoint.rotation, Object.InputAuthority);
            GameObject botGameObject = bot.gameObject;

            spawnedBots.Add(botGameObject);
        }
    }

    void FixedUpdate()
    {
        ManageBots();
    }

    void ManageBots()
    {
        spawnedBots.RemoveAll(bot => bot == null);
    }

    public GameObject GetRandomInGameObject()
    {
        int randomIndex = Random.Range(0, spawnedBots.Count + players.Count);
        Debug.Log(spawnedBots.Count + players.Count);
        if (randomIndex >= spawnedBots.Count)
        {
            return players[randomIndex - spawnedBots.Count];
        }
        else
        {
            return spawnedBots[randomIndex];
        }
    }
}
