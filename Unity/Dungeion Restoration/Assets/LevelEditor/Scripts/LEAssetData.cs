using UnityEngine;


public class LEAssetData : MonoBehaviour
{
    new public string name = "New Asset Data";
    [Tooltip("The unity of size of the actual object built in your program before insert to unity")]
    public Vector3 size = Vector3.one;

    [Tooltip("Has the scale overall beeing alter in the orginal assets before becoming a prefab")]
    public Vector3 alterScale = Vector3.one;

    [Tooltip("if the pivotpoint isn't zero provide difference from center please")]
    public Vector3 pivotPoint = Vector3.zero;

    [Header("Height of Floor from bottom of object")]
    public float facePostiveX = 0;
    public float facePostiveZ = 0;
    public float faceNegativeX = 0;
    public float faceNegativeZ = 0;


}
