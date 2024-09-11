using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControls : MonoBehaviour
{
    [SerializeField]
    private InputActionReference move, jump, fire, block, special;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

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
        jump.action.started += Jump;
        fire.action.started += Fire;
        block.action.started += Block;
        special.action.started += Special;
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
        fire.action.started -= Fire;
        block.action.started -= Block;
        special.action.started -= Special;
    }

    public Vector2 GetMove()
    {
        return move.action.ReadValue<Vector2>();
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
