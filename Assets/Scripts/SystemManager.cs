using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public const string GUNS = "GUNS";
    public const string EVAC = "EVAC";
    public static SystemManager Instance { get; private set; }

    [SerializeField]
    List<Usable> _guns;
    [SerializeField]
    List<Usable> _evacCenters;

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
}