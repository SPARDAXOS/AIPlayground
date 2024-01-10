using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using UnityEngine;
using static State;

public class StateMachine {

    private List<State> states = new List<State>();

    private GameObject owner = null;
    private State currentState = null;
    private static uint stateIDCounter = 1;



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


    public bool AddState(State state) {
        if (DoesStateExist(state))
            return false;

        state.Initialize(this, stateIDCounter);
        states.Add(state);
        stateIDCounter++;
        return true;
    }
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
