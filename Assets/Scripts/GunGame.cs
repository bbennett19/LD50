using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _gunObject;
    [SerializeField]
    private float _successUpperRange, _successLowerRange;

    [SerializeField]
    Vector3 _startRotation, _endRotation;

    [SerializeField]
    GameObject _bigCurrent, _compCurrent, _bigNext, _compNext;

    private Action _completeCallback;

    private void OnEnable()
    {
        _gunObject.transform.eulerAngles = _startRotation;
        _gunObject.transform.DORotate(_endRotation, 2f, RotateMode.Fast).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetCompleteCallback(Action callback)
    {
        _completeCallback = callback;
    }

    public void Fire()
    {
        DOTween.Kill(_gunObject.transform);
        float rotation = _gunObject.transform.rotation.eulerAngles.z;
        if (rotation >= _successLowerRange && rotation <= _successUpperRange)
        {
            Debug.Log("HIT " + rotation);
            SystemManager.Instance.AsteroidHit();
        } else
        {
            Debug.Log("MISS " + rotation);
        }


        _completeCallback();

        TransitionManager.Instance.BigScreenTransition(_compCurrent, _compNext, _bigCurrent, _bigNext);
    }
}
