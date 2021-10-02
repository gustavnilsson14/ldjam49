using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnDeath : MonoBehaviour
{
    public GameObject effectPrefab;

    private void OnDestroy()
    {
        Instantiate(effectPrefab,transform.position,transform.rotation);
    }
}
