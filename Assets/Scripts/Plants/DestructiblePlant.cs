using UnityEngine;

public class DestructiblePlant : MonoBehaviour {

    public event System.EventHandler OnDestructibleTakeDamage;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Sword>()) {
            OnDestructibleTakeDamage?.Invoke(this, System.EventArgs.Empty);
            Destroy(gameObject);
            Invoke(nameof(DelayedRebake), 0f);
        }
    }

    private void DelayedRebake() {
        NavMeshSurfaceManagement.Instance.RebakeNavmeshSurface();
    }
}

