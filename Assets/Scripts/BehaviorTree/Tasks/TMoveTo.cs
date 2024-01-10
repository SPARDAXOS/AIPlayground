using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TMoveTo : Task {

    private float DefaultSnapThreshold = 0.1f;
    private bool UseSnapThresholdKey = false;

    private string GameObjectKey = "";
    private string SpeedKey = "";
    private string SnapThresholdKey = "";
    private string TargetLocationKey = "";


    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {
        bt.SetCurrentNode(this);

        if (!ValidateKeys(bt.GetBlackboard()))
            return BehaviorTree.EvaluationResults.ERROR;

        return BehaviorTree.EvaluationResults.SUCCESS;
    }
    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        if (CurrentTaskStatus == TaskStatus.NOT_RUNNING) {
            CurrentTaskStatus = TaskStatus.RUNNING;
            bt.SetCurrentTaskName("Moving to target");
        }

        bt.SetCurrentNode(this);

        Blackboard MainBlackboard = bt.GetBlackboard();
        MainBlackboard.GetEntry(GameObjectKey, out GameObject Target);
        MainBlackboard.GetEntry(SpeedKey, out float Speed);
        MainBlackboard.GetEntry(TargetLocationKey, out Vector3 TargetLocation);
        float SnapThreshold = DefaultSnapThreshold;
        if (UseSnapThresholdKey)
            MainBlackboard.GetEntry(SnapThresholdKey, out SnapThreshold);

        UpdatePosition(Target, TargetLocation, Speed);
        if (IsAtTarget(Target, TargetLocation, SnapThreshold)) {
            CurrentTaskStatus = TaskStatus.NOT_RUNNING;
            return BehaviorTree.ExecutionResults.SUCCESS;
        }

        return BehaviorTree.ExecutionResults.RUNNING;
    }
    public override void Interrupt() {
        CurrentTaskStatus = TaskStatus.NOT_RUNNING;
    }

    private void UpdatePosition(GameObject gameObject, Vector3 targetLocation, float speed) {
        Vector3 Direction = (targetLocation - gameObject.transform.position).normalized;
        Vector3 Velocity = Direction * speed * Time.deltaTime;
        gameObject.transform.position += Velocity;
    }
    private bool ValidateKeys(Blackboard blackboard) {
        if (!blackboard.GetEntry(GameObjectKey, out GameObject value1))
            return false;

        if (!blackboard.GetEntry(SpeedKey, out float value2))
            return false;

        if (!blackboard.GetEntry(TargetLocationKey, out Vector3 value4))
            return false;

        if (UseSnapThresholdKey) {
            if (!blackboard.GetEntry(SnapThresholdKey, out float value3))
                return false;
        }

        return true;
    }
    private bool IsAtTarget(GameObject target, Vector3 location, float snapThreshold) {
        float Distance = Vector3.Distance(target.transform.position, location);
        if (Distance <= snapThreshold) {
            target.transform.position = location;
            return true;
        }

        return false;
    }


    public void SetGameObjectKey(string key) { GameObjectKey = key; }
    public void SetSpeedKey(string key) { SpeedKey = key; }
    public void SetSnapThresholdKey(string key) { SnapThresholdKey = key; }
    public void SetTargetLocationKey(string key) { TargetLocationKey = key; }
}
