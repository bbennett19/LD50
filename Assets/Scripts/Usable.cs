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
    public string Category, displayName, systemText;
    [SerializeField]
    public int Cost;
    [SerializeField]
    private ActionExecutor _actionExecutor;
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private bool gotReady = false;

    private void Awake()
    {
        _timeSinceLastUse = 1000f;
    }

    public void UpdateResetTime(float newTime)
    {
        _resetTime = newTime;
    }

    public void ActivateUsable()
    {
        isActive = true;
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
        gotReady = false;
    }

    private void Update()
    {
        if (!isActive)
            return;

        _timeSinceLastUse += Time.deltaTime;

        if (_timeSinceLastUse >= _resetTime && !gotReady)
        {
            gotReady = true;
            int t = Index + 1;
            ChatManager.Instance.Say(ChatManager.ChatType.SYSTEM, displayName + t + " " + systemText);
        }
    }
}
