using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[SelectionBase]


public class Player : MonoBehaviour {

    public static Player Instantce { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float _movingSpeed = 5f;
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;
    Vector2 inputVector;

    private Rigidbody2D _rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;


    private void Awake() {
        Instantce = this;
        _rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();
    }

    private void Start() {
        _currentHealth = _maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        GameInpit.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e) {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }


    private void Update() {
        inputVector = GameInpit.Instance.GetMovementVector();
    }

    private void FixedUpdate() {
        if (_knockBack.IsGettingKnockedBack) {
            return;
        }

        HandleMovement();
    }

    public bool IsAlive() => _isAlive;


    public void TakeDamage(Transform damageSource, int damage) {

        if (_canTakeDamage && _isAlive) {
            _canTakeDamage = false;
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);
            Debug.Log(_currentHealth);
            _knockBack.GetKnockedBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRountine());
        }

        DetectDeath();
    }

    private void DetectDeath() {
        if (_currentHealth == 0 && _isAlive) {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            GameInpit.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRountine() {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void HandleMovement() {
        _rb.MovePosition(_rb.position + inputVector * _movingSpeed * Time.fixedDeltaTime);

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed) {
            _isRunning = true;
        }
        else {
            _isRunning = false;
        }
    }

    public bool IsRunning() {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDestroy() {
        GameInpit.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }
}
