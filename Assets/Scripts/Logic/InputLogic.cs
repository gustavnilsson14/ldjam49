using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public enum InputType
{
    JUMP,
    SPELLCAST
}
public enum AxisType
{
    MOVE_X,
    MOVE_Y
}
public enum InputHandlingType
{
    GET_KEY_DOWN,
    GET_KEY_UP,
    GET_KEY,
}
public enum MouseHandlingType
{
    GET_MOUSE_BUTTON_DOWN,
    GET_MOUSE_BUTTON_UP,
    GET_MOUSE_BUTTON
}

public class InputLogic : InterfaceLogicBase
{
    public static InputLogic I;
    public List<IInputReciever> inputRecievers = new List<IInputReciever>();
    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitInputReciever(newBase as IInputReciever);
    }

    private void InitInputReciever(IInputReciever inputReciever)
    {
        if (inputReciever == null)
            return;
        inputRecievers.Add(inputReciever);
    }
    protected override void UnRegister(IBase b)
    {
        base.UnRegister(b);
        if ((b is IInputReciever))
            inputRecievers.Remove(b as IInputReciever);
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        foreach (IInputReciever inputReciever in inputRecievers)
        {
            HandleJumperInput(inputReciever);
            HandleMoverInput(inputReciever);
            HandleCastSpellInput(inputReciever);
        }
    }

    private void HandleCastSpellInput(IInputReciever inputReciever)
    {
        if (!(inputReciever is IMagicCaster))
            return;
        foreach (InputMapping inputMapping in inputReciever.GetInputMappings().FindAll(x => x.inputType == InputType.SPELLCAST))
        {
            if (!GetInput(inputMapping))
                continue;
            MagicLogic.I.CastMagic(inputReciever as IMagicCaster);
        }
    }

    private void HandleJumperInput(IInputReciever inputReciever)
    {
        if (!(inputReciever is IJumper))
            return;
        foreach (InputMapping inputMapping in inputReciever.GetInputMappings().FindAll(x => x.inputType == InputType.JUMP))
        {
            if (!GetInput(inputMapping))
                continue;
            JumpLogic.I.Jump(inputReciever as IJumper);
        }

    }

    private void HandleMoverInput(IInputReciever inputReciever)
    {
        if (!(inputReciever is IMover))
            return;

        Vector3 movementVector = Vector3.zero;
        foreach (AxisMapping axisMapping in inputReciever.GetAxisMappings().FindAll(x => x.axisType == AxisType.MOVE_X))
        {
            movementVector += new Vector3(Input.GetAxis(axisMapping.axisName),0,0);
        }

        (inputReciever as IMover).movementVector = movementVector;
    }

    private bool GetInput(InputMapping inputMapping)
    {
        if (inputMapping.isMouse)
            return GetMouseInput(inputMapping);
        return GetKeyInput(inputMapping);
    }

    private bool GetMouseInput(InputMapping inputMapping)
    {
        switch (inputMapping.mouseHandlingType)
        {
            case MouseHandlingType.GET_MOUSE_BUTTON_DOWN:
                return Input.GetMouseButtonDown(inputMapping.mouseButton);
            case MouseHandlingType.GET_MOUSE_BUTTON_UP:
                return Input.GetMouseButtonUp(inputMapping.mouseButton);
            case MouseHandlingType.GET_MOUSE_BUTTON:
                return Input.GetMouseButton(inputMapping.mouseButton);
        }
        return false;
    }

    private bool GetKeyInput(InputMapping inputMapping)
    {
        switch (inputMapping.inputHandlingType)
        {
            case InputHandlingType.GET_KEY_DOWN:
                return Input.GetKeyDown(inputMapping.keyCode);
            case InputHandlingType.GET_KEY_UP:
                return Input.GetKeyUp(inputMapping.keyCode);
            case InputHandlingType.GET_KEY:
                return Input.GetKey(inputMapping.keyCode);
        }
        return false;
    }
}
[System.Serializable]
public class InputMapping
{
    public bool isMouse = false;
    public KeyCode keyCode;
    public int mouseButton;
    public InputType inputType;
    public InputHandlingType inputHandlingType;
    public MouseHandlingType mouseHandlingType;
}
[System.Serializable]
public class AxisMapping
{
    public string axisName;
    public AxisType axisType;
}
[System.Serializable]
public class MouseMapping
{
    public KeyCode keyCode;
    public InputType inputType;
    public InputHandlingType inputHandlingType;
}
public interface IInputReciever : IBase
{
    List<InputMapping> GetInputMappings();
    List<AxisMapping> GetAxisMappings();
}
public class InputEvent : UnityEvent<KeyCode> { }