using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIsGameObjectValid : Decorator {
    public enum SuccessCondition { IS_VALID, IS_NOT_VALID }

    private SuccessCondition CurrentSuccessCondition = SuccessCondition.IS_VALID;
    private string ObjectKey = "";


    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {
        Blackboard BB = bt.GetBlackboard();
        if (BB == null)
            return BehaviorTree.EvaluationResults.ERROR;

        if (!BB.GetEntry(ObjectKey, out GameObject value))
            return BehaviorTree.EvaluationResults.ERROR;

        if (CurrentSuccessCondition == SuccessCondition.IS_VALID) {
            if (value)
                return BehaviorTree.EvaluationResults.SUCCESS;
            else
                return BehaviorTree.EvaluationResults.FAILURE;
        }
        else {
            if (value)
                return BehaviorTree.EvaluationResults.FAILURE;
            else
                return BehaviorTree.EvaluationResults.SUCCESS;
        }
    }
    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        Blackboard BB = bt.GetBlackboard();
        if (BB == null)
            return BehaviorTree.ExecutionResults.ERROR;

        if (!BB.GetEntry(ObjectKey, out GameObject value))
            return BehaviorTree.ExecutionResults.ERROR;

        if (CurrentSuccessCondition == SuccessCondition.IS_VALID) {
            if (value)
                return BehaviorTree.ExecutionResults.SUCCESS;
            else
                return BehaviorTree.ExecutionResults.FAILURE;
        }
        else {
            if (value)
                return BehaviorTree.ExecutionResults.FAILURE;
            else
                return BehaviorTree.ExecutionResults.SUCCESS;
        }
    }
    
    private bool CheckCondition(Blackboard bb) {




        return false;
    }

    public void SetObjectKey(string key) { ObjectKey = key; }
    public void SetSuccessCondition(SuccessCondition condition) { CurrentSuccessCondition = condition; }
}
