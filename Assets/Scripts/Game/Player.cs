using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviourBase, IInputReciever, IMover, IJumper
{
    public float acceleration;
    public List<AxisMapping> axisMappings;
    public List<InputMapping> inputMappings;
    public float jumpForce;
    public float maxSpeed;

    public JumpEvent onJump { get; set; }
    public Animator animator { get; set; }
    public Vector3 movementVector { get; set; }
    public MoveEvent OnMove { get; set; }

    public float GetAcceleration() => acceleration;

    public List<AxisMapping> GetAxisMappings() => axisMappings;

    public List<InputMapping> GetInputMappings() => inputMappings;

    public float GetJumpForce() => jumpForce;

    public float GetMaxSpeed() => maxSpeed;
}
