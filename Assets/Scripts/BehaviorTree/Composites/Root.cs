using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Composite {


    public override BehaviorTree.ExecutionResults Execute(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogError("No nodes are connected to the root");
            return BehaviorTree.ExecutionResults.ERROR;
        }

        foreach(Node node in ConnectedNodes) {
            bt.SetCurrentNode(this);
            BehaviorTree.ExecutionResults Results = node.Execute(bt);
            if (Results == BehaviorTree.ExecutionResults.FAILURE)
                continue;

            return Results;
        }

        return BehaviorTree.ExecutionResults.FAILURE;
    }
    public override BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt) {
        if (ConnectedNodes.Count == 0) {
            Debug.LogError("No nodes are connected to the root");
            return BehaviorTree.EvaluationResults.ERROR;
        }

        foreach (Node node in ConnectedNodes) {
            bt.SetCurrentNode(this);
            BehaviorTree.EvaluationResults Results = node.Evaluate(bt);
            if (Results == BehaviorTree.EvaluationResults.FAILURE)
                continue;

            return Results;
        }

        return BehaviorTree.EvaluationResults.FAILURE;
    }
}
