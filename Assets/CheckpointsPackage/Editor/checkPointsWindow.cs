using checkPointsManager.runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace checkPointsManager.window
{
    public class CheckpointWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private Texture2D texture;
        private List<CheckPoint> checkPoints = new List<CheckPoint>();

        [MenuItem("Tools/CheckPoints Manager")]
        static void ShowEditor()
        {
            var wn = EditorWindow.GetWindow<CheckpointWindow>("Checkpoints Manager");
            wn.minSize = new Vector2(200, 300);
        }

        private void findHeader()
        {
            string[] files = System.IO.Directory.GetFiles(Application.dataPath, "00cb918a-bcda-11ed-afa1-0242ac120002.png", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(file.Substring(Application.dataPath.Length - 6));
            }
        }

        private GameObject findPrefab()
        {
            string[] files = System.IO.Directory.GetFiles(Application.dataPath, "b264a388-bce3-11ed-afa1-0242ac120002.prefab", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                return AssetDatabase.LoadAssetAtPath<GameObject>(file.Substring(Application.dataPath.Length - 6));
            }

            return null;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        private void OnFocus()
        {
            refreshCheckPoints();
        }

        private void refreshCheckPoints()
        {
            var go = FindObjectsOfType<GameObject>();
            checkPoints = go.SelectMany(x => x.GetComponentsInChildren<CheckPoint>()).Distinct().Reverse().ToList();
        }

        private void OnGUI()
        {
            if(texture == null)
            {
                findHeader();
            }

            if (ColorUtility.TryParseHtmlString("#1b1b1b", out Color color))
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), MakeTex(1, 1, color), ScaleMode.ScaleAndCrop, true);
            GUI.DrawTexture(new Rect(0, 10, Screen.width, Screen.width/(7)), texture, ScaleMode.ScaleAndCrop, true);

            GUILayout.Label("", GUILayout.Height(Screen.width / 7));
            GUILayout.Space(10);
            GUILayout.Label("Player :",  EditorStyles.boldLabel);
            PlayerInf.player = EditorGUILayout.ObjectField(PlayerInf.player, typeof(GameObject), true) as GameObject;
            if(PlayerInf.player != null && !PlayerInf.player.TryGetComponent<Player_Checkpoint>(out var pl))
            {
                if (EditorUtility.DisplayDialog("ERROR", "Player Don't have \"Player_Checkpoint\" component do you want to add it", "Yes", "No"))
                {
                    PlayerInf.player.AddComponent<Player_Checkpoint>();
                }
                else {
                    PlayerInf.player = null;
                };
            }     

            if (PlayerInf.player != null && PlayerInf.player.TryGetComponent<Player_Checkpoint>(out var playerCheckPoint))
            {

                if (GUILayout.Button("Add a new Checkpoint", GUILayout.Height(20)))
                {
                    var go = PrefabUtility.InstantiatePrefab(findPrefab()) as GameObject;
                    go.name = "Checkpoint";
                    go.GetComponent<CheckPoint>().Player = PlayerInf.player;
                    refreshCheckPoints();
                }
                GUILayout.Space(10);

                GUILayout.Label("Teleport Player to :",  EditorStyles.boldLabel);  
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                foreach (GameObject child in checkPoints.Where(x => x != null).Select(x => x.gameObject))
                {
                    if (GUILayout.Button(child.name, GUILayout.Height(20)))
                    {
                        playerCheckPoint.teleportToCheckpoint(child.GetComponent<CheckPoint>());
                    }
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("You need to assign a player to continue");
            }

       
        }

    }

    internal static class PlayerInf
    {
        [SerializeField]
        public static GameObject player;
    }

}