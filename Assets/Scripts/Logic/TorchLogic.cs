using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLogic : InterfaceLogicBase
{
    public static TorchLogic I;

    public Animator torchOverlay;
    public Animator victoryOverlay;

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
        torchLighter.onStopLightTorch = new TorchLighterEvent(torchLighter, "StopTorchLight");
        torchLighter.busyLighting = false;
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
        if (torchesLit.Contains(torch))
            return;
        torch.onTorchLit.Invoke(torch);
        torchLighter.onLightTorch.Invoke(torchLighter, torch);
        torchesLit.Add(torch);
        torchOverlay.Play($"{torchesLit.Count}Torches");
        torchLighter.busyLighting = true;
        if (torchesLit.Count == 3)
        {
            victoryOverlay.Play("VictoryOverlay");
            return;
        }
        StartCoroutine(NotBusyAnymore(torchLighter));
    }

    private IEnumerator NotBusyAnymore(ITorchLighter torchLighter)
    {
        torchLighter.GetGameObject().GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        torchLighter.busyLighting = false;
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

    public bool CheckLighterBusy(IBase ibase) {
        if (!(ibase is ITorchLighter))
            return true;
        return !(ibase as ITorchLighter).busyLighting;
    }
}
public interface ITorch : IBase
{
    TorchEvent onTorchLit { get; set; }
    AudioSource GetLitAudio();
}
public interface ITorchLighter : IBase
{
    bool busyLighting { get; set; }
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



