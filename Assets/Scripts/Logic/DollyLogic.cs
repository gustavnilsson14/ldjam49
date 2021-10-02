using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class DollyLogic : InterfaceLogicBase
{
    public static DollyLogic I;
    public List<IDollyMover> dollyMovers = new List<IDollyMover>();

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitDollyMover(newBase as IDollyMover);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    protected override void UnRegister(IBase b)
    {
        base.UnRegister(b, new List<IList>() {
            dollyMovers
        });
    }

    private void InitDollyMover(IDollyMover dollyMover)
    {
        if (dollyMover == null)
            return;
        dollyMovers.Add(dollyMover);
        dollyMover.dollyPath = dollyMover.GetGameObject().GetComponentInChildren<CinemachineSmoothPath>();
        dollyMover.dollyCart = dollyMover.GetGameObject().GetComponentInChildren<CinemachineDollyCart>();
        LiftOutDollyTrack(dollyMover);
    }

    private void LiftOutDollyTrack(IDollyMover dollyMover)
    {
        dollyMover.dollyCart.transform.parent = dollyMover.GetGameObject().transform.parent;
        dollyMover.dollyPath.transform.parent = dollyMover.GetGameObject().transform.parent;
    }

    private void Update()
    {
        dollyMovers.ForEach(x => HandleDirection(x));
    }

    private void HandleDirection(IDollyMover dollyMover)
    {
        if (dollyMover.dollyCart.transform.forward.x > 0)
            dollyMover.animator.SetInteger("Direction", 1);
        if (dollyMover.dollyCart.transform.forward.x < 0)
            dollyMover.animator.SetInteger("Direction", -1);
    }
}
public interface IDollyMover : IAnimated
{
    Vector3 previousForward { get; set; }
    CinemachineDollyCart dollyCart { get; set; }
    CinemachineSmoothPath dollyPath { get; set; }
}