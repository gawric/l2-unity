﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region InputActions
    // Mouse
    private InputAction _leftClickAction;
    private InputAction _rightClickAction;
    // Camera
    private InputAction _cameraAxisAction;
    private InputAction _zoomAxisAction;
    // Movements
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _nextTargetAction;
    private InputAction _sitAction;
    // UI
    private InputAction _inventoryAction;
    private InputAction _characterStatusAction;
    private InputAction _actionsAction;
    private InputAction _closeWindowAction;
    private InputAction _systemMenuAction;
    private InputAction _validateAction;

    #endregion

    #region InputValues
    [field: Header("Mouse")]
    // Mouse
    [field: SerializeField] public bool LeftClickDown { get; private set; }
    [field: SerializeField] public bool RightClickDown { get; private set; }
    [field: SerializeField] public bool RightClickUp { get; private set; }
    [field: SerializeField] public bool LeftClickHeld { get; private set; }
    [field: SerializeField] public bool RightClickHeld { get; private set; }

    // Camera
    [field: Header("Camera")]
    [field: SerializeField] public Vector2 CameraAxis { get; private set; }
    [field: SerializeField] public bool CameraMoving { get; private set; }
    [field: SerializeField] public bool TurnCamera { get; private set; }
    [field: SerializeField] public float ZoomAxis { get; private set; }

    // Movements
    [field: Header("Movements")]
    [field: SerializeField] public Vector2 MoveInput { get; private set; }
    [field: SerializeField] public bool Move { get; private set; }
    [field: SerializeField] public bool MoveForward { get; private set; }
    [field: SerializeField] public bool Jump { get; private set; }
    [field: SerializeField] public bool Attack { get; private set; }
    [field: SerializeField] public bool NextTarget { get; private set; }
    [field: SerializeField] public bool Sit { get; private set; }

    // UI
    [field: Header("UI")]
    [field: SerializeField] public bool OpenInventory { get; private set; }
    [field: SerializeField] public bool OpenCharacerStatus { get; private set; }
    [field: SerializeField] public bool OpenSystemMenu { get; private set; }
    [field: SerializeField] public bool OpenActions { get; private set; }
    [field: SerializeField] public bool CloseWindow { get; private set; }
    [field: SerializeField] public bool Validate { get; private set; }

    public InputAction[,] SkillbarActions { get; private set; }
    public bool[,] SkillbarInputs { get; private set; }

    #endregion

    private PlayerInput _playerInput;

    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void SetupInputActions()
    {
        _leftClickAction = _playerInput.actions["LeftClick"];
        _rightClickAction = _playerInput.actions["RightClick"];

        _cameraAxisAction = _playerInput.actions["CameraAxis"];
        _zoomAxisAction = _playerInput.actions["ZoomAxis"];

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _nextTargetAction = _playerInput.actions["NextTarget"];
        _attackAction = _playerInput.actions["Attack"];
        _sitAction = _playerInput.actions["Sit"];

        _inventoryAction = _playerInput.actions["Inventory"];
        _characterStatusAction = _playerInput.actions["CharacterStatus"];
        _actionsAction = _playerInput.actions["Actions"];
        _closeWindowAction = _playerInput.actions["CloseWindow"];
        _systemMenuAction = _playerInput.actions["SystemMenu"];
        _validateAction = _playerInput.actions["Validate"];

        SkillbarActions = new InputAction[5, 12];

        for (int skillbar = 1; skillbar <= 5; skillbar++)
        {
            for (int i = 1; i <= 12; i++)
            {
                SkillbarActions[skillbar - 1, i - 1] = _playerInput.actions[$"Shortcut{skillbar}-{i}"];
            }
        }

        SkillbarInputs = new bool[5, 12];
    }

    void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        CameraAxis = _cameraAxisAction.ReadValue<Vector2>();
        CameraMoving = CameraAxis.y != 0 || CameraAxis.x != 0;

        LeftClickDown = _leftClickAction.WasPerformedThisFrame();
        RightClickDown = _rightClickAction.WasPerformedThisFrame();
        RightClickUp = _rightClickAction.WasReleasedThisFrame();
        LeftClickHeld = _leftClickAction.IsPressed();
        RightClickHeld = _rightClickAction.IsPressed();

        CloseWindow = _closeWindowAction.WasPerformedThisFrame();
        Validate = _validateAction.WasPerformedThisFrame();

        if (!L2GameUI.Instance.MouseOverUI)
        {
            if (RightClickHeld && CameraMoving)
            {
                TurnCamera = true;
                L2GameUI.Instance.DisableMouse();
            }

            ZoomAxis = _zoomAxisAction.ReadValue<float>();
        }

        if (RightClickUp)
        {
            TurnCamera = false;
            L2GameUI.Instance.EnableMouse();
        }

        if (!ChatWindow.Instance.ChatOpened)
        {
            MoveInput = _moveAction.ReadValue<Vector2>();
            Jump = _jumpAction.WasPerformedThisFrame();
            Attack = _attackAction.WasPerformedThisFrame();
            NextTarget = _nextTargetAction.WasPerformedThisFrame();
            Sit = _sitAction.WasPerformedThisFrame();

            OpenCharacerStatus = _characterStatusAction.WasPerformedThisFrame();
            OpenInventory = _inventoryAction.WasPerformedThisFrame();
            OpenSystemMenu = _systemMenuAction.WasPerformedThisFrame();
            OpenActions = _actionsAction.WasPerformedThisFrame();
        }
        else
        {
            MoveInput = Vector2.zero;
        }

        MoveForward = LeftClickHeld && RightClickHeld;
        Move = MoveInput.y != 0 || MoveInput.x != 0 || MoveForward;

        for (int skillbar = 0; skillbar < 5; skillbar++)
        {
            for (int i = 0; i < 12; i++)
            {
                SkillbarInputs[skillbar, i] = SkillbarActions[skillbar, i].WasPerformedThisFrame();
            }
        }
    }

    // private void UpdateInputsOld()
    // {
    //     if (!L2GameUI.Instance.MouseOverUI)
    //     {
    //         if (IsInputPressed(InputType.RightMouseButton) && IsInputPressed(InputType.MouseMoving))
    //         {
    //             UpdateInput(InputType.TurnCamera, true);
    //             L2GameUI.Instance.DisableMouse();
    //         }

    //         scrollAxis = Input.GetAxis("Mouse ScrollWheel");
    //         UpdateInput(InputType.Zoom, scrollAxis != 0);

    //         UpdateInput(InputType.LeftMouseButtonDown, Input.GetMouseButtonDown(0));
    //         UpdateInput(InputType.LeftMouseButton, Input.GetMouseButton(0));
    //         UpdateInput(InputType.RightMouseButton, Input.GetMouseButton(1));
    //     }

    //     if (Input.GetMouseButtonUp(1))
    //     {
    //         UpdateInput(InputType.TurnCamera, false);
    //         L2GameUI.Instance.EnableMouse();
    //     }

    //     mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //     UpdateInput(InputType.MouseMoving, mouseAxis.x != 0 || mouseAxis.y != 0);

    //     UpdateInput(InputType.SendMessage, Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));
    //     UpdateInput(InputType.Escape, Input.GetKeyDown(KeyCode.Escape));

    //     if (!ChatWindow.Instance.ChatOpened)
    //     {
    //         inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //         UpdateInput(InputType.Jump, Input.GetKeyDown(KeyCode.Space));
    //         UpdateInput(InputType.Sit, Input.GetKeyDown(KeyCode.E));
    //     }
    //     else
    //     {
    //         inputAxis = Vector2.zero;
    //     }

    //     UpdateInput(InputType.InputAxis, inputAxis.x != 0 || inputAxis.y != 0);
    //     UpdateInput(InputType.Move, IsInputPressed(InputType.InputAxis) || IsInputPressed(InputType.MoveForward));
    //     UpdateInput(InputType.MoveForward, IsInputPressed(InputType.LeftMouseButton) && IsInputPressed(InputType.RightMouseButton));
    //     UpdateInput(InputType.Attack, Input.GetKeyDown(KeyCode.F));
    //     UpdateInput(InputType.DebugAttack, Input.GetKeyDown(KeyCode.C));
    // }

    // public bool IsInputPressed(InputType type)
    // {
    //     return inputsPressed.ContainsKey(type) && inputsPressed[type] != false;
    // }

    // public void UpdateInput(InputType type, bool pressed)
    // {
    //     if (!inputsPressed.ContainsKey(type))
    //     {
    //         inputsPressed.Add(type, pressed);
    //     }
    //     else
    //     {
    //         inputsPressed[type] = pressed;
    //     }
    // }

    void OnDestroy()
    {
        _instance = null;
    }
}
