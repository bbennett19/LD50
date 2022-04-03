using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionExecutor: MonoBehaviour
{
    public abstract void ExecuteAction(Action completeCallback);
}
