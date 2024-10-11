using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControls : MonoBehaviour {
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private void Awake() {
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    // Start is called before the first frame update
    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = transform.GetChild(0).GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnEnable() {
        player.FindAction("Jump").started += Jump;
        player.FindAction("Fire").started += Fire;
        player.FindAction("Block").started += Block;
        player.FindAction("Special").started += Special;
        move = player.FindAction("Move");
        player.FindAction("ChangeCharacterUp").started += ChangeCharacterUp;
        player.FindAction("ChangeCharacterDown").started += ChangeCharacterDown;
        player.Enable();
    }

    private void OnDisable() {
        player.FindAction("Jump").started -= Jump;
        player.FindAction("Fire").started -= Fire;
        player.FindAction("Block").started -= Block;
        player.FindAction("Special").started -= Special;
        player.FindAction("ChangeCharacterUp").started -= ChangeCharacterUp;
        player.FindAction("ChangeCharacterDown").started -= ChangeCharacterDown;
        player.Disable();
    }

    public Vector2 GetMove() {
        return move.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext obj) {
        playerMovement.Jump();
    }

    private void Fire(InputAction.CallbackContext obj) {
        playerCombat.FireBullet();
    }

    private void Block(InputAction.CallbackContext obj) {
        Debug.Log("Blocked");
    }

    private void Special(InputAction.CallbackContext obj) {
        Transform playerChild = transform.GetChild(0);
        ActiveType activeType = playerChild.GetComponent<ActiveAbility>().ActiveType;
        switch(activeType) {
            case ActiveType.Default:
                playerChild.GetComponent<ActiveAbility>().UseSpecial();
                break;
            case ActiveType.SpecialAttack:
                playerChild.GetComponent<SpecialAttack>().UseSpecial();
                break;
            case ActiveType.Heal:
                playerChild.GetComponent<Heal>().UseSpecial();
                break;
            case ActiveType.Buff:
                playerChild.GetComponent<ActiveAbility>().UseSpecial();
                break;
            case ActiveType.Debuff:
                playerChild.GetComponent<ActiveAbility>().UseSpecial();
                break;
            default:
                playerChild.GetComponent<ActiveAbility>().UseSpecial();
                break;
        }
    }

    private void ChangeCharacterUp(InputAction.CallbackContext obj) {
        PlayerManager.instance.ChangeCharacter(gameObject, 1);
    }

    private void ChangeCharacterDown(InputAction.CallbackContext obj) {
        PlayerManager.instance.ChangeCharacter(gameObject, -1);
    }

    public void UpdateCombat(PlayerCombat playerCombat) {
        this.playerCombat = playerCombat;
    }
}
