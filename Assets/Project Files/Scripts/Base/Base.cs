using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CollectorZone _collectorZone;
    [SerializeField] private ResourceCounter _resourceCounter;
    [SerializeField] private FlagController _flagController;

    [SerializeField] private Transform _dropResourcePoint;
    [SerializeField] private SpawnPoint _botSpawnPoint;

    [SerializeField] private int _initialBotCount = 1;
    [SerializeField] private int _minBotsToBuildNewBase = 1;
    [SerializeField] private float _taskDelay = 0.5f;

    [SerializeField] private int _resourcesToNewBot = 3;
    [SerializeField] private int _resourcesToNewBase = 5;

    private ResourceDispatcher _resourceDispatcher;
    private BotSpawner _botSpawner;
    private BaseSpawner _baseSpawner;

    private List<Bot> _bots = new();

    private bool _isBuildingNewBase = false;

    public FlagController FlagController => _flagController;

    private void Start()
    {
        for (int i = 0; i < _initialBotCount; i++)
            SpawnBot();

        StartCoroutine(AssignTaskBotRoutine());
    }

    private void OnEnable()
    {
        _collectorZone.BotEntered += OnTaskBotCompleted;
    }

    private void OnDisable()
    {
        _collectorZone.BotEntered -= OnTaskBotCompleted;
    }

    public void Initialize(ResourceDispatcher resourceDispatcher, BotSpawner botSpawner, BaseSpawner baseSpawner)
    {
        _resourceDispatcher = resourceDispatcher;
        _botSpawner = botSpawner;
        _baseSpawner = baseSpawner;
    }

    public void CreateNewBase(Vector3 position, Bot bot)
    {
        Base newBase = _baseSpawner.Spawn(position);
        newBase.Initialize(_resourceDispatcher, _botSpawner, _baseSpawner);

        TransferBotToNewBase(newBase, bot);

        _flagController.ClearFlag();

        _isBuildingNewBase = false;
    }

    public void ReceiveBot(Bot bot)
    {
        _bots.Add(bot);
        bot.SetBase(this);
    }

    private void TransferBotToNewBase(Base newBase, Bot bot)
    {
        if (_bots.Remove(bot) == false)
            return;

        newBase.ReceiveBot(bot);
    }

    private IEnumerator AssignTaskBotRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_taskDelay);

        while(enabled)
        {
            foreach(Bot bot in _bots)
            {
                if (bot.IsAssigned)
                    continue;

                if (_flagController.IsFlagBuilded && _isBuildingNewBase == false && _bots.Count > _minBotsToBuildNewBase && _resourceCounter.Count >= _resourcesToNewBase)
                {
                    _isBuildingNewBase = true;
                    _resourceCounter.Spend(_resourcesToNewBase);

                    bot.AssignTaskBuildNewBase(_flagController.FlagPosition);

                    break;
                }

                Resource targetResurce = _resourceDispatcher.GetClosestResource(bot.transform.position);

                if (targetResurce != null)
                    bot.AssignTaskCollectResource(targetResurce, _dropResourcePoint);
            }

            yield return wait;
        }
    }

    private void OnTaskBotCompleted(Bot bot, Resource resource)
    {
        _resourceDispatcher.RemoveResource(resource);
        bot.GiveResource(_dropResourcePoint.position);

        _resourceCounter.Add();

        if (_resourceCounter.Count >= _resourcesToNewBot && (_flagController.IsFlagBuilded == false || _bots.Count <= _minBotsToBuildNewBase))
        {
            _resourceCounter.Spend(_resourcesToNewBot);
            SpawnBot();
        }

        resource.Release();
    }

    private void SpawnBot()
    {
        Bot bot = _botSpawner.Spawn(_botSpawnPoint);

        ReceiveBot(bot);
    }
}
