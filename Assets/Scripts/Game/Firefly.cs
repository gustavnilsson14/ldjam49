using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : BehaviourBase, IDollyMover, IMelee
{
    public CinemachineDollyCart dollyCart { get; set; }
    public CinemachineSmoothPath dollyPath { get; set; }
    public Animator animator { get; set; }
    public Vector3 previousForward { get; set; }
    public MeleeEvent onMelee { get; set; }
}
