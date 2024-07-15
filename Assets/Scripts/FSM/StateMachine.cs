using System.Collections.Generic;
using UnityEngine;
using static State;

public class StateMachine {

    //Note: ID is primarily for faster look up.
    private static uint stateIDCounter = 1;

    //For ownership purposes
    private GameObject owner = null; 
    private State currentState = null;

    private State entryState = null;
    private bool valid = false;
    private List<State> states = new List<State>();



    public void Initialize(GameObject owner) {
        this.owner = owner;
    }
    public void Update() {
        if (currentState == null)
            return;

        EvaluateCurrentState();
        UpdateCurrentState();
    }
    private void UpdateCurrentState() {
        currentState.Update();
        currentState.InvokeOnUpdateCallback();
    }
    private void EvaluateCurrentState() {
        if (!currentState.ShouldTransition())
            return;

        currentState.InvokeOnFinishedCallback();

        StateType TransitionType = currentState.GetTransitionStateType();
        if (TransitionType == StateType.NONE) {
            Debug.LogError("State " + currentState.GetStateType().ToString() + " does not specify a transition state.");
            return;
        }

        TransitionToState(TransitionType);
    }


    //Steps for the whole thing
    //1. Connect states and form the structure of the machine.
    //2. Select entry for the machine.
    //3. Machine collects all states in the structure and caches them for access and other purposes.
    //4. Initializes all states.
    //5. Function for checking if a state exist in the machine or to set the current state of the machine are available then!+


    //This function can directly take a list with optional argument being starting state! It checks if the starting state is within the list before succeding.
    public bool SetEntryState(List<State> states, State startingState = null) {
        if (valid || states.Count == 0)
            return false;

        if (startingState != null && !states.Contains(startingState))
            return false;

        this.states = states;

        if (startingState != null)
            entryState = startingState;
        else
            entryState = states[0];

        valid = true;
        return true;
    }
    public bool ClearEntryState() {
        if (entryState == null)
            return false;

        entryState = null;
        return true;
    }
    public bool IsValid() {  return valid; }

    //This is kinda weird now.
    public bool AddState(State state) {
        StateMachine ownerStateMachine = state.GetStateMachine();
        if (ownerStateMachine != null)
            return false;

        if (DoesStateExist(state))
            return false;

        state.Initialize(this, stateIDCounter);
        states.Add(state);
        stateIDCounter++;
        return true;
    }

    //Can be used to trigger a hard transition to any registered state.
    public void TransitionToState(StateType type) {
        foreach (var item in states) {
            if (item.GetStateType() == type) {
                currentState = item;
                currentState.InvokeOnStartedCallback();
                return;
            }
        }

        Debug.LogError("Unable to transition to state of type " + type.ToString() + "\n Reason: It does not exist in state machine.");
    }


    public GameObject GetOwner() { return owner; }
    public State GetCurrentState() { return currentState; }

    public bool DoesStateExist(State state) {
        foreach (var item in states) {
            if (item == state)
                return true;
        }
        
        return false;
    }
    public bool DoesStateExist(uint id) {
        foreach (var item in states) {
            if (item.GetID() == id)
                return true;
        }

        return false;
    }
    public bool DoesStateExist(StateType type) {
        if (type == StateType.NONE)
            return false;

        foreach (var item in states) {
            if (item.GetStateType() == type)
                return true;
        }

        return false;
    }
}
