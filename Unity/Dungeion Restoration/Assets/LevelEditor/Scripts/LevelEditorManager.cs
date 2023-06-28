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
    LEAssetData movingData;
    LEAssetData homeData;


    [MenuItem("Window/Level Editor")] //Creates the window for this object
    public static void ShowWindow ()
    {
        EditorWindow.GetWindow<LevelEditorManager>("Level Editor");
    }

    void OnGUI()
    {
        UpdateSelection();
        //Examples From Research and learning!
        myString = EditorGUILayout.TextField("Name", myString);

        GUILayout.Label(" Testing Coloizing.", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);
        if (GUILayout.Button("Empty Objects"))
        {
            if (!changeMove)
            {
                moveObject = null;
            }

            if (!changeHome)
            {
                homeObject = null;
            }
        }

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

    void UpdateSelection()
    {
        if (Selection.transforms.Length >= 2 && Selection.gameObjects[0] != null && Selection.gameObjects[1] != null)
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
        else if (Selection.transforms.Length >= 1 && Selection.gameObjects[0] != null)
        {
            if (!changeMove)
            {
                moveObject = Selection.gameObjects[0];
            }
            else if (!changeHome)
            {
                homeObject = Selection.gameObjects[0];
            }
        }
    }
    void SnapButton()
    {
        if (moveObject != null && homeObject != null)
        {
            LEAssetData movingData = moveObject.GetComponent<LEAssetData>();
            LEAssetData homeData = homeObject.GetComponent<LEAssetData>();
            if (GUILayout.Button("Testing Orgin"))
            {
                SnapStraight(testingMove, testingHome);
            }

            GUILayout.Label("Snap Objects", EditorStyles.boldLabel);
            SnapAlongY();
            SnapAlongX();
            SnapAlongZ();
        }

    }

    void SnapAlongX()
    {
        
        if (GUILayout.Button("Snap X Postive"))
        {
            float height = 0;
            if (movingData != null)
            {
                height = movingData.faceNegativeX;
            }

            if (homeData != null && homeData.facePostiveX <= 0)
            {
                height = homeData.facePostiveX - height;
            }

            SnapStraight(new Vector3(0.5f, 0.5f + height, 0), new Vector3(0.5f, -0.5f, 0));
        }
        

        if (GUILayout.Button("Snap X Negative"))
        {
            float height = 0;
            if (movingData != null)
            {
                height = movingData.facePostiveX;
            }

            if (homeData != null && homeData.faceNegativeX <= 0)
            {
                height = homeData.faceNegativeX - height;
            }
            
            SnapStraight(new Vector3(-0.5f, 0.5f + height, 0), new Vector3(-0.5f, -0.5f, 0));
        }
    
    }

    void SnapAlongY()
    {

            if (GUILayout.Button("Top"))
            {
                SnapStraight(new Vector3(0, 0.5f, 0), new Vector3(0, 0.5f, 0));
            }
        

            
            if (GUILayout.Button("Bottom"))
            {
                SnapStraight(new Vector3(0, -0.5f, 0), new Vector3(0, -0.5f, 0));
            }
    }

    void SnapAlongZ()
    {
        if (GUILayout.Button("Snap Z Postive"))
        {
            float height = 0;
            if (movingData != null)
            {
                height = movingData.faceNegativeZ;
            }

            if (homeData != null && homeData.facePostiveZ <= 0)
            {
                height = homeData.facePostiveZ - height;
            }

            SnapStraight(new Vector3(0, 0.5f + height, 0.5f), new Vector3(0, -0.5f, 0.5f));
        }
        

        if (GUILayout.Button("Snap Z Negative"))
        {
            float height = 0;
            if (movingData != null)
            {
                height = movingData.facePostiveZ;
            }

            if (homeData != null && homeData.faceNegativeZ <= 0)
            {
                height = homeData.faceNegativeZ - height;
            }
            
            SnapStraight(new Vector3(0, 0.5f + height, -0.5f), new Vector3(0, -0.5f, -0.5f));
        }
    }

    void SnapStraight(Vector3 moving, Vector3 home)
    {
        Vector3 scaleMove = moveObject.transform.localScale;
        Vector3 scaleHome = homeObject.transform.localScale;
        
        //LEAssetData movingData = moveObject.GetComponent<LEAssetData>();
        //LEAssetData homeData = homeObject.GetComponent<LEAssetData>();

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
