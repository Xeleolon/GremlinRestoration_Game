using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "GremlinsTeam/Level Data")]
public class LevelData : ScriptableObject
{
    [Tooltip("name after the actual scene name e.g LevelOne")]
    new public string name = "New LevelData";

    [Tooltip("Fancy name of the level")]
    public string levelName = "";

    public string nextLevel;
    public string MenuName = "MainMenu";
    [Tooltip("number of tools available at start")]
    public int numTools = 3;

    public int numKeys = 0;
    public Item[] items = new Item[4];
    public int[] itemNumber = new int[4];
}
