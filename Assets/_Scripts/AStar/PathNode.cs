using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum PathNodeState
{
    NODE_NONE = -1,
    NODE_OPENED,
    NODE_CLOSED
};

public class PathNode
{
    public GameObject go;
    public List<PathNode> neibors;

    public Vector3 pos;
    public PathNode parent;
    public float toThis;
    public float toGoal;
    public float total;

    public PathNodeState nodeState;
}
