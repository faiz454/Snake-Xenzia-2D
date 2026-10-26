using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private int horizontal = 0, vertical = 0;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        horizontal = 0;
        vertical = 0;

        GetKeyboardInput();
        SetMovement();
    }

    void GetKeyboardInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Check horizontal keys
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            horizontal = 1;
        else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            horizontal = -1;

        // Check vertical keys
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            vertical = 1;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            vertical = -1;

        // Prevent diagonal movement
        if (horizontal != 0)
        {
            vertical = 0;
        }
    }

    void SetMovement()
    {
        if (vertical != 0)
        {
            playerController.SetInputDirection((vertical == 1) ?
                  PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if (horizontal != 0)
        {
            playerController.SetInputDirection((horizontal == 1) ?
                  PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

} // This is the final bracket that closes the class
