using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sword : MonoBehaviour {

    [SerializeField] private int _damageAmount = 2;

    public event EventHandler OnSwordSwing;

    private PolygonCollider2D _polygonCollyder2D;

    private void Awake() {
        _polygonCollyder2D = GetComponent<PolygonCollider2D>();
    }

    private void Start() {
        AttackColliderTurnOff();
    }


    public void Attack() {
        AttackColliderTurnOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity)) {
            enemyEntity.TakeDamage(_damageAmount);
        }
    }

    public void AttackColliderTurnOff() {
        _polygonCollyder2D.enabled = false;
    }

    private void AttackColliderTurnOn() {
        _polygonCollyder2D.enabled = true;
    }

    private void AttackColliderTurnOffOn() {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}
