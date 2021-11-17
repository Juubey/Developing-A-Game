using UnityEngine;
using UnityEditor;

public class ExampleWindow : EditorWindow
{

string myString = "Hello World!";

[MenuItem("Window/Example")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Example");
        
    }


void OnGUI()
    {
    //Window Code

    GUILayout.Label("This is a label.", EditorStyles.boldLabel);


    myString = EditorGUILayout.TextField("Name", myString);

        if (GUILayout.Button("Press Me."))
        {
            Debug.Log("Button Was Pressed.");
        }
    }


}
