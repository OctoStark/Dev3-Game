using UnityEngine;

[CreateAssetMenu]
public class KeyItem : ScriptableObject
{
    public string keyID;
    public string displayName;
    public GameObject itemVisual;
    public Sprite icon;
}
