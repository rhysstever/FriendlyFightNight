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
        player.FindAction("Pause").started += PauseGame;
        player.Enable();
    }

    private void OnDisable() {
        player.FindAction("Jump").started -= Jump;
        player.FindAction("Fire").started -= Fire;
        player.FindAction("Block").started -= Block;
        player.FindAction("Special").started -= Special;
        player.FindAction("Pause").started -= PauseGame;
        player.Disable();
    }

    public Vector2 GetMove() {
        return move.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext obj) {
        if(GameManager.instance.CurrentMenuState == MenuState.CharacterSelect) {
            int playerNum = PlayerManager.instance.GetPlayerNum(gameObject);
            CharacterSelectManager.instance.Submit(playerNum);
        } else if(GameManager.instance.CurrentMenuState == MenuState.Game) {
            playerMovement.Jump();
        }
    }

    private void Fire(InputAction.CallbackContext obj) {
        playerCombat.FireBullet();
    }

    private void Block(InputAction.CallbackContext obj) {
        Debug.Log("Blocked");
    }

    private void Special(InputAction.CallbackContext obj) {
        Transform playerChild = transform.GetChild(0);
        playerChild.GetComponent<PlayerCombat>().GetAbility(false).UseSpecial();
    }

    private void PauseGame(InputAction.CallbackContext obj) {
        GameManager.instance.ChangeMenuState(MenuState.Pause);
    }

    public void UpdateCombat(PlayerCombat playerCombat) {
        this.playerCombat = playerCombat;
    }
}
