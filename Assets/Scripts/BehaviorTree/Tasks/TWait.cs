using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TWait : Task {

    private bool UseWaitKey = false;
    private float WaitDuration = 2.0f;

    private string WaitLengthKey = "";

    private float WaitTimer = 0.0f;


    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {

        bt.SetCurrentNode(this);

        if (UseWaitKey) {
            if (!bt.GetBlackboard().GetEntry(WaitLengthKey, out float WaitDuration)) {
                return BehaviorTree.EvaluationResults.ERROR;
            }
        }

        return BehaviorTree.EvaluationResults.SUCCESS;
    }
    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        if (CurrentTaskStatus == TaskStatus.NOT_RUNNING) {
            ResetWaitTimer(bt.GetBlackboard());
            CurrentTaskStatus = TaskStatus.RUNNING;
            bt.SetCurrentTaskName("Wait");
        }

        bt.SetCurrentNode(this);

        UpdateWaitTimer();
        if (WaitTimer == 0.0f) {
            CurrentTaskStatus = TaskStatus.NOT_RUNNING;
            return BehaviorTree.ExecutionResults.SUCCESS;
        }

        return BehaviorTree.ExecutionResults.RUNNING;
    }
    public override void Interrupt() {
        CurrentTaskStatus = TaskStatus.NOT_RUNNING;
        WaitTimer = WaitDuration; //Doesnt matter since first execution will overwrite it.
    }


    private void UpdateWaitTimer() {
        if (WaitTimer <= 0.0f)
            return;

        WaitTimer -= Time.deltaTime;
        if (WaitTimer < 0.0f)
            WaitTimer = 0.0f;
    }
    private void ResetWaitTimer(Blackboard bb) {
        if (UseWaitKey) {
            bb.GetEntry(WaitLengthKey, out float value);
            WaitTimer = value;
        }
        else
            WaitTimer = WaitDuration;
    }


    public void SetWaitDuration(float duration) { WaitDuration = duration; }
    public void SetUseWaitKeyState(bool state) { UseWaitKey = state; }
    public void SetWaitLengthKey(string key) { WaitLengthKey = key; }
}
