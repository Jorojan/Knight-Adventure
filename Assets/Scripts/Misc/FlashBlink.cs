using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBlink : MonoBehaviour {
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuraction = 0.2f;

    private float _blinkTimer;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;
    private bool _isBlinking;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        _isBlinking = true;
    }

    private void Start() {
        if (_damagableObject is Player) {
            (_damagableObject as Player).OnFlashBlink += DamagleObject_OnFlashBlink;
        }
    }

    private void DamagleObject_OnFlashBlink(object sender, System.EventArgs e) {
        SetBlinkingMaterial();
    }

    private void Update() {
        if (_isBlinking) {
            _blinkTimer -= Time.deltaTime;
            if (_blinkTimer < 0) {
                SetDefaultMaterial();
            }
        }
    }

    private void SetBlinkingMaterial() {
        _blinkTimer = _blinkDuraction;
        spriteRenderer.material = _blinkMaterial;
    }

    private void SetDefaultMaterial() {
        spriteRenderer.material = defaultMaterial;
    }

    public void StopBlinking() {
        SetDefaultMaterial();
        _isBlinking = false;
    }

    private void OnDestroy() {
        if (_damagableObject is Player) {
            (_damagableObject as Player).OnFlashBlink -= DamagleObject_OnFlashBlink;
        }
    }
}
