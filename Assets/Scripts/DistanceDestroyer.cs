using System;
using UnityEngine;

public class DistanceDestroyer : MonoBehaviour {

    [SerializeField]
    public Transform reference;

    [SerializeField]
    public float limit = 2f;

    private void Update() {
        if (reference.transform.position.x < transform.position.x) return;
        var dist = Math.Abs(transform.position.x - reference.transform.position.x);
        if (dist > limit) {
            Destroy(gameObject);
        }
    }
    
}
