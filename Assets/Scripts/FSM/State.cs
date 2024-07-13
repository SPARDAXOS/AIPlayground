using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class State {
    public enum StateType {
        NONE = 0,
        MOVE_TO_TARGET,
        WAIT,
        HARVEST_TREE,
        GATHER_RESOURCES
    }

    protected List<State> connectedStates = new List<State>();
    protected uint ID = 0;
    protected StateType type = StateType.NONE;
    protected StateType transitionStateType = StateType.NONE;
    protected StateMachine stateMachine = null;
    protected bool initialized = false;

    protected Action onStartedCallback  = null;
    protected Action onUpdateCallback   = null;
    protected Action onFinishedCallback = null;


    //API for state management
    //Add
    //Remove
    //Find
    //HasState
    //Count
    
    //Switching to a state uses CanTransition virtual function that each state has to implement
    //Also, priority is decided by which state is connected first. Index basically 0-X



    //Note: Look into how derived classes would go off this.
    public virtual void Initialize(StateMachine stateMachine, uint id) {
        if (initialized)
            return;

        ID = id;
        this.stateMachine = stateMachine;
        initialized = true;
    }
    public abstract void Update();

    //Currently used for if current state should move to the single connected state
    //Rework this to dectate if the current state can be transition into instead.
    public abstract bool ShouldTransition(); 

    public void InvokeOnStartedCallback() { 
        onStartedCallback?.Invoke(); 
    }
    public void InvokeOnUpdateCallback() {
        onUpdateCallback?.Invoke(); 
    }
    public void InvokeOnFinishedCallback() { 
        onFinishedCallback?.Invoke(); 
    }

    public void SetTransitionState(StateType type) {
        transitionStateType = type;
    }
    public void SetOnStartedCallback(Action callback) { onStartedCallback = callback; }
    public void SetOnUpdateCallback(Action callback) { onUpdateCallback = callback; }
    public void SetOnFinishedCallback(Action callback) { onFinishedCallback = callback; }

    public StateType GetTransitionStateType() { return transitionStateType; }
    public uint GetID() { return ID; }
    public StateType GetStateType() { return type; }
    public StateMachine GetStateMachine() { return stateMachine; }
}
