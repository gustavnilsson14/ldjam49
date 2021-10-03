using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brambles : BehaviourBase, IMelee
{
    public MeleeEvent onMelee { get; set; }
    public Animator animator { get; set; }
}
