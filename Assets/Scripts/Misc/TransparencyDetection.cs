using System.Collections;
using UnityEngine;

public class TransparencyDetection : MonoBehaviour {
    [Range(0f, 1f)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTimne = 0.5f;
    SpriteRenderer _spriteRenderer;

    private const float FULL_NOT_TRANSPARENCT = 1.0f;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider is CapsuleCollider2D) {
            if (collider.gameObject.GetComponent<Player>()) {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTimne, _spriteRenderer.color.a, transparencyAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider is CapsuleCollider2D) {
            if (collider.gameObject.GetComponent<Player>()) {
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTimne, _spriteRenderer.color.a, FULL_NOT_TRANSPARENCT));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer _spriteRenderer, float fadeTime, float startTransparencyAmount, float targetTransparencyAmount) {
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime) {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransparencyAmount, targetTransparencyAmount, elapsedTime / fadeTime);
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, newAlpha);

            yield return null;
        }
    }
}
