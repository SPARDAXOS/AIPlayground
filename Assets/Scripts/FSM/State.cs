using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class State {

    protected List<State> connectedStates = new List<State>();
    protected string name = "Unnamed State";
    protected StateMachine parent = null;
    protected State transitionTarget = null;
    private bool shouldTransition = false;

    protected Action onEnterCallback  = null;
    protected Action onUpdateCallback   = null;
    protected Action onExitCallback = null;


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





    //Note: Also by ID?
    public State FindConnectedState(string name) {
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
    

    public abstract void Update();
    public abstract void Evaluate();

    /// <summary>
    /// Called by state machine when state is entered.
    /// </summary>
    public virtual void EnterState() {
        //Start state

    }

    /// <summary>
    /// Called by state machine when state is exited.
    /// </summary>
    public virtual void ExitState() {
        //Clean up

    }


    /// <summary>
    /// For state machine usage only. Do not call this!
    /// </summary>
    public void InvokeOnEnterCallback() { onEnterCallback?.Invoke(); }

    /// <summary>
    /// For state machine usage only. Do not call this!
    /// </summary>
    public void InvokeOnUpdateCallback() { onUpdateCallback?.Invoke(); }

    /// <summary>
    /// For state machine usage only. Do not call this!
    /// </summary>
    public void InvokeOnExitCallback() { onExitCallback?.Invoke(); }

    public void SetOnEnterCallback(Action callback) { onEnterCallback = callback; }
    public void SetOnUpdateCallback(Action callback) { onUpdateCallback = callback; }
    public void SetOnExitCallback(Action callback) { onExitCallback = callback; }
    public void SetName(string name) { this.name = name; }
    private void SetShouldTransition(bool state) { shouldTransition = state; }


    public string GetName() { return name; }
    public StateMachine GetOwner() { return parent; }
    public bool ShouldTransition() { return shouldTransition; } 
    public State GetTransitionTarget() { return transitionTarget; }
    public int GetConnectedStatesCount() { return connectedStates.Count; }
    public List<State> GetConnectedStates() { return connectedStates; }
}
