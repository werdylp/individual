using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [HideInInspector]
    public int index;

    [TextArea]
    public string text;

    public int nextIndex = -1;
    public List<DialogueChoice> choices;
}
