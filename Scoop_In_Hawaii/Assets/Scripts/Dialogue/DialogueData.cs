using System;

[Serializable]
public class DialogueData
{
    public DialogueJson[] dialogues;
}

[Serializable]
public class DialogueJson
{
    public string id;
    public string character;
    public string[] lines;
}