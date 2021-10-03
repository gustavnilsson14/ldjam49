using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLogic : InterfaceLogicBase
{
    public static MagicLogic I;

    public GameObject smokeBombPrefab;
    public GameObject platformPrefab;
    public float platformDownwardsSpawnDistance;

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitMagicCaster(newBase as IMagicCaster);
        InitSmokeBombCaster(newBase as ISmokeBombCaster);
        InitPlatformCaster(newBase as IPlatformCaster);

    }



    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }
    private void InitMagicCaster(IMagicCaster magicCaster)
    {
        if (magicCaster == null)
            return;

        magicCaster.onCastMagic = new MagicCastEvent(magicCaster,"Cast", magicCaster.GetMagicAudio());
    }

    private void InitSmokeBombCaster(ISmokeBombCaster smokeBombCaster)
    {
        if (smokeBombCaster == null)
            return;
        smokeBombCaster.onCastSmokeBomb = new SmokeBombCastEvent();
    }
    private void InitPlatformCaster(IPlatformCaster platformCaster)
    {
        if (platformCaster == null)
            return;
        platformCaster.onCastPlatform = new PlatformCastEvent();
    }
    public void CastMagic(IMagicCaster magicCaster)
    {
        if (!TorchLogic.I.CheckLighterBusy(magicCaster))
            return;
        if (Time.time < magicCaster.nextCast)
            return;
        magicCaster.nextCast = Time.time + magicCaster.GetCastCooldown();
        magicCaster.onCastMagic.Invoke(magicCaster);
        if (!(magicCaster is IMover))
            return;
        if (!(magicCaster as IMover).GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;

        if ((magicCaster as IMover).isGrounded && (magicCaster is ISmokeBombCaster))
            CastSmokeBomb(magicCaster as ISmokeBombCaster);
        if (!(magicCaster as IMover).isGrounded && (magicCaster is IPlatformCaster))
            CastPlatform(magicCaster as IPlatformCaster);
    }

    public void CastSmokeBomb(ISmokeBombCaster smokeBombCaster)
    {
        if (!(smokeBombCaster is IMover))
            return;
        if (!(smokeBombCaster as IMover).GetGameObject().TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            return;
        PrefabFactory.I.Create(smokeBombPrefab, null, smokeBombCaster.GetGameObject().transform.position + rigidbody.velocity.normalized);
    }

    public void CastPlatform(IPlatformCaster platformCaster)
    {
        PrefabFactory.I.Create(platformPrefab, null, platformCaster.GetGameObject().transform.position + (Vector3.down * platformDownwardsSpawnDistance));
    }
}
public interface IMagicCaster : IAnimated
{
    MagicCastEvent onCastMagic { get; set; }
    AudioSource GetMagicAudio();
    float GetCastCooldown();
    float nextCast { get; set; }
}

public class MagicCastEvent : AnimationEvent<IMagicCaster>
{
    public MagicCastEvent(IBase b = null, string name = "default", AudioSource audioSource = null) : base(b, name, audioSource)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
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
    PlatformCastEvent onCastPlatform { get; set; }
}

public class PlatformCastEvent : AnimationEvent<IPlatformCaster>
{
    public PlatformCastEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}
