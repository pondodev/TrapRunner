﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
    [SerializeField] GameObject pointer;
    [SerializeField] bool debugAIDestination;
    [SerializeField] float distCheckPadding = 2.0f;
    GameObject pointerRef;
    Transform target;
    NavMeshAgent aiAgent;

    // Use this for initialization
    void Start()
    {
        // Initialise starting destination
        pointerRef = GameObject.Instantiate(pointer, UpdateMoveTo().position, Quaternion.Euler(Vector3.zero)) as GameObject;
        target = pointer.transform;
        aiAgent = GetComponent<NavMeshAgent>();

        // Check if we are displaying the debug sphere for AI movement
        if (debugAIDestination)
            pointerRef.GetComponent<MeshRenderer>().enabled = true;
        else
            pointerRef.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update()
    {
        aiAgent.SetDestination(target.position); // Start moving to the specified destination

        // Check if we are close to the specified destination...
        if (transform.position.x < target.position.x + distCheckPadding &&
            transform.position.x > target.position.x - distCheckPadding &&
            transform.position.z < target.position.z + distCheckPadding &&
            transform.position.z > target.position.z - distCheckPadding)
            target.position = UpdateMoveTo().position; // ...and get a new destination if we are

        pointerRef.transform.position = target.position;
	}

    // Used to get a new destination to move to
    NavMeshHit UpdateMoveTo()
    {
        Vector3 randDirection = Random.insideUnitSphere * 100.0f;
        randDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randDirection, out hit, 100.0f, 1);
        Debug.Log("Setting a new destination: (" + hit.position.x + ", " + hit.position.z + ")");
        return hit;
    }
}
