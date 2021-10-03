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

        mover.OnStartMove = new MoveEvent(mover, "StartMove");
        mover.OnStopMove = new MoveEvent(mover, "StopMove");
        mover.OnLand = new MoveEvent();
        movers.Add(mover);
    }

    public void FixedUpdate()
    {
        movers.ForEach(x => CheckGrounded(x));
        movers.ForEach(x => Move(x));
        movers.ForEach(x => HandleDirection(x));
        movers.ForEach(x => MultiplyFallSpeed(x));
    }

    private void HandleDirection(IMover mover)
    {
        if (mover.movementVector.x > 0)
            mover.animator.SetInteger("Direction", 1);
        if (mover.movementVector.x < 0)
            mover.animator.SetInteger("Direction", -1);
    }

    private void MultiplyFallSpeed(IMover mover)
    {
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;
        if (rigidbody.velocity.y > 0)
            return;
        if (mover.isGrounded)
        {
            mover.gravityMultiplier = 0;
            return;
        }

        mover.gravityMultiplier += 1.5f;
        rigidbody.AddForce(Vector3.down * mover.gravityMultiplier);
    }
    

    private void Move(IMover mover)
    {
        if (!MortalityLogic.I.CheckMortal(mover))
            return;
        if (!TorchLogic.I.CheckLighterBusy(mover))
            return;
        if (!mover.GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;
        if (!mover.isGrounded)
        {
            if (rigidbody.velocity.x < 6 && rigidbody.velocity.x > -6)
                rigidbody.AddForce(mover.movementVector * mover.GetMoveSpeedAir());
            if(mover.movementVector.x < 0 && rigidbody.velocity.x >= 6)
                rigidbody.AddForce(mover.movementVector * mover.GetMoveSpeedAir());
            if (mover.movementVector.x > 0 && rigidbody.velocity.x <= -6)
                rigidbody.AddForce(mover.movementVector * mover.GetMoveSpeedAir());
        }

        Vector3 currentMovement = mover.movementVector * mover.GetMoveSpeed() * Time.fixedDeltaTime;
        HandleStartStop(mover, currentMovement);

        if (mover.isGrounded)
        {
            if (rigidbody.velocity.x < 6 && rigidbody.velocity.x > -6)
                rigidbody.AddForce(mover.movementVector * mover.GetMoveSpeed());
        }

        mover.previousHorizontalVelocity = mover.movementVector;
    }

    private void HandleStartStop(IMover mover, Vector3 currentMovement)
    {
        if (mover.previousHorizontalVelocity == Vector3.zero && currentMovement != Vector3.zero)
            mover.OnStartMove.Invoke(mover);
        if (mover.previousHorizontalVelocity != Vector3.zero && currentMovement == Vector3.zero)
            mover.OnStopMove.Invoke(mover);
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
                {
                    mover.isGrounded = true;
                }
            }
        }
        if (mover.animator.GetBool("IsGrounded") == mover.isGrounded)
            return;
        mover.animator.SetBool("IsGrounded", mover.isGrounded);
        mover.animator.ResetTrigger("MoveEvent_StopMove");
        mover.animator.ResetTrigger("MoveEvent_StartMove");
        if (!mover.isGrounded)
            return;
        mover.OnLand.Invoke(mover);
        if (mover.previousHorizontalVelocity != Vector3.zero)
            mover.animator.SetTrigger("MoveEvent_StartMove");
    }
}

public interface IMover : IAnimated
{
    float GetSlopeLimit();
    float GetMoveSpeed();
    float GetMoveSpeedAir();
    bool isGrounded { get; set; }

    Vector3 previousHorizontalVelocity { get; set; }
    Vector3 movementVector { get; set; }
    MoveEvent OnStartMove { get; set; }
    MoveEvent OnStopMove { get; set; }
    MoveEvent OnLand { get; set; }
    float gravityMultiplier { get; set; }
    Vector3 direction { get; set; }

}

public class MoveEvent : AnimationEvent<IMover>
{
    public MoveEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}
