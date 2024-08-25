using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public PlayerControls playerControls { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerControls();
    }

    public void InitializePlayerControls()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }
}
