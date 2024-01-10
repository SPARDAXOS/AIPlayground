using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite {

    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogWarning("Composite does not contain any connected nodes");
            return BehaviorTree.ExecutionResults.ERROR;
        }

        bt.SetCurrentNode(this);

        foreach (Node node in ConnectedNodes) {
            if (BehaviorParent)
                bt.SetLastTickedComposite(this);

            BehaviorTree.ExecutionResults Results = node.Execute(bt);
            if (Results == BehaviorTree.ExecutionResults.FAILURE)
                continue;

            return Results;
        }

        return BehaviorTree.ExecutionResults.ERROR;
    }
    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogWarning("Composite does not contain any connected nodes");
            return BehaviorTree.EvaluationResults.ERROR;
        }

        bt.SetCurrentNode(this);

        foreach (Node node in ConnectedNodes) {
            if (BehaviorParent)
                bt.SetLastTickedComposite(this);

            BehaviorTree.EvaluationResults Results = node.Evaluate(bt);
            if (Results == BehaviorTree.EvaluationResults.FAILURE)
                continue;

            return Results;
        }

        return BehaviorTree.EvaluationResults.FAILURE;
    }
}
