using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActionExecutor : ActionExecutor
{
    public override void ExecuteAction()
    {
        SystemManager.Instance.AsteroidHit();
    }
}
