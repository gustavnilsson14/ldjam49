using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLogic : InterfaceLogicBase
{
    public static JumpLogic I;
    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitJumper(newBase as IJumper);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
        JumperInternalListeners(newBase as IJumper);
    }

    private void InitJumper(IJumper jumper)
    {
        if (jumper == null)
            return;
        jumper.onJump = new JumpEvent(jumper, "Jump");
        jumper.onLand = new JumpEvent(jumper, "Land");
    }
    private void JumperInternalListeners(IJumper jumper)
    {
        if (jumper == null)
            return;

        if (jumper is IMover)
            (jumper as IMover).OnLand.AddListener(Land);
    }

    public void Jump(IJumper jumper) {
        if (!jumper.GetGameObject().TryGetComponent(out Rigidbody rigidbody))
            return;
        if (!(jumper as IMover).isGrounded)
            return;

        rigidbody.AddForce(transform.up * jumper.GetJumpSpeed(), ForceMode.VelocityChange);
        jumper.onJump.Invoke(jumper);
    }

    private void Land(IMover mover)
    {
        IJumper jumper = mover as IJumper;
        if (jumper == null)
            return;
        jumper.onLand.Invoke(jumper);
    }

}
public interface IJumper : IAnimated
{
    JumpEvent onJump { get; set; }
    JumpEvent onLand { get; set; }
    float GetJumpSpeed();
}
public class JumpEvent : AnimationEvent<IJumper>
{
    public JumpEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}