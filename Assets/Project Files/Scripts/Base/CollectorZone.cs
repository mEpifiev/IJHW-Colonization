using System;
using UnityEngine;

public class CollectorZone : MonoBehaviour
{
    public event Action<Bot, Resource> BotEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bot bot) && bot.Collector.HaveResource)
        {
            Resource resource = bot.Collector.CurrentResource;

            BotEntered?.Invoke(bot, resource);          
        }
    }
}
