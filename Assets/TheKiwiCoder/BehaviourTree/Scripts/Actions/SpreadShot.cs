using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SpreadShot : ActionNode
{

    public List<GameObject> projectilePrefabs;
    public int amount = 4;
    public int repeat = 1;
    public float shotDelay = 0.1f;
    public float projectileSpeed = 3f;
    public bool randomAngle = true;

    private int counter;
    private float angle;

    protected override void OnStart() {
        counter = 0;
        angle = 360f/amount;
        
        context.gameObject.GetComponent<Enemy>().StartCoroutine(Shoot());

        if (projectilePrefabs.Count == 0) {
            Debug.LogError("No projectile prefab set");
        }
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }

    IEnumerator Shoot() {
        float angle = 360f / amount;
        float angleOffset = randomAngle ? Random.Range(0, 360) : 0;
        for (int i = 0; i < amount; i++) {
            GameObject projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
            if (projectilePrefab == null) continue;

            GameObject projectile = Object.Instantiate(projectilePrefab, context.transform.position, context.transform.rotation);
            
            Quaternion rotation = Quaternion.AngleAxis(angle * i + angleOffset, Vector3.forward);
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
