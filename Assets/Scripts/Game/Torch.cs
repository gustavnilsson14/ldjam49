using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : BehaviourBase,
    ITorch
{
    public AudioSource litSound;

    public TorchEvent onTorchLit { get; set; }

    public AudioSource GetLitAudio() => litSound;
}
