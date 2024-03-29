using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void OnEnter(PlayerStateManager player);

    public abstract void OnUpdate(PlayerStateManager player);

    //public abstract void OnExit();
    
}
