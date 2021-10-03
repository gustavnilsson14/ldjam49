using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLogic : InterfaceLogicBase
{
    public static TorchLogic I;

    public List<ITorch> torches = new List<ITorch>();
    public List<ITorch> torchesLit = new List<ITorch>();

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitTorch(newBase as ITorch);
        InitTorchLighter(newBase as ITorchLighter);
    }

    private void InitTorch(ITorch torch)
    {
        if (torch == null)
            return;
        torch.onTorchLit = new TorchEvent(torch, "Lit", torch.GetLitAudio());
        torch.onTriggerEnter.AddListener(EnteredTorchZone);
        torch.onTriggerExit.AddListener(ExitedTorchZone);
        torches.Add(torch);
    }
    private void InitTorchLighter(ITorchLighter torchLighter)
    {
        if (torchLighter == null)
            return;
        torchLighter.onLightTorch = new TorchLighterEvent(torchLighter, "TorchLight", torchLighter.GetTorchLightAudio());
        torchLighter.onStopLightTorch = new TorchLighterEvent(torchLighter, "StopTorchLight", torchLighter.GetTorchLightAudio());
    }

    protected override void UnRegister(IBase b)
    {
        base.UnRegister(b, new List<IList>() { 
            torches
        });
    }

    private void EnteredTorchZone(IBase arg0, Collider arg1)
    {
        ITorch torch = arg0 as ITorch;
        ITorchLighter torchLighter = arg1.GetComponent<ITorchLighter>();
        if (torch == null || torchLighter == null)
            return;
        torchLighter.onLightTorch.Invoke(torchLighter, torch);
        if (torchesLit.Contains(torch))
            return;
        torch.onTorchLit.Invoke(torch);
        torchesLit.Add(torch);
    }

    private void ExitedTorchZone(IBase arg0, Collider arg1)
    {
        ITorch torch = arg0 as ITorch;
        ITorchLighter torchLighter = arg1.GetComponent<ITorchLighter>();
        if (torch == null || torchLighter == null)
            return;
        torchLighter.onStopLightTorch.Invoke(torchLighter, torch);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }
}
public interface ITorch : IBase
{
    TorchEvent onTorchLit { get; set; }
    AudioSource GetLitAudio();
}
public interface ITorchLighter : IBase
{
    TorchLighterEvent onLightTorch { get; set; }
    TorchLighterEvent onStopLightTorch { get; set; }
    AudioSource GetTorchLightAudio();
}
public class TorchEvent : AnimationEvent<ITorch>
{
    public TorchEvent(IBase b = null, string name = "default", AudioSource audioSource = null) : base(b, name, audioSource)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}
public class TorchLighterEvent : AnimationEvent<ITorchLighter, ITorch>
{
    public TorchLighterEvent(IBase b = null, string name = "default", AudioSource audioSource = null) : base(b, name, audioSource)
    {
    }
    public override bool TryGetParameterType(out AnimatorControllerParameterType parameterType)
    {
        parameterType = AnimatorControllerParameterType.Trigger;
        return true;
    }
}



