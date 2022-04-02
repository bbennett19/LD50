using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButtonController : MonoBehaviour
{
    [SerializeField]
    private string _category;
    [SerializeField]
    private int _level;
    [SerializeField]
    private bool _isForPower;
    [SerializeField]
    private GameObject _disabledGameObject;
    [SerializeField]
    private GameObject _buyableGameObject;
    [SerializeField]
    private GameObject _purchasedGameObject;

    private GameObject _currentActive;

    // Update is called once per frame
    void Update()
    {
        int level = _isForPower ? SystemManager.Instance.GetUsablePowerLevel(_category) : SystemManager.Instance.GetUsableReloadLevel(_category);

        if (level >= _level)
        {
            EnableNewState(_purchasedGameObject);
        }
        else if (level == _level - 1)
        {
            EnableNewState(_buyableGameObject);
        }
        else
        {
            EnableNewState(_disabledGameObject);
        }
    }

    private void EnableNewState(GameObject go)
    {
        if (go == _currentActive)
            return;

        if (_currentActive != null)
        {
            _currentActive.SetActive(false);
        }

        _currentActive = go;

        _currentActive.SetActive(true);
    }

    public void BuyLevel()
    {
        if (_isForPower)
            SystemManager.Instance.IncreaseUsablePowerLevel(_category);
        else
            SystemManager.Instance.IncreaseUsableReloadLevel(_category);
    }
}
