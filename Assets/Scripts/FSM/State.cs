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

    protected Action onStartedCallback  = null;
    protected Action onUpdateCallback   = null;
    protected Action onFinishedCallback = null;

    public virtual void Initialize(StateMachine stateMachine, uint id) {
        ID = id;
        this.stateMachine = stateMachine;
    }
    public abstract void Update();
    public abstract bool ShouldTransition();

    public void SetOnStartedCallback(Action callback) { onStartedCallback = callback; }
    public void SetOnUpdateCallback(Action callback) { onUpdateCallback = callback; }
    public void SetOnFinishedCallback(Action callback) { onFinishedCallback = callback; }

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
    public StateType GetTransitionStateType() {
        return transitionStateType;
    }

    public uint GetID() {
        return ID;
    }
    public StateType GetStateType() {
        return type;
    }
}
