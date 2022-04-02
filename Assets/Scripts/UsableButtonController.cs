using System.Collections;
using System.Collections.Generic;
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
        SystemManager.Instance.EnableNewUsable(_usable.Category);
    }
}
