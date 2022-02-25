using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WPSave : MonoBehaviour
{
    private void Start()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("WP");
        StreamWriter sw = new StreamWriter("Assets/aStarNode.txt", false);

        string s = "";

        for (int i = 0; i < nodes.Length; i++)
        {
            s = "";
            s += nodes[i].name;
            s += " ";
            WP wp = nodes[i].GetComponent<WP>();
            s += wp.neibors.Count;
            s += " ";

            for (int j = 0; j < wp.neibors.Count; j++)
            {
                s += wp.neibors[j].name;
                s += " ";
            }

            sw.WriteLine(s);
        }

        sw.Close();
    }
}
