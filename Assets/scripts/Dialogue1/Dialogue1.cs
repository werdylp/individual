using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue1
{
    public string name;
    public List<DialogueNode> nodes;

    public void OnValidate()
    {
        if (nodes == null) return;

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].index = i;
        }
    }
}
