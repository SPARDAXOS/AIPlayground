using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : Node {
    public enum TaskStatus {
        NOT_RUNNING,
        RUNNING
    }

    protected TaskStatus CurrentTaskStatus = TaskStatus.NOT_RUNNING;
    protected string Name = "Task name unspecified!";

    public abstract void Interrupt();
    public TaskStatus GetStatus() { return CurrentTaskStatus; }
    public string GetName() { return Name; }
}
