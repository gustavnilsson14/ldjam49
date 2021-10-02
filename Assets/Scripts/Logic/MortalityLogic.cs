using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalityLogic : InterfaceLogicBase
{
    public static MortalityLogic I;
    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitMortality(newBase as IMortality);
    }


    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    private void InitMortality(IMortality mortality)
    {
        if (mortality == null)
            return;

        mortality.onTakeDamage = new MortalityEvent();
    }

    public void TakeDamage(IMortality mortality)
    {
        Debug.Log("take dmg");
        Destroy(mortality.GetGameObject());

    }
}
public interface IMortality : IAnimated
{
    MortalityEvent onTakeDamage { get; set; }
}
public class MortalityEvent : AnimationEvent<IMortality>
{
    public MortalityEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}


