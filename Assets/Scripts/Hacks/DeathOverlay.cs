using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOverlay : MonoBehaviour
{
    public GameObject character;

    void Start()
    {
        StartCoroutine(DeathListener());
        
    }

    private IEnumerator DeathListener()
    {
        yield return 0;
        yield return 0;
        if (!character.TryGetComponent<IMortal>(out IMortal mortal))
            yield break;
        mortal.onTakeDamage.AddListener(ShowOverlay);
    }

    private void ShowOverlay(IMortal arg0)
    {
        if (!TryGetComponent(out Animator animator))
            return;
        animator.Play("DeathOverlay");
    }
}
