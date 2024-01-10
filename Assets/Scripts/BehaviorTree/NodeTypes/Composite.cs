using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : Node {

    protected List<Node> ConnectedNodes = new List<Node>();


    public virtual void ConnectNode(Node node) {
        if (node == null) {
            Debug.LogWarning("Attempted to connect invalid node to Composite");
            return;
        }

        ConnectedNodes.Add(node);
    }
}
