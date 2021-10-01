using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : InterfaceLogicBase
{
    public static MoveLogic I;
    public List<IMover> movers = new List<IMover>();

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
    }

    private void Move(IMover mover)
    {
        Debug.Log("yay");
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rb))
            return;
        Debug.Log(mover.GetSpeed());

        rb.MovePosition(rb.transform.position + (mover.movementVector * mover.GetSpeed() * Time.deltaTime)); 
    }
}

public interface IMover : IAnimated
{
    float GetSpeed();
    Vector3 movementVector { get; set; }
    MoveEvent OnMove { get; set; }
}

public class MoveEvent : AnimationEvent<IMover>
{
    public MoveEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
