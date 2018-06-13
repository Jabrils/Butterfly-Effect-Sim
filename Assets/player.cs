using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
    public bool next;

    void OnTriggerEnter(Collider other)
    {
        next =  true;
    }
}
