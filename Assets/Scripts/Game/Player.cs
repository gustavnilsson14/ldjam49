using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviourBase, 
    IInputReciever, 
    IMover,
    IJumper, 
    ISmokeBombCaster, 
    IPlatformCaster, 
    IMagicCaster,
    IMortality
{
    public List<AxisMapping> axisMappings;
    public List<InputMapping> inputMappings;

    public float slopeLimit;
    public float moveSpeed;
    public bool allowJump;
    public float jumpSpeed;
    public float moveSpeedAir;
    public AudioSource jumpSound;

    public bool isGrounded { get; set; }

    public JumpEvent onJump { get; set; }
    public Animator animator { get; set; }
    public Vector3 movementVector { get; set; }
    public float gravityMultiplier { get; set; }
    public Vector3 direction { get; set; }
    public MoveEvent OnMove { get; set; }
    public Vector3 previousHorizontalVelocity { get; set; }
    public MoveEvent OnStartMove { get; set; }
    public MoveEvent OnStopMove { get; set; }
    public JumpEvent onLand { get; set; }
    public MoveEvent OnLand { get; set; }
    public SmokeBombCastEvent onCastSmokeBomb { get; set; }
    public PlatformCastEvent onCastPlatform { get; set; }
    public MagicCastEvent onCastMagic { get; set; }
    public MortalityEvent onTakeDamage { get; set; }

    public float GetSlopeLimit() => slopeLimit;
    public float GetMoveSpeed() => moveSpeed;
    public float GetMoveSpeedAir() => moveSpeedAir;
    public bool GetAllowJump() => allowJump;
    public float GetJumpSpeed() => jumpSpeed;

    public List<AxisMapping> GetAxisMappings() => axisMappings;
    public List<InputMapping> GetInputMappings() => inputMappings;

    public AudioSource GetJumpSound() => jumpSound;

}
