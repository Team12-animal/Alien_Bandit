using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    WPTerrain terrain;

    List<PathNode> openList;
    List<PathNode> closeList;

    List<Vector3> pathList;

    static public AStar instance;

    public void Init(WPTerrain terrain)
    {
        this.terrain = terrain;
        openList = new List<PathNode>();
        closeList = new List<PathNode>();
        pathList = new List<Vector3>();
        instance = this;
    }

    public List<Vector3> GetPath()
    {
        return pathList;
    }

    private bool CheckCloseList(PathNode node)
    {
        foreach (PathNode n in closeList)
        {
            if (n == node)
            {
                return true;
            }
        }

        return false;
    }

    private PathNode GetBestNode()
    {
        PathNode rn = null;
        float max = 10000.0f;

        foreach (PathNode n in openList)
        {
            if (n.total < max)
            {
                max = n.total;
                rn = n;
            }
        }

        openList.Remove(rn);
        return rn;
    }

    private void BuildPath(Vector3 sPos, Vector3 ePos, PathNode sNode, PathNode eNode)
    {
        pathList.Clear();
        pathList.Add(sPos);

        if (sNode == eNode)
        {
            pathList.Add(sNode.pos);
        }
        else
        {
            PathNode pNode = eNode;
            while (pNode != null)
            {
                pathList.Insert(1, pNode.pos);
                pNode = pNode.parent;
            }
        }

        pathList.Add(ePos);
    }

    public bool PerformAStar(Vector3 sPos, Vector3 ePos)
    {
        PathNode sNode = terrain.GetNodeFromPos(sPos);
        PathNode eNode = terrain.GetNodeFromPos(ePos);

        if (sNode == null || eNode == null)
        {
            return false;
        }
        else if (sNode == eNode)
        {
            BuildPath(sPos, ePos, sNode, eNode);
            return true;
        }

        openList.Clear();
        closeList.Clear();
        terrain.ClearAStarInfo();

        PathNode nNode;
        PathNode cNode;

        openList.Add(sNode);

        Debug.Log("astar" + openList.Count);

        while (openList.Count > 0)
        {
            cNode = GetBestNode();

            Debug.Log(openList.Count + "astar a" + cNode.go.name + sNode.go.name + eNode.go.name);

            if (cNode == null)
            {
                return false;
            }
            else if (cNode.go == eNode.go)
            {
                Debug.Log("astar bbb");
                BuildPath(sPos, ePos, sNode, eNode);
                return true;
            }

            int nei = cNode.neibors.Count;
            Vector3 dist;

            for (int i = 0; i < nei; i++)
            {
                nNode = cNode.neibors[i];

                if (nNode.nodeState == PathNodeState.NODE_CLOSED)
                {
                    continue;
                }
                else if (nNode.nodeState == PathNodeState.NODE_OPENED)
                {
                    dist = cNode.pos - nNode.pos;
                    float newG = cNode.toThis + dist.magnitude;

                    if (newG < nNode.toThis)
                    {
                        nNode.toThis = newG;
                        nNode.total = nNode.toThis + nNode.toGoal;
                        nNode.parent = cNode;
                    }

                    continue;
                }

                nNode.nodeState = PathNodeState.NODE_OPENED;
                dist = cNode.pos - nNode.pos;
                nNode.toThis = cNode.toThis + dist.magnitude;
                dist = eNode.pos - nNode.pos;
                nNode.toGoal = dist.magnitude;
                nNode.total = nNode.toThis + nNode.toGoal;
                nNode.parent = cNode;
                openList.Add(nNode);

                Debug.Log(openList.Count + "astar n " + nNode.go.name);
            }
            cNode.nodeState = PathNodeState.NODE_CLOSED;
        }

        return false;
    }
}
