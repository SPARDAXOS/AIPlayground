using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;
using static State;

public class StateMachine {

    private GameObject owner = null; 
    private State currentState = null;
    private State entryState = null;
    private List<State> states = new List<State>();

    private bool valid = false;
    private bool activated = false;


    /// <summary>
    /// Registers a state-structure to use in the state machine.
    /// </summary>
    public bool Initialize(GameObject owner, List<State> structure, State entry = null) {
        if (valid || states.Count == 0 || owner == null)
            return false;

        if (entry != null && !structure.Contains(entry))
            return false;

        this.owner = owner;
        states = structure;
        foreach (var state in states)
            state.Possess(this);

        if (entry != null)
            entryState = entry;
        else
            entryState = states[0];

        valid = true;
        return true;
    }

    /// <summary>
    /// Clears all data from the state machine and deactivates it if active.
    /// </summary>
    public bool Clear() {
        if (!valid)
            return false;

        if (activated)
            Deactivate();

        foreach (var state in states)
            state.Unpossess();
        states.Clear();

        entryState = null;
        currentState = null;
        owner = null;
        valid = false;

        return true;
    }

    /// <summary>
    /// Enters the designated entry state and starts the state machine.
    /// </summary>
    public bool Activate() {
        if (!valid || activated)
            return false;

        currentState = entryState;
        currentState.EnterState();
        currentState.InvokeOnEnterCallback();

        activated = true;
        return true;
    }

    /// <summary>
    /// Exits the current running state if active and stops the state machine.
    /// </summary>
    public bool Deactivate() {
        if (!valid || !activated)
            return false;

        currentState.ExitState();
        currentState.InvokeOnExitCallback();
        currentState = null;

        activated = false;
        return true;
    }


    /// <summary>
    /// Updates the current state in the state machine.
    /// </summary>
    public void Update() {
        if (!valid || !activated)
            return;

        EvaluateCurrentState();
        UpdateCurrentState();
    }
    private void UpdateCurrentState() {
        if (currentState == null)
            return;

        currentState.Update();
        currentState.InvokeOnUpdateCallback();
    }
    private void EvaluateCurrentState() {
        if (currentState == null)
            return;

        currentState.EvaluateTransition();
        if (currentState.ShouldTransition()) {
            State nextState = currentState.GetTransitionTarget();
            if (nextState == null) {
                currentState.EnterState();
                currentState.InvokeOnEnterCallback();
                return;
            }
            //Can reuse transition function
            currentState.ExitState();
            currentState.InvokeOnExitCallback();

            nextState.EnterState();
            nextState.InvokeOnEnterCallback();
            currentState = nextState;
        }
    }
    public bool Transition(State state) {
        if (!valid || !activated)
            return false;

        if (!ContainsState(state)) {
            Debug.LogError("Unable to transition to state " + state.GetName() + "\n Reason: It does not exist in state machine.");
            return false;
        }

        if (currentState == null)
            currentState = state;
        else {
            currentState.ExitState();
            currentState.InvokeOnExitCallback();
            currentState = state;
        }

        currentState.EnterState();
        currentState.InvokeOnEnterCallback();
        return true;
    }
    public bool SetEntryState(State entry) {
        if (!valid)
            return false;

        if (!states.Contains(entry))
            return false;

        entryState = entry;
        return true;
    }


    public bool IsValid() { return valid; }
    public GameObject GetOwner() { return owner; }
    public State GetCurrentState() { return currentState; }
    public State GetEntryState() { return entryState; }


    /// <summary>
    /// Does not reuse FindState(string) implementation. It performs full comparison to evaluate results not only name comparison.
    /// </summary>
    public bool ContainsState(State state) {
        if (!valid)
            return false;

        foreach (var item in states) {
            if (item == state)
                return true;
        }

        return false;
    }
    public bool ContainsState(string name) {
        return FindState(name) != null;
    }
    public State FindState(string name) {
        if (!valid)
            return null;

        foreach (var item in states) {
            if (item.GetName() == name)
                return item;
        }

        return null;
    }
    public List<State> FindStates(string name) {
        if (!valid)
            return null;

        List<State> targets = new List<State>();
        foreach (var item in states) {
            if (item.GetName() == name)
                targets.Add(item);
        }

        return targets;
    }
    public T FindState<T>() where T : State {
        if (!valid)
            return null;

        foreach (var item in states) {
            if (item.GetType() == typeof(T))
                return (T)item;
        }

        return null;
    }
    public List<T> FindStates<T>() where T : State {
        if (!valid)
            return null;

        List<T> targets = new List<T>();
        foreach (var item in states) {
            if (item.GetType() == typeof(T))
                targets.Add((T)item);
        }

        return targets;
    }
}
