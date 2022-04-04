using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float _dest;

    // Start is called before the first frame update
    void Start()
    {
        ImpactTimeUpdated();
    }

    public void Override(float time)
    {
        DOTween.Kill(transform);
        transform.DOMoveX(_dest, time).SetEase(Ease.Linear);
    }

    public void ImpactTimeUpdated()
    {
        transform.DOMoveX(_dest, SystemManager.Instance.GetTimeToImpact()).SetEase(Ease.Linear);
    }
}
