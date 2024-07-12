using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberjack : MonoBehaviour {

    public enum AIMode { FSM, BT };

    public AIMode CurrentAIMode = AIMode.BT;

    public float Speed = 1.0f;
    private Vector3 RoamTarget = Vector3.zero;

    private float BlackboardUpdateFrequency = 0.1f;
    private float BlackboardUpdateTimer = 0.0f;


    private StateMachine stateMachine;
    private State_MoveToTarget movingState;
    private State_Wait waitState;
    private State_HarvestWood harvestState;
    private State_GatherResources gatherResourcesState;

    private BehaviorTree MainBehaviorTree;
    private Blackboard MainBlackboard;


    void Start() {
        SetupFSM();
        SetupBT();
    }
    void Update() {
        if (CurrentAIMode == AIMode.FSM)
            stateMachine.Update();
        else if (CurrentAIMode == AIMode.BT) {
            UpdateBlackboard();
            MainBehaviorTree.Update();
            //Debug.Log(MainBehaviorTree.GetCurrentTaskName());
        }
    }
    private void UpdateBlackboard() {
        BlackboardUpdateTimer += Time.deltaTime;
        if (BlackboardUpdateTimer < BlackboardUpdateFrequency)
            return;

        BlackboardUpdateTimer -= BlackboardUpdateFrequency;

        MainBlackboard.UpdateEntry("Speed", Speed);
        MainBlackboard.UpdateEntry("RoamTarget", RoamTarget);
    }


    private void SetupFSM() {
        stateMachine = new StateMachine();
        stateMachine.Initialize(gameObject);

        movingState = new State_MoveToTarget();
        waitState = new State_Wait();
        harvestState = new State_HarvestWood();
        gatherResourcesState = new State_GatherResources();


        //Switch this out with BT style connections.
        waitState.SetTransitionState(State.StateType.MOVE_TO_TARGET);
        movingState.SetTransitionState(State.StateType.HARVEST_TREE);
        harvestState.SetTransitionState(State.StateType.GATHER_RESOURCES);
        gatherResourcesState.SetTransitionState(State.StateType.WAIT);

        //movingState.SetOnStartedCallback(OnMovingStartCallback);
        //harvestState.SetOnFinishedCallback(OnHarvestFinishedCallback);

        stateMachine.AddState(movingState);
        stateMachine.AddState(waitState);
        stateMachine.AddState(harvestState);
        stateMachine.AddState(gatherResourcesState);

        //Some sort of function for starting state?
        stateMachine.TransitionToState(State.StateType.WAIT);
    }
    private void SetupBT() {
        MainBehaviorTree = new BehaviorTree();
        MainBlackboard = MainBehaviorTree.GetBlackboard();
        SetupBlackboardEntries();

        Sequence RoamingSequence = new Sequence();

        TWait WaitTask = new TWait();
        WaitTask.SetWaitDuration(2.0f);

        TMoveTo MoveToTask = new TMoveTo();
        MoveToTask.SetGameObjectKey("Self");
        MoveToTask.SetSpeedKey("Speed");
        MoveToTask.SetTargetLocationKey("RoamTarget");

        MainBehaviorTree.GetRoot().ConnectNode(RoamingSequence); //Do something more elegant
        RoamingSequence.ConnectNode(MoveToTask);
        RoamingSequence.ConnectNode(WaitTask);
    }
    private void SetupBlackboardEntries() {
        MainBlackboard.AddEntry("Self", gameObject);
        MainBlackboard.AddEntry("Speed", Speed);
        MainBlackboard.AddEntry("RoamTarget", RoamTarget);
    }



    //public Tree GetTargetTree() {
    //    //return targetTree;
    //}
    //public List<GameObject> GetResourcesToGather() {
    //    return resourcesToGather;
    //}

    //public void OnMovingStartCallback() {
    ////    targetTree = SimulationManagerReference.GetClosestTree(transform.position);
    ////    movingState.SetTargetPosition(targetTree.transform.position);
   // }
    //public void OnHarvestFinishedCallback() {
    ////    resourcesToGather = targetTree.GetResources();
   // //    targetTree = null;
   // }
}
