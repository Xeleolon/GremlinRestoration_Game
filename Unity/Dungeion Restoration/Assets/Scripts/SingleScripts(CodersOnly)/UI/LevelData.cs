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
}
