using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class HoldRandom : ActionNode
{

    public float range = 1.0f;
    
    GameObject obj = null;
    protected override void OnStart() {
        List<GameObject> gos = new List<GameObject>();
        foreach (string tag in new string[] {"Enemy", "Item"}) {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag)) {
                if (Vector3.Distance(go.transform.position, context.transform.position) <= range) {
                    gos.Add(go);
                }
            }
        }

        if (gos.Count == 0) {
            return;
        }

        // pick random
        GameObject targetObj = gos[Random.Range(0, gos.Count)];
        Sprite targetSprite;
        if (targetObj.GetComponent<SpriteRenderer>() != null) {
            targetSprite = targetObj.GetComponent<SpriteRenderer>().sprite;
        } else {
            // find child with tag "sprite"
            targetSprite = targetObj.transform.Find("sprite").GetComponent<SpriteRenderer>().sprite;
        }
        targetObj.SetActive(false);
        context.holding = targetObj;

        // create new object
        obj = new GameObject();
        obj.transform.parent = context.transform;
        obj.transform.localPosition = new Vector3(0, 0.3f, 0);

        // add sprite
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = targetSprite;
        sr.sortingLayerName = "Shadow";
        obj.AddComponent<AutoSortOrder>();
    }

    protected override void OnStop() {
        
    }

    protected override State OnUpdate() {
        if (obj == null) return State.Failure;
        return State.Success;
    }
}
