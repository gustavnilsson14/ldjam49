﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BehaviourBase : MonoBehaviour, IBase, IPointerClickHandler
{
    public DestroyEvent onDestroy { get; set; }
    public CollisionEvent onCollision { get; set; }
    public TriggerEvent onTriggerEnter { get; set; }
    public TriggerEvent onTriggerExit { get; set; }
    public ClickEvent onClick { get; set; }
    public int uniqueId { get; set; }
    public GameObject GetGameObject() => gameObject;
    private void OnCollisionEnter(Collision collision)
    {
        if (onCollision == null)
            return;
        onCollision.Invoke(this, collision);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null)
            return;
        onTriggerEnter.Invoke(this, collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider == null)
            return;
        onTriggerExit.Invoke(this, collider);
    }
    private void OnDestroy()
    {
        if (onDestroy == null)
            return;
        onDestroy.Invoke(this);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke(this);
    }
}
