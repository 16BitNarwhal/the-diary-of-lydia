using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SpreadShot : ActionNode
{

    public GameObject projectilePrefab;
    public int amount = 4;
    public int repeat = 1;
    public float shotDelay = 0.1f;
    public float projectileSpeed = 3f;
    
    private float lastShot;
    private int counter;
    private float angle;

    protected override void OnStart() {
        lastShot = 0;
        counter = 0;
        angle = 360f/amount;
        
        context.gameObject.GetComponent<Enemy>().StartCoroutine(Shoot());
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }

    IEnumerator Shoot() {
        float angle = 360f / amount;
        for (int i = 0; i < amount; i++) {
            GameObject projectile = Object.Instantiate(projectilePrefab, context.transform.position, context.transform.rotation);
            // rotation projectile variable set to angle * i
            
            Quaternion rotation = Quaternion.AngleAxis(angle * i, Vector3.forward);
            Vector3 direction = rotation * Vector3.right;

            Projectile p = projectile.GetComponent<Projectile>();
            p.SetVelocity(direction * projectileSpeed);
        }
        counter++;
        yield return new WaitForSeconds(shotDelay);

        if (counter >= repeat) yield break;
        context.script.StartCoroutine(Shoot());
    }
}
