using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    private float _resetTime = 15f;
    private float _timeSinceLastUse = 0f;
    [SerializeField]
    public int Index;
    [SerializeField]
    public string Category;
    [SerializeField]
    public int Cost;
    [SerializeField]
    private ActionExecutor _actionExecutor;

    private void Awake()
    {
        _timeSinceLastUse = _resetTime;
    }

    public bool CanUse()
    {
        return _timeSinceLastUse >= _resetTime;
    }

    public void Use()
    {
        _actionExecutor.ExecuteAction(UseComplete);
    }

    public float GetReloadPercent()
    {
        return Mathf.Clamp01(_timeSinceLastUse / _resetTime);
    }

    private void UseComplete()
    {
        _timeSinceLastUse = 0f;
    }

    private void Update()
    {
        _timeSinceLastUse += Time.deltaTime;
    }
}
