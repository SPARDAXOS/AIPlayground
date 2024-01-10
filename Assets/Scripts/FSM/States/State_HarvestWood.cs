using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_HarvestWood : State {

    private float harvestPower = 1.0f;
    private Tree harvestTarget = null;

    private GameObject owner = null;

    private bool finished = false;


    public override void Initialize(StateMachine stateMachine, uint id) {
        base.Initialize(stateMachine, id);
        type = StateType.HARVEST_TREE;
        //owner = stateMachine.GetOwner().GetComponent<Lumberjack>();
    }
    public override void Update() {
        if (!harvestTarget) {
            //harvestTarget = owner.GetTargetTree();
            if (!harvestTarget)
                return;
        }

        HarvestTree();
    }
    public override bool ShouldTransition() {
        if (finished) {
            harvestTarget = null;
            finished = false;
            return true;
        }

        return false;
    }


    private void HarvestTree() {
        //harvestTarget.TakeDamage(harvestPower * Time.deltaTime);
        //if (harvestTarget.GetHealth() == 0.0f)
            finished = true;
    }
    public void SetHarvestPower(float value) {
        harvestPower = value;
    }
}
