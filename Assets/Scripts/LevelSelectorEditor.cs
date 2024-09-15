using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LevelManager))]
public class LevelSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelManager levelManagerTarget = (LevelManager)target;
        DrawDefaultInspector();

        if (GUILayout.Button("start game"))
        {
            levelManagerTarget.StartGame();
        }

        if (GUILayout.Button("level zero"))
        {
            levelManagerTarget.ChangeLevel(0);
        }

        if (GUILayout.Button("level one"))
        {
            levelManagerTarget.ChangeLevel(1);
        }

        if (GUILayout.Button("level two"))
        {
            levelManagerTarget.ChangeLevel(2);
        }

        if (GUILayout.Button("level three"))
        {
            levelManagerTarget.ChangeLevel(3);
        }

        if (GUILayout.Button("Hide levels"))
        {
            levelManagerTarget.HideLevels();
        }

        if (GUILayout.Button("Main Menu"))
        {
            levelManagerTarget.DisplayMainMenu();
        }

        if (GUILayout.Button("Trigger Victory"))
        {
            levelManagerTarget.OnPlayerFinishesGame();
        }
    }
}
