using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Wait : State {

    private float waitDuration = 2.0f;
    private float waitTimer = 0.0f;


    public override void Initialize(StateMachine stateMachine, uint id) {
        base.Initialize(stateMachine, id);
        type = StateType.WAIT;
        waitTimer = waitDuration;
    }
    public override void Update() {
        UpdateTimer();
    }
    public override bool ShouldTransition() {
        if (waitTimer == 0.0f) {
            waitTimer = waitDuration;
            return true;
        }
        else
            return false;
    }


    private void UpdateTimer() {
        if (waitTimer <= 0.0f)
            return;

        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0.0f)
            waitTimer = 0.0f;
    }
    public void SetWaitDuration(float duration) {
        waitDuration = duration;
        waitTimer = waitDuration;
    }
}
