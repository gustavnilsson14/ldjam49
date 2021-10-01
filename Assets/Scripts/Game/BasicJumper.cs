using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicJumper : BehaviourBase, IJumper, IInputReciever
{
    public float jumpForce;
    public JumpEvent onJump { get; set; }
    public Animator animator { get; set; }
    public float GetJumpForce() => jumpForce;
    public List<InputMapping> inputMappings;
    public List<AxisMapping> axisMappings;
    public List<InputMapping> GetInputMappings() => inputMappings;

    public List<AxisMapping> GetAxisMappings() => axisMappings;
}
