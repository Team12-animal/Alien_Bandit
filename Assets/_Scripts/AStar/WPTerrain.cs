using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WPTerrain
{
    public List<PathNode> nodeList;
    public GameObject[] obstacles;

    public void Init(string txtName, string nodeName)
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        nodeList = new List<PathNode>();
        GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeName);

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

        LoadWP(txtName);
        Debug.Log($"wpt init text{txtName}node{nodeName} amt{nodes.Length}");
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

    public void LoadWP(string txtName)
    {
        string path = $"/{txtName}.txt";
        string all = File.ReadAllText(Application.streamingAssetsPath + path);

        Debug.Log($"wp load with io {all == null}");

        if (all == null)
        {
            all = (Resources.Load(txtName) as TextAsset).text;
            Debug.Log($"wp load resources.load {all == null}");
        }

        Debug.Log($"wp load result{all == null}");

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
                    if (nodeList[j].go.name.Equals(name))
                    {
                        current.neibors.Add(nodeList[j]);
                        break;
                    }
                }
            }

            string print = $"{current.go.name} ";

            foreach (PathNode p in current.neibors)
            {
                print += p.go.name;
            }

            Debug.Log($"wpt line: neinum {nei}, nodes {print}");
        }
    }

    public PathNode GetNodeFromPos(Vector3 pos)
    {
        PathNode rnode = null;
        PathNode node;
        int nodeAmt = nodeList.Count;
        float min = 1000000000000.0f;

        for (int i = 0; i < nodeAmt; i++)
        {
            node = nodeList[i];

            //if (Physics.Linecast(pos, node.pos, 1 << 8))
            //{
            //    continue;
            //}

            Vector3 vec = node.pos - pos;
            vec.y = 0.0f;
            float dist = vec.magnitude;

            if(dist < min)
            {
                min = dist;
                rnode = node;
            }
        }

        return rnode;
    }
}
