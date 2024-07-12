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

    protected uint ID = 0;
    protected StateType type = StateType.NONE;
    protected StateType transitionStateType = StateType.NONE;
    protected StateMachine stateMachine = null;
    protected bool initialized = false;

    protected Action onStartedCallback  = null;
    protected Action onUpdateCallback   = null;
    protected Action onFinishedCallback = null;


    //Note: Look into how derived classes would go off this.
    public virtual void Initialize(StateMachine stateMachine, uint id) {
        if (initialized)
            return;

        ID = id;
        this.stateMachine = stateMachine;
        initialized = true;
    }
    public abstract void Update();
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
