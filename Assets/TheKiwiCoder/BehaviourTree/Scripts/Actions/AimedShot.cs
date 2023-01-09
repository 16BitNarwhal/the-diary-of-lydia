using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AimedShot : ActionNode
{

    public List<GameObject> projectilePrefabs;
    public int repeat = 1;
    public float shotDelay = 0.1f;
    public float projectileSpeed = 5f;

    protected override void OnStart() {
        context.script.StartCoroutine(Shoot());
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }

    IEnumerator Shoot() {
        Debug.Log("zombie shoot");
        
        Vector2 direction = Player.instance.transform.position - context.transform.position;

        for (int i = 0; i < repeat; i++) {
            GameObject projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
            if (projectilePrefab == null) continue;

            GameObject projectile = Object.Instantiate(projectilePrefab, context.transform.position, context.transform.rotation);
            
            Projectile p = projectile.GetComponent<Projectile>();
            p.SetVelocity(direction * projectileSpeed);
            

            yield return new WaitForSeconds(shotDelay);
        }
    
    }

}
