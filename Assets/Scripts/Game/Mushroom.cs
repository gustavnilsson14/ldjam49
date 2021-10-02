using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : BehaviourBase, IPlatform, IBouncy
{
    public float bounciness;
    public BouncyEvent onBounce { get; set; }
    public Animator animator { get; set; }
    public PlatformEvent onInteract { get; set; }
    public float GetBounciness() => bounciness;

    private void OnTriggerEnter(Collider other)
    {
        PlatformLogic.I.Bounce((this as IBouncy), other);
    }

}
