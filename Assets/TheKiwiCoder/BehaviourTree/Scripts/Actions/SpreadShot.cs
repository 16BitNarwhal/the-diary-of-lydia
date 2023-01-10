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
    public float coneAngle = 360;
    public bool randomAngle = true;

    private int counter;
    private float angle;

    Vector2 target;
    protected override void OnStart() {
        counter = 0;
        angle = coneAngle/amount;
        
        context.gameObject.GetComponent<Enemy>().StartCoroutine(Shoot());

        if (projectilePrefabs.Count == 0) {
            Debug.LogError("No projectile prefab set");
        }

        target = Player.instance.transform.position - context.transform.position;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }

    IEnumerator Shoot() {
        float angle = coneAngle / amount;
        float angleOffset = Random.Range(0, 360);
        if (!randomAngle) {
            angleOffset = Vector2.SignedAngle(Vector2.right, target);
        }
        angleOffset -= coneAngle / 2;
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
