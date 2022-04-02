using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsableButtonController : MonoBehaviour
{
    [SerializeField]
    private Usable _usable;
    [SerializeField]
    private GameObject _disabledGameObject;
    [SerializeField]
    private GameObject _buyableGameObject;
    [SerializeField]
    private GameObject _reloadingGameObject;
    [SerializeField]
    private GameObject _readyGameObject;
    [SerializeField]
    private TextMeshProUGUI _buyText;

    private GameObject _activeGameObject = null;

    // Update is called once per frame
    void Update()
    {
        if (SystemManager.Instance.IsUsableUsable(_usable.Category, _usable.Index))
        {
            EnableNewState(_readyGameObject);
        }
        else if (SystemManager.Instance.IsUsableBuyable(_usable.Category, _usable.Index))
        {
            EnableNewState(_buyableGameObject);
            _buyText.text = "$" + _usable.Cost;
        }
        else if (SystemManager.Instance.IsUseableEnabled(_usable.Category, _usable.Index))
        {
            EnableNewState(_reloadingGameObject);
        }
        else
        {
            EnableNewState(_disabledGameObject);
        }
    }

    private void EnableNewState(GameObject go)
    {
        if (go == _activeGameObject)
            return;

        if (_activeGameObject != null)
        {
            _activeGameObject.SetActive(false);
        }

        _activeGameObject = go;

        _activeGameObject.SetActive(true);
    }

    public void Use()
    {
        _usable.Use();
    }

    public void Buy()
    {
        if (SystemManager.Instance.GetFunds() < _usable.Cost)
        {
            Debug.Log("NOT ENOUGH MONEY");
            return;
        }

        SystemManager.Instance.UseFunds(_usable.Cost);
        SystemManager.Instance.EnableNewUsable(_usable.Category);
    }
}
