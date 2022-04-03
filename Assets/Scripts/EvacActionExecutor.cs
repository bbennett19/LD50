using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacActionExecutor : ActionExecutor
{
    [SerializeField]
    GameObject _currentComputerScreen, _currentBigScreen, _nextComputerScreen, _nextBigScreen;
    [SerializeField]
    EvacGame _evacGame;
    public override void ExecuteAction(Action completeCallback)
    {
        TransitionManager.Instance.BigScreenTransition(_currentComputerScreen, _nextComputerScreen, _currentBigScreen, _nextBigScreen);
        _evacGame.SetCompleteCallback(completeCallback);
    }
}
