using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ImpactTimeDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var span = new TimeSpan(0, 0, (int) SystemManager.Instance.GetTimeToImpact());
        _text.text = string.Format("{0}:{1:00}", (int)span.TotalMinutes, span.Seconds);
    }
}
