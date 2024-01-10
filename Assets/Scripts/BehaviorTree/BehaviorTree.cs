using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree {

    public enum ExecutionResults {
        ERROR = 0,
        FAILURE,
        SUCCESS,
        RUNNING
    }
    public enum EvaluationResults {
        ERROR = 0,
        FAILURE,
        SUCCESS,
        RUNNING
    }


    private ExecutionResults CurrentExecutionResults = ExecutionResults.ERROR;
    private EvaluationResults CurrentEvaluationResults = EvaluationResults.ERROR;


    private Root RootNode = new Root();

    private Node CurrentNode = null;
    private Task CurrentTask = null;
    private Composite CurrentLastTickedComposite = null;



    public void SetCurrentNode(Node node) {
        CurrentNode = node;
    }
    public void SetCurrentTask(Task task) {
        CurrentTask = task;
    }
    public void SetLastTickedComposite(Composite composite) {
        CurrentLastTickedComposite = composite;
    }

    public Root GetRoot() {
        return RootNode;
    }

}
