using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_GatherResources : State {


    private GameObject owner = null;
    private State_MoveToTarget movingState = null;
    private GameObject currentTargetResource = null;



    public override void Update() {
        if (!currentTargetResource)
            DecideNextResourceToGather();
        movingState.Update();
        if (movingState.ShouldTransition()) {
            CollectResource();
            DecideNextResourceToGather();
        }
    }
    public override void EvaluateTransition() {
        //For each connected state.
    }
    public override void EnterState() {

    }
    public override void ExitState() {

    }

    private void DecideNextResourceToGather() {
        //if (owner.GetResourcesToGather().Count == 0) {
        //    finished = true;
        //    return;
        //}

        //currentTargetResource = owner.GetResourcesToGather()[0];
        movingState.SetTargetPosition(currentTargetResource.transform.position);
    }


    private void CollectResource() {
        //var resources = owner.GetResourcesToGather();
        //resources[0].SetActive(false);
        //resources.RemoveAt(0);
    }
}
