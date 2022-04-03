using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FundsDisplay : MonoBehaviour
{
    [SerializeField]
    private string _headerText;

    TextMeshProUGUI _fundsText;

    // Start is called before the first frame update
    void Start()
    {
        _fundsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _fundsText.text = _headerText + SystemManager.Instance.GetDisplayFunds();
    }
}
