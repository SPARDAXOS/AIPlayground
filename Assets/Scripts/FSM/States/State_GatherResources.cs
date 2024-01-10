using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_GatherResources : State {


    private GameObject owner = null;
    private State_MoveToTarget movingState = null;
    private GameObject currentTargetResource = null;

    private bool finished = false;


    public override void Initialize(StateMachine stateMachine, uint id) {
        base.Initialize(stateMachine, id);
        type = StateType.GATHER_RESOURCES;
        //owner = stateMachine.GetOwner().GetComponent<Lumberjack>();

        movingState = new State_MoveToTarget();
        movingState.Initialize(stateMachine, id);
    }
    public override void Update() {
        if (!currentTargetResource)
            DecideNextResourceToGather();
        movingState.Update();
        if (movingState.ShouldTransition()) {
            CollectResource();
            DecideNextResourceToGather();
        }
    }
    public override bool ShouldTransition() {
        if (finished) {
            finished = false;
            return true;
        }

        return false;
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
