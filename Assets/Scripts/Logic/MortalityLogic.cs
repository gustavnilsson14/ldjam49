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

        mortal.onTakeDamage = new MortalityEvent(mortal, "Damage", mortal.GetDamageAudio());
    }

    public void TakeDamage(IMortal mortal)
    {
        Destroy(mortal.GetGameObject());
        mortal.onTakeDamage.Invoke(mortal);
    }
}
public interface IMortal : IAnimated
{
    MortalityEvent onTakeDamage { get; set; }
    AudioSource GetDamageAudio();
}
public class MortalityEvent : AnimationEvent<IMortal>
{
    public MortalityEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}


