using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void OnEnter(PlayerStateManager state);

    public abstract void OnUpdate(PlayerStateManager state);

    //public abstract void OnExit();
    
}
