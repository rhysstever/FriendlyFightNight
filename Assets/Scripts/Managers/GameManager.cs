using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState {
    Title,
    CharacterSelect,
    MapSelect,
    Game,
    Pause,
    Results
}

public enum InputType {
    Player,
    Menu
}

public class GameManager : MonoBehaviour {
    #region Singleton Code
    // A public reference to this script
    public static GameManager instance = null;

    // Awake is called even before start
    private void Awake() {
        // If the reference for this script is null, assign it this script
        if(instance == null)
            instance = this;
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        else if(instance != this)
            Destroy(gameObject);
    }
    #endregion

    [SerializeField]
    private MenuState currentMenuState;
    [SerializeField]
    private GameObject bullets;

    private Dictionary<InputType, string> inputTypes;

    public MenuState CurrentMenuState { get { return currentMenuState; } }
    public GameObject Bullets { get { return bullets; } }

    // Start is called before the first frame update
    void Start() {
        // Map each Input Type to the corresponding string value
        inputTypes = new Dictionary<InputType, string>() {
            [InputType.Player] = "Player",
            [InputType.Menu] = "UI"
        };

        ChangeMenuState(currentMenuState);
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Changes the current menu state and performs any one time logic
    /// </summary>
    /// <param name="newMenuState">The new menu state</param>
    public void ChangeMenuState(MenuState newMenuState) {
        switch(newMenuState) {
            case MenuState.Title:
                ChangePlayerInputs(InputType.Menu);
                break;
            case MenuState.CharacterSelect:
                ChangePlayerInputs(InputType.Menu);
                CharacterSelectManager.instance.DisplayCharacterSelections();
                break;
            case MenuState.MapSelect:
                ChangePlayerInputs(InputType.Menu);
                break;
            case MenuState.Game:
                ChangePlayerInputs(InputType.Player);
                break;
            case MenuState.Pause:
                ChangePlayerInputs(InputType.Menu);
                break;
            case MenuState.Results:
                ChangePlayerInputs(InputType.Menu);
                break;
        }

        UIManager.instance.UpdateUI(newMenuState);
        currentMenuState = newMenuState;
    }

    /// <summary>
    /// Switches all controllers' action maps
    /// </summary>
    /// <param name="inputType">The input type that each controller should change to</param>
    private void ChangePlayerInputs(InputType inputType) {
        PlayerManager.instance.PlayerInputs.ForEach(input => {
            input.SwitchCurrentActionMap(inputTypes[inputType]);
        });
    }
}
