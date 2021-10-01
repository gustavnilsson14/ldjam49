using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : InterfaceLogicBase
{
    public static MoveLogic I;
    public List<IMover> movers = new List<IMover>();
    public LayerMask groundLayer;

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitMover(newBase as IMover);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    protected override void UnRegister(IBase b)
    {
        base.UnRegister(b, new List<IList>() { 
            movers
        });
    }

    private void InitMover(IMover mover)
    {
        if (mover == null)
            return;

        mover.OnMove = new MoveEvent();
        movers.Add(mover);
    }

    public void Update()
    {
        movers.ForEach(x => Move(x));
        movers.ForEach(x => HandleFallSpeed(x));
    }

    private void HandleFallSpeed(IMover mover)
    {
        if (!mover.GetGameObject().TryGetComponent(out Rigidbody rb))
            return;
        if (rb.velocity.y > 0)
            return;
        rb.velocity += new Vector3(0, rb.velocity.y, 0) * Time.deltaTime * 5;
    }

    private void Move(IMover mover)
    {
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rb))
            return;
        if (!IsGrounded(mover))
            return;
        rb.velocity = GetVelocity(mover, rb);
    }

    private Vector3 GetVelocity(IMover mover, Rigidbody rb)
    {
        if (mover.movementVector == Vector3.zero)
            return new Vector3(0,rb.velocity.y,0);
        return rb.velocity + (mover.movementVector * mover.GetAcceleration() * Time.deltaTime);
    }

    public bool IsGrounded(IMover mover) {
        Transform transform = mover.GetGameObject().transform;
        Vector3 overlapCenter = transform.position + Vector3.down;
        if (Physics.OverlapBox(overlapCenter, transform.localScale / 2, transform.rotation, groundLayer).Length == 0)
            return false;
        return true;
    }
}

public interface IMover : IAnimated
{
    float GetMaxSpeed();
    float GetAcceleration();
    Vector3 movementVector { get; set; }
    MoveEvent OnMove { get; set; }
}

public class MoveEvent : AnimationEvent<IMover>
{
    public MoveEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
