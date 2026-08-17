using UnityEngine;

[System.Serializable]
public class Client
{
    public string clientName;

    public Sprite sideSprite;

    public Sprite frontSprite;

    public float maxPatience = 30f;

    [HideInInspector] public float currentPatience;
}
