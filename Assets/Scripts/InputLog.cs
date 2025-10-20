using UnityEngine;

public class InputLog : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            Debug.Log("A is pressed");

        if (Input.GetKey(KeyCode.D))
            Debug.Log("D is pressed");

        if (Input.GetMouseButtonDown(0))
            Debug.Log("Left Click");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Right Click");
    }
}
