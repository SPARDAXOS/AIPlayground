using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public sealed class State_MoveToTarget : State {

    private GameObject targetGameObject = null;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private float speed = 1.0f;
    private float snapDistance = 0.01f;


    public override void Update() {
        if (targetGameObject)
            UpdatePosition();
    }
    public override void EvaluateTransition() {
        if (currentPosition != targetPosition) //This states condition.
            return;

        //Check the conditions of other states.
        foreach (var item in connectedStates) {
            if (item.EvaluateEntry()) {
                SetShouldTransition(true);
                SetTransitionTarget(item);
                return;
            }
        }
    }
    public override void EnterState() {
        targetGameObject = parent.GetOwner();
    }
    public override void ExitState() {

    }


    private void UpdatePosition() {
        currentPosition = targetGameObject.transform.position;

        Vector3 direction = targetPosition - currentPosition;
        direction.z = 0.0f;
        direction.Normalize();
        Vector3 velocity = direction * speed * Time.deltaTime;
        currentPosition += velocity;

        float distance = Vector3.Distance(currentPosition, targetPosition);
        if (distance <= snapDistance)
            targetGameObject.transform.position = targetPosition;
        else
            targetGameObject.transform.position = currentPosition;
    }

    public void SetTargetPosition(Vector3 targetPosition) { this.targetPosition = targetPosition; }
    public void SetTargetGameObject(GameObject target) { targetGameObject = target; }
    public void SetSpeed(float speed) { this.speed = speed; }
    public void SetSnapDistance(float distance) { snapDistance = distance; }
}
