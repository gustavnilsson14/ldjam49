using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOverlay : MonoBehaviour
{
    public GameObject character;

    void Start()
    {
        StartCoroutine(DeathListener());
    }

    private IEnumerator DeathListener()
    {
        yield return new WaitForSeconds(1);
        if (!character.TryGetComponent<IMortal>(out IMortal mortal))
            yield break;
        
        mortal.onTakeDamage.AddListener(ShowOverlay);
    }

    private void ShowOverlay(IMortal arg0)
    {
        if (!TryGetComponent(out Animator animator))
            return;
        animator.Play("DeathOverlay");
        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
