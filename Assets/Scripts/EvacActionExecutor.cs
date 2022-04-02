using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacActionExecutor : ActionExecutor
{
    public override void ExecuteAction()
    {
        SystemManager.Instance.EvacSuccess();
    }
}
