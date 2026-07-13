using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameInputActions inputActions;

    public event Action ContextMenuRequested;
    public event Action CloseUIRequested;

    private void Awake()
    {
        inputActions = new GameInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.GamePlay.OpenContextMenu.performed += OnOpenContextMenu;
        inputActions.GamePlay.CloseUI.started += OnCloseUI;
    }

    private void OnDisable()
    {
        inputActions.GamePlay.OpenContextMenu.performed -= OnOpenContextMenu;
        inputActions.GamePlay.CloseUI.started -= OnCloseUI;
        inputActions.Disable();
    }

    private void OnOpenContextMenu(InputAction.CallbackContext context)
    {
        ContextMenuRequested?.Invoke();
    }
    private void OnCloseUI(InputAction.CallbackContext context)
    {
        CloseUIRequested?.Invoke();
    }
}