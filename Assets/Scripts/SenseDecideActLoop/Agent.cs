using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Agent : MonoBehaviour {

    protected bool isActive = false;
    protected AgentsManager managerRef    = null;



    virtual public void Init(AgentsManager manager) {

        managerRef = manager;
    }
    public bool GetState() { return isActive; }
    virtual public void SetState(bool state) {
        isActive = state;
        gameObject.SetActive(state);
    }

    virtual public void Sense() {
        //Update data
    }
    virtual public void Decide() {
        //Check which action to take.
    }
    virtual public void Act() {
        //Perform action.
    }
}