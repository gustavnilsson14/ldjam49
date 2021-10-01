using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMover : BehaviourBase, IMover, IInputReciever
{
    public float speed;
    public MoveEvent OnMove { get; set; }
    public Animator animator { get; set; }
    public Vector3 movementVector { get; set; }

    public List<InputMapping> inputMappings;
    public List<AxisMapping> axisMappings;
    public List<InputMapping> GetInputMappings() => inputMappings;
    public List<AxisMapping> GetAxisMappings() => axisMappings;

    public float GetSpeed() => speed;
}
