using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState state;
    PlayerIdleState idleState = new PlayerIdleState();
    PlayerWalkState walkState = new PlayerWalkState();
    PlayerRunState runState = new PlayerRunState();


    void Start()
    {
        state = idleState;

        state.OnEnter(this);
    }


    void Update()
    {
        
    }
}
