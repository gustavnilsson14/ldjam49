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

    public void FixedUpdate()
    {
        movers.ForEach(x => CheckGrounded(x));
        movers.ForEach(x => Move(x));
        movers.ForEach(x => MultiplyFallSpeed(x));
    }

    private void MultiplyFallSpeed(IMover mover)
    {
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;
        if (rigidbody.velocity.y > 0)
            return;
        rigidbody.velocity += new Vector3(0, rigidbody.velocity.y, 0) * Time.deltaTime * 5;
    }

    private void Move(IMover mover)
    {
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;

        Vector3 move = mover.movementVector * mover.GetMoveSpeed() * Time.fixedDeltaTime;
        rigidbody.MovePosition(mover.GetGameObject().transform.position + move);
    }

    public void CheckGrounded(IMover mover) {
        mover.isGrounded = false;
        if (!mover.GetGameObject().TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
            return;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = mover.GetGameObject().transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = mover.GetGameObject().transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + mover.GetGameObject().transform.up * .01f, -mover.GetGameObject().transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, mover.GetGameObject().transform.up);
            if (normalAngle < mover.GetSlopeLimit())
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    mover.isGrounded = true;
            }
        }
    }
}

public interface IMover : IAnimated
{
    float GetSlopeLimit();
    float GetMoveSpeed();
    bool GetAllowJump();
    bool isGrounded { get; set; }
    float GetJumpSpeed();

    Vector3 movementVector { get; set; }
    MoveEvent OnMove { get; set; }
}

public class MoveEvent : AnimationEvent<IMover>
{
    public MoveEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
