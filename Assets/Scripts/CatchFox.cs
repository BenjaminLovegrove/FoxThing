﻿using UnityEngine;
using System.Collections;

public class CatchFox : MonoBehaviour {

    public GameObject toEnable;
    public MeshCollider[] toDisable;

	void OnTriggerEnter (Collider col) {
        Invoke("Caught", 1.5f);
        foreach (MeshCollider fenceCol in toDisable)
        {
            fenceCol.enabled = false;
        }
	}

    void Caught()
    {
        toEnable.SetActive(true);
    }
	
}