using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    private const float SpawnOffset = 2;

    [SerializeField] private BotPool _botPool;

    public Bot Spawn(SpawnPoint spawnPoint)
    {
        Bot bot = _botPool.Get();

        Vector3 randomOffset = new Vector3(Random.Range(-SpawnOffset, SpawnOffset), 0, Random.Range(-SpawnOffset, SpawnOffset));

        bot.transform.position = spawnPoint.transform.position + randomOffset;

        return bot;
    }
}
