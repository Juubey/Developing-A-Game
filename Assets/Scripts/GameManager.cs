using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over!");
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [InitializeOnLoad]
    public static class FixResolutionScale
    {
        static FixResolutionScale()
        {
            EditorApplication.playModeStateChanged += OnPlayStateChanged;
            SetGameViewScale();
        }

        private static void OnPlayStateChanged(PlayModeStateChange playModeStateChange)
        {
            SetGameViewScale();
        }

        private static void SetGameViewScale()
        {
            Type gameViewType = GetGameViewType();
            EditorWindow gameViewWindow = GetGameViewWindow(gameViewType);

            if (gameViewWindow == null)
            {
                Debug.LogError("GameView is null!");
                return;
            }

            var defScaleField = gameViewType.GetField("m_defaultScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //whatever scale you want when you click on play
            float defaultScale = 0.1f;

            var areaField = gameViewType.GetField("m_ZoomArea", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var areaObj = areaField.GetValue(gameViewWindow);

            var scaleField = areaObj.GetType().GetField("m_Scale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            scaleField.SetValue(areaObj, new Vector2(defaultScale, defaultScale));
        }

        private static Type GetGameViewType()
        {
            Assembly unityEditorAssembly = typeof(EditorWindow).Assembly;
            Type gameViewType = unityEditorAssembly.GetType("UnityEditor.GameView");
            return gameViewType;
        }

        private static EditorWindow GetGameViewWindow(Type gameViewType)
        {
            Object[] obj = Resources.FindObjectsOfTypeAll(gameViewType);
            if (obj.Length > 0)
            {
                return obj[0] as EditorWindow;
            }
            return null;
        }
    }
}
