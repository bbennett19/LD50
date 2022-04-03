using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _text;

    private string _message;

    public void SetMessage(string message)
    {
        _message = message;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _text.text = ".";
        yield return new WaitForSeconds(0.33f);
        _text.text = "..";
        yield return new WaitForSeconds(0.33f);
        _text.text = "...";
        yield return new WaitForSeconds(0.33f);
        _text.text = ".";
        yield return new WaitForSeconds(0.33f);
        _text.text = "..";
        yield return new WaitForSeconds(0.33f);
        _text.text = "...";
        yield return new WaitForSeconds(0.33f);
        _text.text = _message;
        // play sound effect
    }
}
