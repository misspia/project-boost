using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    // todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; // 0 0 not moved, 1 fully moved

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position; // gets object's transform
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
