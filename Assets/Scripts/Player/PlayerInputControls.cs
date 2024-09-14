using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControls : MonoBehaviour
{
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        player.FindAction("Jump").started += Jump;
        player.FindAction("Fire").started += Fire;
        player.FindAction("Block").started += Block;
        player.FindAction("Special").started += Special;
        move = player.FindAction("Move");
        player.Enable();
    }

    private void OnDisable()
    {
        player.FindAction("Jump").started -= Jump;
        player.FindAction("Fire").started -= Fire;
        player.FindAction("Block").started -= Block;
        player.FindAction("Special").started -= Special;
        player.Disable();
    }

    public Vector2 GetMove()
    {
        return move.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        playerMovement.Jump();
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        playerCombat.FireBullet();
    }

    private void Block(InputAction.CallbackContext obj)
    {
        Debug.Log("Blocked");
    }

    private void Special(InputAction.CallbackContext obj)
    {
        ActiveType activeType = gameObject.GetComponent<ActiveAbility>().ActiveType;
        switch(activeType)
        {
            case ActiveType.Default:
                gameObject.GetComponent<ActiveAbility>().UseSpecial();
                break;
            case ActiveType.SpecialAttack:
                gameObject.GetComponent<SpecialAttack>().UseSpecial();
                break;
            case ActiveType.Heal:
                gameObject.GetComponent<Heal>().UseSpecial();
                break;
            case ActiveType.Buff:
                gameObject.GetComponent<ActiveAbility>().UseSpecial();
                break;
            case ActiveType.Debuff:
                gameObject.GetComponent<ActiveAbility>().UseSpecial();
                break;
            default:
                gameObject.GetComponent<ActiveAbility>().UseSpecial();
                break;
        }
    }
}
