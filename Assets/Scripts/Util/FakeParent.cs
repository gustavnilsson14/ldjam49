using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParent : MonoBehaviour
{
    public Transform fakeParent;
    void FixedUpdate()
    {
        transform.position = fakeParent.position;
    }
}
