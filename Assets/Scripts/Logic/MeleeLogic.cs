using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLogic : InterfaceLogicBase
{
    public static MeleeLogic I;

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitMelee(newBase as IMelee);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }


    private void InitMelee(IMelee melee)
    {
        if (melee == null)
            return;

        melee.onMelee = new MeleeEvent();
        melee.onTriggerEnter.AddListener(OnTriggerEnterMelee);
    }

    private void OnTriggerEnterMelee(IBase newBase, Collider collider)
    {
        Debug.Log("melee!");
        if (!BehaviourBase.GetBehaviourOfType<IMortal>(out BehaviourBase behaviourBase, collider.gameObject))
            return;

        MortalityLogic.I.TakeDamage(behaviourBase as IMortal);
    }

}
public interface IMelee : IAnimated
{
    MeleeEvent onMelee { get; set; }
}
public class MeleeEvent : AnimationEvent<IExplosive>
{
    public MeleeEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}