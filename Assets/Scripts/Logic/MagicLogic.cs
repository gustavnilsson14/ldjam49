using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLogic : InterfaceLogicBase
{
    public static MagicLogic I;

    public GameObject smokeBombPrefab;

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitSmokeBombCaster(newBase as ISmokeBombCaster);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }


    private void InitSmokeBombCaster(ISmokeBombCaster smokeBombCaster)
    {
        if (smokeBombCaster == null)
            return;
        smokeBombCaster.onCastSmokeBomb = new SmokeBombCastEvent();

    }

    public void CastSmokeBomb(ISmokeBombCaster smokeBombCaster)
    {
        PrefabFactory.I.Create(smokeBombPrefab, null, smokeBombCaster.GetGameObject().transform);
    }
}
 
public interface ISmokeBombCaster : IAnimated
{
    SmokeBombCastEvent onCastSmokeBomb { get; set; }
}

public class SmokeBombCastEvent : AnimationEvent<ISmokeBombCaster>
{
    public SmokeBombCastEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}

public interface IPlatformCaster : IAnimated
{

}
