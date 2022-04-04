using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGame : MonoBehaviour
{
    [SerializeField]
    private GameObject _gunObject, _resultScreen;
    [SerializeField]
    private float _successUpperRange, _successLowerRange;

    [SerializeField]
    Vector3 _startRotation, _endRotation;

    [SerializeField]
    GameObject _bigCurrent, _compCurrent, _bigNext, _compNext, _staticScreen, _mainContent, _resultContent, _bullet;

    [SerializeField]
    Vector3 _bulletStart, _bulletMiss, _bulletHit;

    private Action _completeCallback;

    private void OnEnable()
    {
        _mainContent.SetActive(true);
        _resultContent.SetActive(false);
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
        StartCoroutine(DoFire(rotation >= _successLowerRange && rotation <= _successUpperRange));
    }

    IEnumerator DoFire(bool hit)
    {
        // animate shot
        yield return new WaitForSeconds(0.5f);
        _staticScreen.SetActive(true);
        _mainContent.SetActive(false);
        _resultContent.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _staticScreen.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        _bullet.transform.position = _bulletStart;
        _bullet.gameObject.SetActive(true);

        if (hit)
        {
            Debug.Log("HIT");
            _bullet.transform.DOMove(_bulletHit, 0.25f).SetEase(Ease.Linear).OnComplete(() => _bullet.gameObject.SetActive(false));
            // play hit effect
        }
        else
        {
            Debug.Log("MISS");
            _bullet.transform.DOMove(_bulletMiss, 0.25f).SetEase(Ease.Linear).OnComplete(() => _bullet.gameObject.SetActive(false));
            // play miss effect
        }

        yield return new WaitForSeconds(1.5f);

        if (hit)
        {
            ChatManager.Instance.Say(ChatManager.ChatType.SYSTEM, "direct hit");
            SystemManager.Instance.AsteroidHit();
        } else
        {
            ChatManager.Instance.Say(ChatManager.ChatType.SYSTEM, "miss");
        }

        _completeCallback();

        TransitionManager.Instance.BigScreenQuickTransition(_compCurrent, _compNext, _bigCurrent, _bigNext);        
    }
}
