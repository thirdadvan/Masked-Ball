using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueNodeType
{
    PlayerOption,
    NpcReply,
    SystemRemark
}

[Serializable]
public class DialogueTree
{
    public string startNodeId;
    public List<DialogueNode> nodes = new();
}

[Serializable]
public class DialogueNode
{
    public string nodeId;
    public DialogueNodeType type;

    [TextArea] public string text;

    public List<string> next = new();
    public List<string> requiredEvidenceIds = new();
}
