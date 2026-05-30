using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlant : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Sword>()) {
            Destroy(gameObject);
        }
    }
}

