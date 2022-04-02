using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _nextScreen;
    [SerializeField]
    private GameObject _currentScreen;

    public void Click()
    {
        TransitionManager.Instance.ComputerScreenTransition(_currentScreen, _nextScreen);
    }
}
