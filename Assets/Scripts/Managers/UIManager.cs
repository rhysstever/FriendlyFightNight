using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private List<TMP_Text> playerNameTexts;
    [SerializeField]
    private List<GameObject> playerHealthBars, playerSpecialBars;

    // Start is called before the first frame update
    void Start() {
        UpdateAllPlayerUI();
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateAllPlayerUI() {
        UpdatePlayerNames();
        UpdatePlayerHealth();
        UpdatePlayerSpecial();
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
                healthPercent = Mathf.Clamp(healthPercent, healthPercent, 1.0f);
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
                if(child.GetComponent<ActiveAbility>() != null) {
                    float specialPercent = child.GetComponent<ActiveAbility>().SpecialPercentage;
                    specialPercent = Mathf.Clamp(specialPercent, specialPercent, 1.0f);
                    playerSpecialBars[i].transform.localScale = new Vector3(specialPercent, 1.0f, 1.0f);
                }
            }
        }
    }
}
