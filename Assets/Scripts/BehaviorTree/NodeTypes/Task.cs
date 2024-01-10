using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : Node {
    public enum TaskStatus {
        NOT_RUNNING,
        RUNNING
    }

    protected TaskStatus CurrentStatus = TaskStatus.NOT_RUNNING;

    public abstract void Interrupt();
    public TaskStatus GetStatus() { return CurrentStatus; }
}
