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
    protected string name = "Unnamed State";

    protected uint ID = 0;
    protected StateType type = StateType.NONE;
    protected StateType transitionStateType = StateType.NONE;
    protected StateMachine parent = null;
    protected bool initialized = false;

    protected Action onStartedCallback  = null;
    protected Action onUpdateCallback   = null;
    protected Action onFinishedCallback = null;


    public bool Connect(State state) {
        if (IsConnected(state))
            return false;

        connectedStates.Add(state);
        return true;
    }
    public bool Disconnect(State state) {
        if (!IsConnected(state))
            return false;

        connectedStates.Remove(state);
        return true; 
    }
    public bool IsConnected(State state) {
        if (connectedStates.Count == 0) 
            return false;

        return connectedStates.Contains(state); 
    }
    public bool Possess(StateMachine owner) {
        if (parent != null) {
            if (owner != parent)
                Debug.LogWarning("Attempted to possess a state that is already owned by another state machine.");
            else
                Debug.LogWarning("State machine attempted to possess a state that is already possessed.");

            return false;
        }

        parent = owner;
        return true;
    }
    public void Unpossess() {
        if (parent == null)
            return;

        parent = null;
    }

    public string GetName() { return name; }
    public void SetName(string name) {  this.name = name; } 
    public int GetConnectedStatesCount() { return connectedStates.Count; }
    public List<State> GetConnectedStates() {  return connectedStates; }



    //Note: Also by ID?
    public State FindState(string name) {
        if (connectedStates.Count == 0)
            return null;

        foreach (State state in connectedStates) {
            if (state.GetName() == name)
                return state;
        }

        return null;
    }



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
        this.parent = stateMachine;
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
    public StateMachine GetOwner() { return parent; }
}
