using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : InterfaceLogicBase
{
    public static PlatformLogic I;
    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitPlatform(newBase as IPlatform);
        InitBouncy(newBase as IBouncy);
    }


    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    private void InitPlatform(IPlatform platform)
    {
        if (platform == null)
            return;

        platform.onInteract = new PlatformEvent();
    }

    private void InitBouncy(IBouncy bouncy)
    {
        if (bouncy == null)
            return;

        bouncy.onBounce = new BouncyEvent();
    }

    public void Bounce(IBouncy bouncy, Collider other)
    {

        if (!other.gameObject.TryGetComponent(out Rigidbody rigidbody))
            return;

        Debug.Log("triggered!");
        rigidbody.AddForce(Vector3.up * bouncy.GetBounciness(), ForceMode.VelocityChange);
    }
}
public interface IPlatform : IAnimated
{
    PlatformEvent onInteract { get; set; }
}
public class PlatformEvent : AnimationEvent<IPlatform>
{
    public PlatformEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}

public interface IBouncy : IAnimated
{
    float GetBounciness();
    BouncyEvent onBounce { get; set; }
}

public class BouncyEvent : AnimationEvent<IBouncy>
{
    public BouncyEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}

