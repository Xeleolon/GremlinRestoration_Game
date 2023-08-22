using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GremlinsTeam/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public GameObject dropModel;

}