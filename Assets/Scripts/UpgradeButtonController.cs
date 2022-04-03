using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int _cost;
    [SerializeField]
    private GameObject _disabledGameObject;
    [SerializeField]
    private GameObject _buyableGameObject;
    [SerializeField]
    private GameObject _purchasedGameObject;
    [SerializeField]
    TextMeshProUGUI _costText;

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
            _costText.text = "$" + _cost;
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
        if (SystemManager.Instance.GetFunds() < _cost)
        {
            Debug.Log("NOT ENOUGH MONEY");
            return;
        }

        SystemManager.Instance.UseFunds(_cost);

        if (_isForPower)
            SystemManager.Instance.IncreaseUsablePowerLevel(_category);
        else
            SystemManager.Instance.IncreaseUsableReloadLevel(_category);
    }
}
