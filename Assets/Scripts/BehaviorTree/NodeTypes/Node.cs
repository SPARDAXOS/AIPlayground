using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node {


    public abstract BehaviorTree.ExecutionResults Execute(BehaviorTree bt);
    public abstract BehaviorTree.EvaluationResults Evaluate(BehaviorTree bt);
}
