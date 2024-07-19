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


    public void Initialize(GameObject owner) {
        this.owner = owner;
    }
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

        currentState.Evaluate();
        if (currentState.ShouldTransition()) {
            State nextState = currentState.GetTransitionTarget();
            if (nextState == null) {
                currentState.EnterState(); //Should reset the state.
                currentState.InvokeOnEnterCallback();
                return; //Any other clean up? Structure with 1 state is fine and should be supported.
            }

            //Can reuse transition to state here. but it doesnt clean up much and adds 1 check if the state is contained in the state machine for extra protection in case the state got a new one connected to it after runniong the machine
            currentState.ExitState();
            currentState.InvokeOnExitCallback();

            nextState.EnterState();
            nextState.InvokeOnEnterCallback();
            currentState = nextState;
        }

        //TransitionToState(TransitionType);
    }

    public bool Activate() {
        if (!valid || activated)
            return false;

        currentState = entryState;
        currentState.EnterState();
        currentState.InvokeOnEnterCallback();

        activated = true;
        return true;
    }
    public bool Deactivate() {
        if (!valid || !activated)
            return false;

        currentState.ExitState();
        currentState.InvokeOnExitCallback();
        currentState = null;

        activated = false;
        return true;
    }


    //Steps for the whole thing
    //1. Connect states and form the structure of the machine.
    //2. Select entry for the machine.
    //3. Machine collects all states in the structure and caches them for access and other purposes.
    //4. Initializes all states.
    //5. Function for checking if a state exist in the machine or to set the current state of the machine are available then!+


    //This function can directly take a list with optional argument being starting state! It checks if the starting state is within the list before succeding.
    public bool Setup(List<State> structure, State startingState = null) {
        if (valid || states.Count == 0)
            return false;

        if (startingState != null && !states.Contains(startingState))
            return false;

        states = structure;
        foreach (var state in states)
            state.Possess(this);

        if (startingState != null)
            entryState = startingState;
        else
            entryState = states[0];

        valid = true;
        return true;
    }
    public bool Clear() {
        if (!valid)
            return false;

        foreach (var state in states)
            state.Unpossess();
        states.Clear();

        entryState = null;
        currentState = null;
        valid = false;

        return true;
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
