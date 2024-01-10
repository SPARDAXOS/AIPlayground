using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : Node {

    protected Node ConnectedNode = null;

    public void ConnectNode(Node node) {
        if (node == null) {
            Debug.LogWarning("Attempted to connect invalid node to Decorator");
            return;
        }

        ConnectedNode = node;
    }
}
