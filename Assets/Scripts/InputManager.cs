using UnityEngine;

public class InputManager : MonoBehaviour
{

    public InputData inputData;
    //public InputData_1 inputData_1;
    //public inputData_2 inputData_2;
    void Update()
    {
        WriteInputData();
    }

    void WriteInputData()
    {
        inputData.isPressed = Input.GetMouseButtonDown(0);
        inputData.isHeld = Input.GetMouseButton(0);
        inputData.isReleased = Input.GetMouseButtonUp(0);


        inputData.isPressed_1 = Input.GetMouseButtonDown(1);
        inputData.isHeld_1 = Input.GetMouseButton(1);
        inputData.isReleased_1 = Input.GetMouseButtonUp(1);

        inputData.isPressed_2 = Input.GetMouseButtonDown(2);
        inputData.isHeld_2 = Input.GetMouseButton(2);
        inputData.isReleased_2 = Input.GetMouseButtonUp(2);
    }
}
