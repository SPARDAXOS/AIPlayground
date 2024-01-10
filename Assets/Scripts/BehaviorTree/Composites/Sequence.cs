using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite {


    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogWarning("Composite does not contain any connected nodes");
            return BehaviorTree.ExecutionResults.ERROR;
        }

        bt.SetCurrentNode(this);
        foreach (Node node in ConnectedNodes) {
            BehaviorTree.ExecutionResults Results = node.Execute(bt);
            if (Results == BehaviorTree.ExecutionResults.SUCCESS)
                continue;

            return Results;
        }

        return BehaviorTree.ExecutionResults.SUCCESS;
    }
    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogWarning("Composite does not contain any connected nodes");
            return BehaviorTree.EvaluationResults.ERROR;
        }

        bt.SetCurrentNode(this);
        foreach (Node node in ConnectedNodes) {
            BehaviorTree.EvaluationResults Results = node.Evaluate(bt);
            if (Results == BehaviorTree.EvaluationResults.SUCCESS)
                continue;

            return Results;
        }

        return BehaviorTree.EvaluationResults.SUCCESS;
    }
}
