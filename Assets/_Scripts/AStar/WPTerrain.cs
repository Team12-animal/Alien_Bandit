using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WPTerrain
{
    public List<PathNode> nodeList;
    public GameObject[] obstacles;

    public void Init()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        nodeList = new List<PathNode>();
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("WP");

        foreach (GameObject node in nodes)
        {
            PathNode p = new PathNode();
            p.total = p.toThis = p.toGoal = 0.0f;
            p.parent = null;
            p.neibors = new List<PathNode>();
            p.pos = node.transform.position;
            p.go = node;
            nodeList.Add(p);
        }

        LoadWP();
    }

    public void ClearAStarInfo()
    {
        foreach (PathNode node in nodeList)
        {
            node.parent = null;
            node.toThis = 0.0f;
            node.toGoal = 0.0f;
            node.total = 0.0f;
            node.nodeState = PathNodeState.NODE_NONE;
        }
    }

    public void LoadWP()
    {
        StreamReader sr = new StreamReader("Assets/aStarNode.txt");
        if (sr == null)
        {
            return;
        }

        sr.Close();

        TextAsset ta = Resources.Load("aStarNode") as TextAsset;

        Debug.Log("ta.text" + ta == null);

        string all = ta.text;
        string[] lines = all.Split('\n');
        int lineAmt = lines.Length;
        int lineIndex = 0;

        while (lineIndex < lineAmt)
        {
            string s = lines[lineIndex];
            lineIndex++;
            string[] ss = s.Split(' ');
            PathNode current = null;
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].go.name.Equals(ss[0]))
                {
                    current = nodeList[i];
                    break;
                }
            }

            if (current == null)
            {
                continue;
            }

            int nei = int.Parse(ss[1]);
            int index = 2;

            for (int i = 0; i < nei; i++)
            {
                string name = ss[index + i];
                for (int j = 0; j < nodeList.Count; j++)
                {
                    if (nodeList[i].go.name.Equals(name))
                    {
                        current.neibors.Add(nodeList[j]);
                        break;
                    }
                }
            }
        }
    }

    public PathNode GetNodeFromPos(Vector3 pos)
    {
        PathNode rnode = null;
        PathNode node;
        int nodeAmt = nodeList.Count;
        float max = 100000.0f;

        for (int i = 0; i < nodeAmt; i++)
        {
            node = nodeList[i];

            if (Physics.Linecast(pos, node.pos, 1 << 8 | 1 << 15))
            {
                continue;
            }

            Vector3 vec = node.pos - pos;
            vec.y = 0.0f;
            float dist = vec.magnitude;

            if(dist < max)
            {
                max = dist;
                rnode = node;
            }
        }

        return rnode;
    }
}
