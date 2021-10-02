using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : BehaviourBase, IExplosive
{
    public float power;
    public float time;
    public float radius;

    public ExplodeEvent onExplode { get ; set ; }
    public Animator animator { get; set; }
    public float explodeTime { get; set; }
    public bool exploded { get; set; }

    public float GetPower() => power;
    public float GetTime() => time;

    public float GetRadius() => radius;

}
