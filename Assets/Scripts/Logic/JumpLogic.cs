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
    }

    private void InitJumper(IJumper jumper)
    {
        if (jumper == null)
            return;

        jumper.onJump = new JumpEvent();
    }

    public void Jump(IJumper jumper) {
        if (!jumper.GetGameObject().TryGetComponent(out Rigidbody rigidbody))
            return;
        rigidbody.velocity += Vector3.up * jumper.GetJumpForce();
    }

}
public interface IJumper : IAnimated
{
    float GetJumpForce();
    JumpEvent onJump { get; set; }
}
public class JumpEvent : AnimationEvent<IJumper>
{
    public JumpEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}