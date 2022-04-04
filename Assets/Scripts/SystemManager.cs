using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField]
    RawImage _gameOverImg;
    [SerializeField]
    Color _gameOverColor;
    [SerializeField]
    List<GameObject> _alldisableonend;

    [SerializeField]
    AudioClip _oneMinClip, _thirtySecClip;
    [SerializeField]
    List<AudioClip> _tenSCountdown;

    private float _evacProgress = 0f;
    private bool _active = false;
    private Tweener _fundsTween;
    private bool _gameDone = false;

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
            yield return new WaitForSeconds(1f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "we've got a problem");
            yield return new WaitForSeconds(1f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "there's an asteroid");
            yield return new WaitForSeconds(0.5f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "on a direct collision course");
            yield return new WaitForSeconds(2f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "starting evac protocol");
            yield return new WaitForSeconds(3f);
            ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "good luck");
            yield return new WaitForSeconds(0.5f);
            ChatManager.Instance.Say(ChatManager.ChatType.SYSTEM, "evac systems online");
            yield return new WaitForSeconds(2f);
        }
        TransitionManager.Instance.InstantScreenTransition(_bigScreenLogo, _bigScreenMain);
        TransitionManager.Instance.InstantScreenTransition(_compScreenLogo, _compScreenMain);
        yield return new WaitForSeconds(0.5f);
        _guns[0].ActivateUsable();
        _evacCenters[0].ActivateUsable();
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
            _active = false;
            StartCoroutine(FinishGame());
        }

        int time = Mathf.FloorToInt(_timeToImpact);
        if (time == 60)
        {
            Debug.Log("PLAY");
            SoundPlayer.Instance.PlayAudio(_oneMinClip, true);
        } else if (time == 30)
        {
            SoundPlayer.Instance.PlayAudio(_thirtySecClip, true);
        } else if (time < _tenSCountdown.Count)
        {
            SoundPlayer.Instance.PlayAudio(_tenSCountdown[time], true);
        }
    }

    IEnumerator FinishGame()
    {
        if (_gameDone)
            yield break;

        _gameDone = true;
        _alldisableonend.ForEach(g => g.SetActive(false));
        _compScreenLogo.SetActive(true);
        _gameOverImg.gameObject.SetActive(true);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "You're out of time");
        yield return new WaitForSeconds(1f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "what happened");
        yield return new WaitForSeconds(3.2f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "...");
        _gameOverImg.DOColor(_gameOverColor, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator WinGame()
    {
        if (_gameDone)
            yield break;

        _gameDone = true;
        // deactive screens
        _alldisableonend.ForEach(g => g.SetActive(false));
        _compScreenLogo.SetActive(true);
        _gameOverImg.gameObject.SetActive(true);
        // prez lines
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "Good job");
        yield return new WaitForSeconds(3f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "You've made humanity proud");
        yield return new WaitForSeconds(3f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "Oh!");
        yield return new WaitForSeconds(0.5f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "We're not at 100% evactuated");
        yield return new WaitForSeconds(0.5f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "it says there's one person left");
        yield return new WaitForSeconds(4f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "oops...");
        yield return new WaitForSeconds(4f);
        ChatManager.Instance.Say(ChatManager.ChatType.PRESIDENT, "uhh I'm losing signal...");

        // count down timer to 10
        if (_timeToImpact > 15f) {
            _active = false;
            Debug.Log("wait " + _timeToImpact);
            _asteroid.Override(15f);
            DOVirtual.Float(_timeToImpact, 10f, 5f, t => _timeToImpact = t);
            yield return new WaitForSeconds(5f);
        }

        _active = true;
        Debug.Log("wait " + (_timeToImpact + 2f));
        yield return new WaitForSeconds(_timeToImpact + 2f);

        _gameOverImg.DOColor(_gameOverColor, 1.5f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
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
        int numActive = _usableCategoryMap[category]._enabledCount;
        _usableCategoryMap[category]._enabledCount++;
        _usableCategoryMap[category]._usables[numActive].ActivateUsable();
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
