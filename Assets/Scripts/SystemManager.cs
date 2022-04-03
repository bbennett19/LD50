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
    float _timeToImpact;
    [SerializeField]
    Asteroid _asteroid;
    [SerializeField]
    GameObject _bigScreenLogo, _bigScreenMain, _compScreenLogo, _compScreenMain;
    [SerializeField]
    bool _skipIntro;
    [SerializeField]
    List<float> _weaponLevelPower, _weaponLevelSpeed, _evacLevelPower, _evacLevelSpeed;

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
        _usableCategoryMap.Add(GUNS, new UsableCategory(_guns, 1, 0, 0));
        _usableCategoryMap.Add(EVAC, new UsableCategory(_evacCenters, 1, 0, 0));

        _guns.ForEach((t) => t.UpdateResetTime(_weaponLevelSpeed[0]));
        _evacCenters.ForEach(t => t.UpdateResetTime(_evacLevelSpeed[0]));
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
        UsableCategory c = _usableCategoryMap[category];
        int newLevel = c._reloadLevel + 1;
        c._reloadLevel = newLevel;
        float time = category == GUNS ? _weaponLevelSpeed[newLevel] : _evacLevelSpeed[newLevel];
        c._usables.ForEach(t => t.UpdateResetTime(time));

        Debug.Log(category + " RESET TIME: " + time);
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

    public void UseFunds(int amount, float delay=0f)
    {
        _funds -= amount;

        if (_fundsTween != null && _fundsTween.active)
            _fundsTween.Kill();

        _fundsTween = DOVirtual.Int(_displayFunds, _funds, .5f, UpdateDisplayFunds).SetDelay(delay);
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
        float incTime = _weaponLevelPower[_usableCategoryMap[GUNS]._powerLevel];
        Debug.Log("TIME ADDED: " + incTime);
        _active = false;
        float newTime = _timeToImpact + incTime;
        DOVirtual.Float(_timeToImpact, newTime, 1f, SetImpactTime).SetDelay(1f).OnComplete(() => _active = true).SetEase(Ease.Linear);
        UseFunds(-_moneyPerHit, 1f);
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
        float progressInc = _evacLevelPower[_usableCategoryMap[EVAC]._powerLevel];
        Debug.Log("PROGRESS ADDED: " + progressInc);
        float newEvacProgress = _evacProgress + progressInc;
        DOVirtual.Float(_evacProgress, newEvacProgress, 1f, SetEvacProgress).SetDelay(1f);

        if (_evacProgress >= 100f)
        {
            _evacProgress = 99.9999f;
            StartCoroutine(WinGame());
        }
        else
        {
            UseFunds(-_moneyPerEvac, 1f);
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
