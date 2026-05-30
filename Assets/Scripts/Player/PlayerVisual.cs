using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private FlashBlink _flashblink;

    private const string IS_DIE = "IsDie";


    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _flashblink = GetComponent<FlashBlink>();
    }

    private void Start() {
        Player.Instantce.OnPlayerDeath += Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, System.EventArgs e) {
        animator.SetBool(IS_DIE, true);
        _flashblink.StopBlinking();
    }

    private void Update() {
        animator.SetBool("IsRunning", Player.Instantce.IsRunning());
        if (Player.Instantce.IsAlive()) {
            AdjustPlayerFacingDirection();
        }
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = GameInpit.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instantce.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }
    }

    private void OnDestroy() {
        Player.Instantce.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
