using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    private const float SpawnOffset = 2;

    [SerializeField] private BotPool _botPool;
    [SerializeField] private SpawnPoint _spawnPoint;

    public List<Bot> Spawn(int count)
    {
        List<Bot> bots = new();

        for (int i = 0; i < count; i++)
        {
            Bot bot = _botPool.Get();

            Vector3 randomOffset = new Vector3(Random.Range(-SpawnOffset, SpawnOffset), 0, Random.Range(-SpawnOffset, SpawnOffset)
        );

            bot.transform.position = _spawnPoint.transform.position + randomOffset;
            bots.Add(bot);
        }

        return bots;
    }
}
