using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviourBase, IInputReciever, IMover, IJumper
{
    public List<AxisMapping> axisMappings;
    public List<InputMapping> inputMappings;
    public float jumpForce;
    public float maxSpeed;

    public float slopeLimit;
    public float moveSpeed;
    public bool allowJump;
    public float jumpSpeed;
    public bool isGrounded { get; set; }

    public JumpEvent onJump { get; set; }
    public Animator animator { get; set; }
    public Vector3 movementVector { get; set; }
    public MoveEvent OnMove { get; set; }

    public float GetSlopeLimit() => slopeLimit;
    public float GetMoveSpeed() => moveSpeed;
    public bool GetAllowJump() => allowJump;
    public float GetJumpSpeed() => jumpSpeed;

    public List<AxisMapping> GetAxisMappings() => axisMappings;

    public List<InputMapping> GetInputMappings() => inputMappings;

    public float GetJumpForce() => jumpForce;
}
