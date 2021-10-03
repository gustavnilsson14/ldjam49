using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalityLogic : InterfaceLogicBase
{
    public static MortalityLogic I;
    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitMortality(newBase as IMortal);
    }


    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    private void InitMortality(IMortal mortal)
    {
        if (mortal == null)
            return;
        mortal.alive = true;
        mortal.onTakeDamage = new MortalityEvent(mortal, "Damage", mortal.GetDamageAudio());
    }

    public void TakeDamage(IMortal mortal)
    {
        if (!mortal.alive)
            return;
        mortal.onTakeDamage.Invoke(mortal);
        Die(mortal);
    }

    private void Die(IMortal mortal)
    {
        mortal.alive = false;
        if (mortal.GetGameObject().TryGetComponent(out Rigidbody rb))
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        //Destroy(mortal.GetGameObject(), mortal.GetDecayTime());
    }


    public bool CheckMortal(IBase ibase)
    {
        if (!(ibase is IMortal))
            return true;
        return (ibase as IMortal).alive;
    }
}
public interface IMortal : IAnimated
{
    bool alive { get; set; }
    float GetDecayTime();
    MortalityEvent onTakeDamage { get; set; }
    AudioSource GetDamageAudio();
}
public class MortalityEvent : AnimationEvent<IMortal>
{
    public MortalityEvent(IBase b = null, string name = "default", AudioSource audioSource = null) : base(b, name, audioSource)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}


