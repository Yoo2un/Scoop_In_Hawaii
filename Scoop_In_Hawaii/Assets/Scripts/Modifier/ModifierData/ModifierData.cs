using UnityEngine;

[CreateAssetMenu(fileName = "NewModifierData", menuName = "ScriptableObjects/ModifierData")]
public class ModifierData : ScriptableObject
{
    public string modifierName;
    public ModifierType modifierType;
}