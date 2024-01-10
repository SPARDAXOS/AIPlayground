using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public sealed class State_MoveToTarget : State {

    private GameObject targetGameObject = null;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private float speed = 1.0f;
    private float snapDistance = 0.01f;


    public override void Initialize(StateMachine stateMachine, uint id) {
        base.Initialize(stateMachine, id);
        type = StateType.MOVE_TO_TARGET;
        targetGameObject = stateMachine.GetOwner();
        currentPosition = targetGameObject.transform.position;
    }
    public override void Update() {
        UpdatePosition();
    }
    public override bool ShouldTransition() {
        if (currentPosition == targetPosition)
            return true;
        return false;
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

    public void SetTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }
    public void SetSpeed(float speed) {
        this.speed = speed;
    }
    public void SetSnapDistance(float distance) {
        snapDistance = distance;
    }
}
