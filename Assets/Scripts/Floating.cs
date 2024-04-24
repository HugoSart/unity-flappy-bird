using UnityEngine;

public class Floating : MonoBehaviour {
    
    // Properties
    public bool enabled = true;
    public float amplitude = 0.5f; // Height of the float
    public float frequency = 1f; // Speed of the float

    // Position Storage Variables
    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
    

    // =================================================================================================================
    // Unity lifecycle
    // =================================================================================================================
    void Start() {
        
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        
    }

    // Update is called once per frame
    void Update() {
        if (!enabled) return;
        
        // Float up/down with a Sin()
        var pos = transform.position;
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = new Vector3(pos.x, tempPos.y, pos.z);

    }
    
}