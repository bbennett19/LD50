using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public const string GUNS = "GUNS";
    public const string EVAC = "EVAC";
    public static SystemManager Instance { get; private set; }

    [SerializeField]
    List<Usable> _guns;
    [SerializeField]
    List<Usable> _evacCenters;
    [SerializeField]
    int _funds, _moneyPerHit, _moneyPerEvac, _displayFunds;
    [SerializeField]
    int _upgradeCost;
    [SerializeField]
    float _timeToImpact;
    [SerializeField]
    Asteroid _asteroid;
    [SerializeField]
    GameObject _bigScreenLogo, _bigScreenMain, _compScreenLogo, _compScreenMain;
    [SerializeField]
    bool _skipIntro;

    private float _evacProgress = 0f;
    private bool _active = false;
    private Tweener _fundsTween;

    private Dictionary<string, UsableCategory> _usableCategoryMap = new Dictionary<string, UsableCategory>();

    private class UsableCategory
    {
        public List<Usable> _usables;
        public int _enabledCount;
        public int _powerLevel;
        public int _reloadLevel;

        public UsableCategory(List<Usable> usables, int enabledCount, int powerLevel, int reloadLevel)
        {
            _usables = usables;
            _enabledCount = enabledCount;
            _powerLevel = powerLevel;
            _reloadLevel = reloadLevel;
        }
    }

    private void Awake()
    {
        Instance = this;
        _usableCategoryMap.Add(GUNS, new UsableCategory(_guns, 1, 1, 1));
        _usableCategoryMap.Add(EVAC, new UsableCategory(_evacCenters, 1, 1, 1));
    }

    IEnumerator Start()
    {
        _displayFunds = _funds;

        if (!_skipIntro)
        {
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "hey");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "we've got a problem");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "there's an asteroid");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "on a direct collision course");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "starting evac protocol");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "good luck");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.SYSTEM, "evac systems online");
            yield return new WaitForSeconds(2f);
        }
        TransitionManager.Instance.InstantScreenTransition(_bigScreenLogo, _bigScreenMain);
        TransitionManager.Instance.InstantScreenTransition(_compScreenLogo, _compScreenMain);
        yield return new WaitForSeconds(0.5f);
        _active = true;
    }

    private void Update()
    {
        if (!_active)
            return;

        _timeToImpact -= Time.deltaTime;

        if (_timeToImpact <= 0f)
        {
            _timeToImpact = 0f;
            StartCoroutine(FinishGame());
        }
    }

    IEnumerator FinishGame()
    {
        _active = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator WinGame()
    {
        _active = false;
        yield return null;
        SceneManager.LoadScene("GameOver");
    }

    public bool IsUseableEnabled(string category, int i)
    {
        return i < _usableCategoryMap[category]._enabledCount;
    }

    public bool IsUsableBuyable(string category, int i)
    {
        return i == _usableCategoryMap[category]._enabledCount;
    }

    public bool IsUsableUsable(string category, int i)
    {
        return i < _usableCategoryMap[category]._enabledCount && _usableCategoryMap[category]._usables[i].CanUse();
    }

    public void EnableNewUsable(string category)
    {
        _usableCategoryMap[category]._enabledCount++;
    }

    public void IncreaseUsablePowerLevel(string category)
    {
        _usableCategoryMap[category]._powerLevel++;
    }

    public void IncreaseUsableReloadLevel(string category)
    {
        _usableCategoryMap[category]._reloadLevel++;
    }

    public int GetUsablePowerLevel(string category)
    {
        return _usableCategoryMap[category]._powerLevel;
    }

    public int GetUsableReloadLevel(string category)
    {
        return _usableCategoryMap[category]._reloadLevel;
    }

    public int GetFunds()
    {
        return _funds;
    }

    public int GetDisplayFunds()
    {
        return _displayFunds;
    }

    public int GetUpgradeCost()
    {
        return _upgradeCost;
    }

    public void UseFunds(int amount)
    {
        _funds -= amount;

        if (_fundsTween != null && _fundsTween.active)
            _fundsTween.Kill();

        _fundsTween = DOVirtual.Int(_displayFunds, _funds, .5f, UpdateDisplayFunds);
    }

    private void UpdateDisplayFunds(int funds)
    {
        _displayFunds = funds;
    }

    public float GetTimeToImpact()
    {
        return _timeToImpact;
    }

    public void AsteroidHit()
    {
        _active = false;
        float newTime = _timeToImpact + _usableCategoryMap[GUNS]._powerLevel * 30;
        DOVirtual.Float(_timeToImpact, newTime, 1f, SetImpactTime).OnComplete(() => _active = true).SetEase(Ease.Linear);
    }

    private void SetImpactTime(float time)
    {
        _timeToImpact = time;
        _asteroid.ImpactTimeUpdated();
    }


    public float GetEvacProgress()
    {
        return _evacProgress;
    }

    public void EvacSuccess()
    {
        float newEvacProgress = _evacProgress + _usableCategoryMap[EVAC]._powerLevel * 10;
        DOVirtual.Float(_evacProgress, newEvacProgress, 1f, SetEvacProgress);

        if (_evacProgress >= 100f)
        {
            _evacProgress = 99.9999f;
            StartCoroutine(WinGame());
        }
    }

    private void SetEvacProgress(float progress)
    {
        _evacProgress = progress;

        if (_evacProgress >= 100f)
        {
            _evacProgress = 99.9999f;
            StartCoroutine(WinGame());
        }
    }
}
