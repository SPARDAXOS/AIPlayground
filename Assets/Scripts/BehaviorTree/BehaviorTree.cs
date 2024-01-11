using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree {

    public enum ExecutionResults {
        ERROR = 0,
        FAILURE,
        SUCCESS,
        RUNNING
    }
    public enum EvaluationResults {
        ERROR = 0,
        FAILURE,
        SUCCESS
    }


    private ExecutionResults CurrentExecutionResults = ExecutionResults.ERROR;
    private EvaluationResults CurrentEvaluationResults = EvaluationResults.ERROR;

    private string CurrentTaskName = "None";


    private Root RootNode = new Root();
    private Blackboard MainBlackboard = new Blackboard();


    private Node CurrentNode = null;
    private Task CurrentRunningTask = null;
    private Composite CurrentLastTickedComposite = null;

    private float EvaluationFrequency = 0.2f;
    private float ExecutionFrequency  = 0.01f;

    private float EvaluationTimer = 0.0f;
    private float ExecutionTimer  = 0.0f;


    //Notes!
    //What is the purpose of evaluate and the purpose of execute.

    //The main goal of evaluate is to check which last ticked composite should be executed and if it is the same as the current -
    //- to check for interruptions.

    //The main goal of execute is to run the last ticked composite in case it is set to something valid.

    //The goal of tasks is to report if they can be ran or not and SetCurrentTaskName



    public void Update() {
        Evaluate();
        Execute();
        UpdateTimers();
    }
    private void Evaluate() {
        if (EvaluationTimer < EvaluationFrequency)
            return;

        CurrentEvaluationResults = RootNode.Evaluate(this);
        if (CurrentNode == null) {
            Debug.LogError("CurrentNode was not valid after evaluation!");
            return;
        }

        if (CurrentEvaluationResults == EvaluationResults.SUCCESS) {
            if (CurrentNode == CurrentRunningTask)
                return; //You are good to go. 

            //This here is whats making it complicated currently.
            //The implication is who sets which taks is running. 

            //Case:
            //  If a new task should be running instead.
            //Steps:
            //  -Interrupt old task
            //  -Set new running task

            if (CurrentRunningTask != null) {
                CurrentRunningTask.Interrupt();
                Debug.Log("Running task was interrupted due to another task succeding in evaluation!");
            }

            //This is kinda sus. Running task and running composite. It feels like its working by a fluke
            SetCurrentTask((Task)CurrentNode);
            Debug.Log("New task started running - " + CurrentRunningTask.GetName());
        }
        else if (CurrentEvaluationResults == EvaluationResults.ERROR) {

            //Case:
            //  If an error was received during evaluation. (Errors are critical and should stop the tree from running)
            //Steps:
            //  -If running task was valid then stop it and set to null.
            //  -Display message regarding the error

            if (CurrentRunningTask == null)
                Debug.LogError("Error was reported during evaluation! \nTask was interrupted");
            else if (CurrentRunningTask == CurrentNode) {
                Debug.LogError("Running task reported error during evaluation! \nTask was interrupted");
                CurrentRunningTask.Interrupt();
                CurrentRunningTask = null;
                CurrentTaskName = "None";
            }
            else {
                Debug.LogError("Error reported while a task is running during evaluation! \nTask was interrupted");
                CurrentRunningTask.Interrupt();
                CurrentRunningTask = null;
                CurrentTaskName = "None";
            }
        }
        else if (CurrentEvaluationResults == EvaluationResults.FAILURE) {

            //Case:
            //  If failure was received during evaluation. (Either couldnt run any behaviors or current running behavior was interrupted)
            //Steps:
            //  -If running task was valid then stop it and set to null.
            //  -idk


            if (CurrentRunningTask == null) {
                Debug.Log("Evaluation reported that no behaviors can be performed!");
                return;
            }
            else if (CurrentRunningTask == CurrentNode) {
                Debug.Log("Evaluation reported that the running task failed \nTask was stopped");
                CurrentRunningTask.Interrupt();
                CurrentRunningTask = null;
                CurrentTaskName = "None";
            }
            else {
                Debug.Log("Evaluation reported that the running task was interrupted and failure was returned \nTask was stopped");
                CurrentRunningTask.Interrupt();
                CurrentRunningTask = null;
                CurrentTaskName = "None";
            }
        }

        EvaluationTimer -= EvaluationFrequency;
    }
    private void Execute() {
        if (ExecutionTimer < ExecutionFrequency)
            return;

        if (CurrentRunningTask == null)
            return;

        CurrentExecutionResults = CurrentLastTickedComposite.Execute(this);
        if (CurrentExecutionResults == ExecutionResults.ERROR) {

            //Case: Critical error occured during behavior. (Dependency missing or major error, should stop immediately!)
            //Steps: Stop current behavior.
            //Note: The worry would be if the evaluation sets the same behavior as the running one after this. (Evaluation for task should report error same as here then!)

            Debug.LogError("Execution reported error! \nRunning task was interrupted");
            CurrentRunningTask.Interrupt();
            CurrentRunningTask = null;
            CurrentTaskName = "None";
        }
        else if (CurrentExecutionResults == ExecutionResults.FAILURE) {

            //Case: Running task failed during execution (Condition not being met, should stop immediately!)
            //Steps: Stop current behavior.

            Debug.Log("Execution reported failure! \nRunning task was interrupted");
            CurrentRunningTask.Interrupt();
            CurrentRunningTask = null;
            CurrentTaskName = "None";
        }
        else if (CurrentExecutionResults == ExecutionResults.SUCCESS) {

            //Case: Task finished successfully
            //Steps: Invalidate the reference to running task

            Debug.Log("Execution reported success! \nRunning task was finished");
            CurrentRunningTask = null;
            CurrentTaskName = "None";
        }
        else if (CurrentExecutionResults == ExecutionResults.RUNNING) {

            //Case: Task is still running
            //Steps: nothing

            Debug.Log("Execution reported running! \nTask is still running");
        }

        ExecutionTimer -= ExecutionFrequency;
    }

    private void UpdateTimers() {
        UpdateEvaluationTimer();
        UpdateExecutionTimer();
    }
    private void UpdateEvaluationTimer() {
        if (EvaluationTimer >= EvaluationFrequency)
            return;

        EvaluationTimer += Time.deltaTime;
    }
    private void UpdateExecutionTimer() {
        if (ExecutionTimer >= ExecutionFrequency)
            return;
        if (CurrentRunningTask == null) //Maybe reset the timer in case the ref was invalidated!
            return;

        ExecutionTimer += Time.deltaTime;
    }


    public void SetEvaluationFrequency(float value) { EvaluationFrequency = value; }
    public void SetExecutionFrequency(float value) { ExecutionFrequency = value; }
    public void SetCurrentTaskName(string name) {  CurrentTaskName = name; }

    public void SetCurrentNode(Node node) { CurrentNode = node; }
    public void SetCurrentTask(Task task) { CurrentRunningTask = task; }
    public void SetLastTickedComposite(Composite composite) { CurrentLastTickedComposite = composite; }


    public Root GetRoot() { return RootNode; }
    public Blackboard GetBlackboard() { return MainBlackboard; }
    public string GetCurrentTaskName() { return CurrentTaskName; }
}
