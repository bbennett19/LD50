using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvacGame : MonoBehaviour
{
    [SerializeField]
    float _maxHeight, _minHeight;
    [SerializeField]
    GameObject _greenZone, _pointer;
    [SerializeField]
    GameObject _bigCurrent, _compCurrent, _bigNext, _compNext, _ship;
    [SerializeField]
    TextMeshProUGUI _statusText1, _statusText2;

    [SerializeField]
    Vector3 _shipStart, _shipEnd;

    private float _successMin, _successMax;

    private int _iteration = 0;

    Action _completeCallback;

    private void OnEnable()
    {
        _iteration = 0;

        // reset any UI
        _statusText2.text = "Pending";

        _ship.transform.position = _shipStart;

        StartGame();
    }

    private void StartGame()
    {
        _iteration++;
        UpdateStatusText("Activating");
        float height = UnityEngine.Random.Range(_minHeight, _maxHeight);
        _greenZone.transform.localPosition = new Vector3(0f, height, 0f);
        _successMax = height;
        _successMin = height - _greenZone.GetComponent<SpriteRenderer>().bounds.size.y;

        _pointer.transform.localPosition = new Vector3(_pointer.transform.localPosition.x, 0f, 0f);
        DOTween.Kill(_pointer.transform);
        _pointer.transform.DOLocalMoveY(_maxHeight, 1.5f).SetLoops(-1).SetEase(Ease.InSine);
    }

    public void SetCompleteCallback(Action callback)
    {
        _completeCallback = callback;
    }

    public void DoThing()
    {
        DOTween.Kill(_pointer.transform);

        float pointHeight = _pointer.transform.localPosition.y;

        if (pointHeight >= _successMin && pointHeight <= _successMax)
        {
            // update UI
            UpdateStatusText("Go");

            // if last iteration/complete game
            if (_iteration == 2)
            {
                StartCoroutine(FinishGame(true));
            }
            else
            {
                StartGame();
            }
        }
        else
        {
            // update UI
            UpdateStatusText("Abort");
            // complete game
            StartCoroutine(FinishGame(false));
        }
    }

    private void UpdateStatusText(string text)
    {
        if (_iteration == 1)
        {
            _statusText1.text = text;
        }
        else
        {
            _statusText2.text = text;
        }
    }

    private IEnumerator FinishGame(bool success)
    {
        if (success)
        {
            yield return new WaitForSeconds(0.5f);
            _ship.transform.DOMove(_shipEnd, 2f).SetEase(Ease.InSine);
            yield return new WaitForSeconds(2.5f);
            SystemManager.Instance.EvacSuccess();
        }
        else
        {
            GameObject flashingObj = _iteration == 1 ? _statusText1.gameObject : _statusText2.gameObject;
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            flashingObj.SetActive(true);

        }

        _completeCallback();
        TransitionManager.Instance.BigScreenQuickTransition(_compCurrent, _compNext, _bigCurrent, _bigNext);
    }
}
