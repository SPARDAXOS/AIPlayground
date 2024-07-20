using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class State_Wait : State {

    private float waitDuration = 2.0f;
    private float waitTimer = 0.0f;

    public override void Update() {
        UpdateTimer();
    }
    public override void EvaluateTransition() {
        if (waitTimer != 0.0f) //This states condition.
            return;

        //Check the conditions of other states.
        foreach (var item in connectedStates) {
            if (item.EvaluateEntry()) {
                SetShouldTransition(true);
                SetTransitionTarget(item);
                return;
            }
        }
    }
    public override void EnterState() {
        base.EnterState();
        waitTimer = waitDuration;
    }
    public override void ExitState() {
        base.ExitState();
        waitTimer = waitDuration;
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
