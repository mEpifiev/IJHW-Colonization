using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private CollectorZone _collectorZone;
    [SerializeField] private ResourceCounter _resourceCounter;

    [SerializeField] private Transform _dropResourcePoint;
    [SerializeField] private SpawnPoint _botSpawnPoint;

    [SerializeField] private int _initialBotCount = 1;
    [SerializeField] private int _minBotsToBuildNewBase = 1;
    [SerializeField] private float _collectResourceDelay = 0.5f;
    [SerializeField] private float _buildNewBaseDelay = 0.1f;

    [SerializeField] private int _resourcesToNewBot = 3;
    [SerializeField] private int _resourcesToNewBase = 5;

    private ResourceDispatcher _resourceDispatcher;
    private BotSpawner _botSpawner;
    private BaseSpawner _baseSpawner;

    private List<Bot> _bots = new();

    private Flag _currentFlag;

    private bool _isBuildingNewBase = false;
    public bool _isFlagBuilded = false;

    private void Start()
    {
        for (int i = 0; i < _initialBotCount; i++)
            SpawnBot();

        StartCoroutine(AssignTaskBotCollectResourceRoutine());
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

    public bool TryGetFlag(out Flag flag)
    {
        if (_currentFlag != null)
        {
            flag = _currentFlag;
            return true;
        }

        flag = null;
        return false;
    }

    public void SetFlag(Flag flag)
    {
        if (_currentFlag != null)
            return;

        _currentFlag = flag;
        _isFlagBuilded = true;

        StartCoroutine(AssignTaskBuildNewBaseRoutine());
    }


    public void CreateNewBase(Vector3 position, Bot bot)
    {
        Base newBase = _baseSpawner.Spawn(position);
        newBase.Initialize(_resourceDispatcher, _botSpawner, _baseSpawner);

        TransferBotToNewBase(newBase, bot);

        Destroy(_currentFlag.gameObject);
        _currentFlag = null;

        _isFlagBuilded = false;
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

    private IEnumerator AssignTaskBotCollectResourceRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_collectResourceDelay);

        while(enabled)
        {
            foreach(Bot bot in _bots)
            {
                if (bot.IsAssigned)
                    continue;

                Resource targetResurce = _resourceDispatcher.GetClosestResource(bot.transform.position);

                if (targetResurce != null)
                    bot.AssignTaskCollectResource(targetResurce, _dropResourcePoint);
            }

            yield return wait;
        }
    }

    private IEnumerator AssignTaskBuildNewBaseRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_buildNewBaseDelay);

        while (_isFlagBuilded)
        {
            if (_bots.Count > _minBotsToBuildNewBase && _resourceCounter.Count >= _resourcesToNewBase && _isBuildingNewBase == false)
            {
                Bot availableBot = null;

                foreach (Bot bot in _bots)
                {
                    if (bot.IsAssigned == false)
                    {
                        availableBot = bot;
                        break;
                    }
                }

                if (availableBot != null)
                {
                    _isBuildingNewBase = true;
                    _resourceCounter.Spend(_resourcesToNewBase);

                    Vector3 flagPosition = _currentFlag.transform.position;

                    availableBot.AssignTaskBuildNewBase(flagPosition);        
                }
            }

            yield return wait;
        }
    }

    private void OnTaskBotCompleted(Bot bot, Resource resource)
    {
        _resourceDispatcher.RemoveResource(resource);
        bot.GiveResource(_dropResourcePoint.position);

        _resourceCounter.Add();

        if (_resourceCounter.Count >= _resourcesToNewBot && (_isFlagBuilded == false || _bots.Count <= _minBotsToBuildNewBase))
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
