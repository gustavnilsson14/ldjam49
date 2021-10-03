using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosiveLogic : InterfaceLogicBase
{
    public static ExplosiveLogic I;
    public List<IExplosive> explosives = new List<IExplosive>();

    protected override void OnInstantiate(GameObject newInstance, IBase newBase)
    {
        base.OnInstantiate(newInstance, newBase);
        InitExplosive(newBase as IExplosive);
    }

    protected override void OnRegisterInternalListeners(GameObject newInstance, IBase newBase)
    {
        base.OnRegisterInternalListeners(newInstance, newBase);
    }

    protected override void UnRegister(IBase b)
    {
        base.UnRegister(b, new List<IList>() {
            explosives
        });
    }

    private void InitExplosive(IExplosive explosive)
    {
        if (explosive == null)
            return;
        explosive.exploded = false;
        explosive.explodeTime = Time.unscaledTime + explosive.GetTime();
        explosive.onExplode = new ExplodeEvent();
        explosives.Add(explosive);
    }

    private void Update()
    {
        explosives.ForEach(x => Explode(x));
    }


    public void Explode(IExplosive explosive)
    {
        if (explosive.explodeTime > Time.unscaledTime || explosive.exploded)
            return;

        Collider[] hitColliders = Physics.OverlapSphere(explosive.GetGameObject().transform.position, explosive.GetRadius());

        foreach (Collider collider in hitColliders)
        {
            if(BehaviourBase.GetBehaviourOfType<IExplodable>(out BehaviourBase behaviour, collider.gameObject))
                DestoryOnExplode(behaviour as IExplodable);
            
            if (!collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                continue;
            rigidbody.AddExplosionForce(explosive.GetPower(), explosive.GetGameObject().transform.position, explosive.GetRadius(), 1.0f);
        }
        explosive.exploded = true;
        Destroy(explosive.GetGameObject());
    }

    public void DestoryOnExplode(IExplodable explodable)
    {
        Destroy(explodable.GetGameObject());
    }

}
public interface IExplodable : IAnimated
{

}
public interface IExplosive : IAnimated
{
    float GetPower();
    float GetTime();
    float GetRadius();
    float explodeTime { get; set; }
    bool exploded { get; set; }

    ExplodeEvent onExplode { get; set; }
}
public class ExplodeEvent : AnimationEvent<IExplosive>
{
    public ExplodeEvent(IBase b = null, string name = "default") : base(b, name)
    {
    }
}