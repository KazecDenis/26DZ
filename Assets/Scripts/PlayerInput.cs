using UnityEngine;

public class PlayerInput
{
    private const string _horizontalInputKey = "Horizontal";
    public float HorizontalInput => Input.GetAxisRaw(_horizontalInputKey);
    public bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
}

