using System.Collections;
using UnityEngine;
using UnityEditor;

public class LevelEditorManager : EditorWindow //The Editor Window for snapping object to each other
{
    //Examples from 
    Color color;
    string myString = "hello, WOrld!";
    GameObject moveObject;
    bool changeMove = false;
    GameObject homeObject;
    bool changeHome = false;
    Vector3 testingMove;
    Vector3 testingHome;


    [MenuItem("Window/Level Editor")] //Creates the window for this object
    public static void ShowWindow ()
    {
        EditorWindow.GetWindow<LevelEditorManager>("Level Editor");
    }

    void OnGUI()
    {
        //Examples From Research and learning!
        myString = EditorGUILayout.TextField("Name", myString);

        GUILayout.Label(" Testing Coloizing.", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        moveObject = EditorGUILayout.ObjectField("Moving", moveObject, typeof(GameObject), true) as GameObject;
        changeMove = EditorGUILayout.Toggle("Lock Moving", changeMove);

        homeObject = EditorGUILayout.ObjectField("Moving To", homeObject, typeof(GameObject), true) as GameObject;
        changeHome = EditorGUILayout.Toggle("Lock Home", changeHome);

        testingMove = EditorGUILayout.Vector3Field("Testing moving", testingMove);
        testingHome = EditorGUILayout.Vector3Field("Testing home", testingHome);

        if (GUILayout.Button("Colorise!"))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial.color = color;
                }
            }
        }

        
        SnapButton();
    }


    void SnapButton()
    {
        if (moveObject != null && homeObject != null)
        {
            if (GUILayout.Button("Testing Orgin"))
            {
                SnapStraight(testingMove, testingHome);
            }
            SnapAlongX();
            SnapAlongZ();
            SnapAlongZ();
        }
        else if (Selection.transforms.Length >= 2 && Selection.gameObjects[0] != null && Selection.gameObjects[1] != null)
        {
            if (!changeMove)
            {
                moveObject = Selection.gameObjects[0];
            }
            
            if (!changeHome) 
            {
                homeObject = Selection.gameObjects[1];
            }
        }

    }

    void SnapAlongX()
    {
        if (moveObject.transform.position.x >= homeObject.transform.position.x)
        {
            GUILayout.Label("Snap Object X Postive", EditorStyles.boldLabel);

            if (GUILayout.Button("Orgin"))
            {
                SnapStraight(new Vector3(0.5f, 0, 0), new Vector3(0.5f, 0, 0));
            }
        }
        
        if(moveObject.transform.position.x <= homeObject.transform.position.x)
        {
            GUILayout.Label("Snap Object X Negative", EditorStyles.boldLabel);

            if (GUILayout.Button("Snap X Negative"))
            {
                SnapStraight(new Vector3(-0.5f, 0, 0), new Vector3(-0.5f, 0, 0));
            }
        }
    }

    void SnapAlongY()
    {
        
    }

    void SnapAlongZ()
    {
        
    }

    void SnapStraight(Vector3 moving, Vector3 home)
    {
        Vector3 scaleMove = moveObject.transform.localScale;
        Vector3 scaleHome = homeObject.transform.localScale;
        
        LEAssetData movingData = moveObject.GetComponent<LEAssetData>();
        LEAssetData homeData = homeObject.GetComponent<LEAssetData>();

        if (movingData != null)
        {
            scaleMove = Vector3.Scale(scaleMove, Vector3.Scale(movingData.alterScale, movingData.size)); //updates the scale to any changes
        }

        if (homeData != null)
        {
             scaleHome = Vector3.Scale(scaleHome, Vector3.Scale(homeData.alterScale, homeData.size));
        }

        scaleMove = Vector3.Scale(moving, scaleMove);

        scaleHome = Vector3.Scale(home, scaleHome);

        Vector3 moveTo = homeObject.transform.position;
        
        moveTo += scaleHome + scaleMove;
            
        moveObject.transform.position = moveTo;
    }
}
