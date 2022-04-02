using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{
    private float _resetTime = 5f;
    private float _timeSinceLastUse = 0f;
    [SerializeField]
    public int Index;
    [SerializeField]
    public string Category;

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
        _timeSinceLastUse = 0f;
    }

    private void Update()
    {
        _timeSinceLastUse += Time.deltaTime;
    }
}
