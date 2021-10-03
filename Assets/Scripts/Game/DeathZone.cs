using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : BehaviourBase, IMelee
{
    public MeleeEvent onMelee { get; set; }
    public Animator animator { get; set; }
}
