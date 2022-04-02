using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvacProgressDisplay : MonoBehaviour
{
    [SerializeField]
    Slider _slider;
    [SerializeField]
    TextMeshProUGUI _text;

    // Update is called once per frame
    void Update()
    {
        _slider.value = SystemManager.Instance.GetEvacProgress();
        _text.text = string.Format("{0:0.####}%", _slider.value);
    }
}
