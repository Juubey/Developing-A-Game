using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text text;
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    [SerializeField]
    PanelSettings DebugBox;
    [SerializeField]
    PanelSettings DebugCommands;

    bool showConsole;
    bool showHelp;
    string input;

    public static DebugCommand<int> set_hp;
    public static DebugCommand HELP;
    public List<object> commandList;

    Playerinputs debugInput;

    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
        
        var uiDocument = GetComponent<UIDocument>(); // The UXML is already instantiated by the UIDocument component
        //DebugBox = GetComponent<PanelSettings>();
        //DebugCommands = GetComponent<PanelSettings>();

        debugInput = new Playerinputs();

        HELP = new DebugCommand("help", "Shows a list of commands", "help", () =>
        {
            showHelp = true;
        });

        commandList = new List<object>
        {
            HELP,
        };
        //BuildCommandList();   //builds the list of commands for debugging - not a part of the input system/actions/etc circus
        
    }

    void OnEnable()
    {
        //GameObject.Find("DebugInfoBox").SetActive(true);
        //GameObject.Find("DebugCommand").SetActive(true);
        //debugInput.Debug.Enable();
    }
    private void OnDisable()
    {
        //GameObject.Find("DebugInfoBox").SetActive(false);
        //GameObject.Find("DebugCommand").SetActive(false);
        //debugInput.Debug.Disable();
    }

    public void OnToggleDebug()
    {
        if (Keyboard.current.backquoteKey.wasPressedThisFrame && showConsole == false)
        {
            showConsole = !showConsole;
            OnEnable();
            //HandleInput();
            //input = "";
            Debug.Log("` was pressed.");
            Pause();
        }
        else if (Keyboard.current.backquoteKey.wasPressedThisFrame && showConsole == true)
        {
            showConsole = !showConsole;
            OnDisable();
            Debug.Log("` was pressed." + "showConsole is " + showConsole);
            Resume();
            return;
        }
    }

    

    /*
    #region magic numbers GUI
    private void OnGUI()
    {
        if(!showConsole) { return;}

        float y = 0f;
        
        if(showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 30), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
            
            for(int i=0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);

            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

    }
    #endregion*/

    private void HandleInput() 
    {
        string[] properties = input.Split(' ');


        for(int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    //Cast to this type and invoke the command
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));

                }
            }
        }
    }
    
    
    private float CalculateFps() 
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray) {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        text.text = Mathf.RoundToInt(CalculateFps()).ToString();
        OnToggleDebug();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    void Pause()
    {
        Time.timeScale = 0f;
    }
}
