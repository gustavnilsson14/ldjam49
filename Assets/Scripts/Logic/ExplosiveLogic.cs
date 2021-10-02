using System.Collections;
using System.Collections.Generic;
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

        Debug.Log("exploded");

        foreach (Collider collider in hitColliders)
        {
            if (!collider.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                continue;

            Debug.Log("exploded");
            rigidbody.AddExplosionForce(explosive.GetPower(), explosive.GetGameObject().transform.position, explosive.GetRadius(), 1.0f);
            Debug.Log(collider.gameObject.name);
        }
        explosive.exploded = true;
    }

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