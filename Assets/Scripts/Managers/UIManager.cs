using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    #region Singleton Code
    // A public reference to this script
    public static UIManager instance = null;

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
    private GameObject titleParent, characterSelectParent, mapSelectParent, gameParent, pauseParent, resultsParent;
    [SerializeField]
    private Button startButton, continueToGame;
    [SerializeField]
    private List<TMP_Text> characterSelectionPlayerSubTexts;
    [SerializeField]
    private List<TMP_Text> playerNameTexts;
    [SerializeField]
    private List<GameObject> playerHealthBars, playerSpecialBars;
    [SerializeField]
    private TMP_Text resultsText;
    [SerializeField]
    private Button backToMainMenuButton;

    private Dictionary<MenuState, GameObject> menuStateParents;

    // Start is called before the first frame update
    void Start() {
        // Map each menu state to a ui menu parent
        menuStateParents = new Dictionary<MenuState, GameObject>() {
            [MenuState.Title] = titleParent,
            [MenuState.CharacterSelect] = characterSelectParent,
            [MenuState.MapSelect] = mapSelectParent,
            [MenuState.Game] = gameParent,
            [MenuState.Pause] = pauseParent,
            [MenuState.Results] = resultsParent
        };

        startButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.CharacterSelect));

        continueToGame.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Game));

        backToMainMenuButton.onClick.AddListener(() => GameManager.instance.ChangeMenuState(MenuState.Title));

        UpdateAllPlayerGameUI();
    }

    // Update is called once per frame
    void Update() {
        UpdatePlayerSpecial();
    }

    public void UpdateUI(MenuState newMenuState) {
        foreach(KeyValuePair<MenuState, GameObject> menuStateData in menuStateParents) {
            menuStateData.Value.SetActive(menuStateData.Key == newMenuState);
        }

        switch(newMenuState) {
            case MenuState.Title:
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
                foreach(TMP_Text playerSubText in characterSelectionPlayerSubTexts) {
                    playerSubText.text = "";
                }
                break;
            case MenuState.CharacterSelect:
                foreach(TMP_Text playerSubText in characterSelectionPlayerSubTexts) {
                    playerSubText.text = "Press \'Start\' to Join";
                }
                break;
            case MenuState.MapSelect:
                EventSystem.current.SetSelectedGameObject(continueToGame.gameObject);
                break;
            case MenuState.Game:
                UpdateAllPlayerGameUI();
                break;
            case MenuState.Results:
                for(int i = 0; i < PlayerManager.instance.PlayerInputs.Count; i++) {
                    if(PlayerManager.instance.PlayerInputs[i].GetComponentInChildren<PlayerCombat>().HealthPercentage > 0.0f) {
                        resultsText.text = string.Format("Player {0} Wins!", i + 1);
                    }
                }
                EventSystem.current.SetSelectedGameObject(backToMainMenuButton.gameObject);
                break;
        }
    }

    public void UpdateAllPlayerGameUI() {
        UpdatePlayerNames();
        UpdatePlayerHealth();
        UpdatePlayerSpecial();
    }

    public void UpdateCharacterSelectPlayerSubText(int playerIndex, Character character) {
        string subTextString;
        if(CharacterSelectManager.instance.PlayerReadyStatuses[playerIndex]) {
            subTextString = string.Format("Player {0} is Ready!", playerIndex + 1);
        } else {
            PlayerCombat playerCombat = PlayerManager.instance.CharacterPrefabs[character].GetComponent<PlayerCombat>();

            subTextString = string.Format("{0}\n\n{1}\n{2}",
                playerCombat.Character,
                playerCombat.PassiveAbility.AbilityName,
                playerCombat.ActiveAbility.AbilityName);
        }

        characterSelectionPlayerSubTexts[playerIndex].text = subTextString;
    }

    /// <summary>
    /// Update the display name for each player in the scene
    /// </summary>
    public void UpdatePlayerNames() {
        for(int i = 0; i < playerNameTexts.Count; i++) {
            string characterName = "";
            if(i < PlayerManager.instance.PlayerInputs.Count) {
                characterName = PlayerManager.instance.PlayerInputs[i].GetComponentInChildren<PlayerCombat>().Character.ToString();
            }
            UpdatePlayerNames(i, characterName);
        }
    }

    /// <summary>
    /// Update a display name for a specific player
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    /// <param name="name">The new name for the player</param>
    public void UpdatePlayerNames(int playerIndex, string name) {
        if(playerIndex < PlayerManager.instance.PlayerInputs.Count) {
            playerNameTexts[playerIndex].text = name;
        } else {
            playerNameTexts[playerIndex].text = "";
        }
    }

    /// <summary>
    /// Update the health bar for each player in the scene
    /// </summary>
    public void UpdatePlayerHealth() {
        for(int i = 0; i < playerHealthBars.Count; i++) {
            bool playerExists = i < PlayerManager.instance.PlayerInputs.Count;
            playerHealthBars[i].transform.parent.gameObject.SetActive(playerExists);

            if(playerExists) {
                float healthPercent = PlayerManager.instance.PlayerInputs[i].transform.GetChild(0).GetComponent<PlayerCombat>().HealthPercentage;
                healthPercent = Mathf.Clamp(healthPercent, 0.0f, 1.0f);
                playerHealthBars[i].transform.localScale = new Vector3(healthPercent, 1.0f, 1.0f);
            }
        }
    }

    /// <summary>
    /// Update the special bar for each player in the scene
    /// </summary>
    public void UpdatePlayerSpecial() {
        for(int i = 0; i < playerSpecialBars.Count; i++) {
            bool playerExists = i < PlayerManager.instance.PlayerInputs.Count;
            playerSpecialBars[i].transform.parent.gameObject.SetActive(playerExists);

            if(playerExists) {
                Transform child = PlayerManager.instance.PlayerInputs[i].transform.GetChild(0);
                SpecialAbility activeAbility = child.GetComponent<PlayerCombat>().ActiveAbility;

                if(activeAbility != null) {
                    float specialPercent = activeAbility.CooldownPercentage;
                    specialPercent = Mathf.Clamp(specialPercent, specialPercent, 1.0f);
                    playerSpecialBars[i].transform.localScale = new Vector3(specialPercent, 1.0f, 1.0f);
                }
            }
        }
    }
}
