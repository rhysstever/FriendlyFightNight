using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Character {
    Rhys,
    Grace,
    Sam,
    Con
}

public class PlayerManager : MonoBehaviour {
    #region Singleton Code
    // A public reference to this script
    public static PlayerManager instance = null;

    // Awake is called even before start
    private void Awake() {
        // If the reference for this script is null, assign it this script
        if(instance == null)
            instance = this;
        // If the reference is to something else (it already exists)
        // than this is not needed, thus destroy it
        else if(instance != this)
            Destroy(gameObject);

        playerInputs = new List<PlayerInput>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }
    #endregion

    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private List<GameObject> characterPrefabs;

    private Dictionary<Character, GameObject> characters;

    private List<PlayerInput> playerInputs;
    private PlayerInputManager playerInputManager;

    public List<PlayerInput> PlayerInputs { get { return playerInputs; } }
    public PlayerInputManager PlayerInputManager { get { return playerInputManager; } }

    private void OnEnable() {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable() {
        playerInputManager.onPlayerLeft -= AddPlayer;
    }

    // Start is called before the first frame update
    void Start() {
        // Create a dictionary of characters and the corresponding prefab object
        characters = new Dictionary<Character, GameObject>();
        foreach(GameObject characterPrefab in characterPrefabs) {
            characters.Add(characterPrefab.GetComponent<PlayerCombat>().Character, characterPrefab);
        }
    }

    /// <summary>
    /// Find a player's index from the GameObject
    /// </summary>
    /// <param name="playerObject">The player's parent GameObject</param>
    /// <returns>The player's index</returns>
    public int GetPlayerNum(GameObject playerObject) {
        for(int i = 0; i < playerInputs.Count; i++) {
            if(playerInputs[i].gameObject == playerObject) {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Adds a PlayerInput to the scene and updates its values
    /// </summary>
    /// <param name="playerInput">The PlayerInput being added to the scene</param>
    public void AddPlayer(PlayerInput playerInput) {
        playerInputs.Add(playerInput);
        playerInput.gameObject.name = "Player" + playerInputs.Count;
        playerInput.transform.position = spawnPoints[playerInputs.Count - 1].position;
        
        // Flip the sprite based on the player count
        playerInput.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = playerInputs.Count % 2 == 0;

        UIManager.instance.UpdateAllPlayerUI();
    }

    /// <summary>
    /// Changes a character object to another character
    /// </summary>
    /// <param name="newCharacter">The new Character</param>
    /// <param name="playerNum">The player's index</param>
    public void ChangeCharacter(int playerNum, Character newCharacter) {
        GameObject currentPlayerObject = playerInputs[playerNum].gameObject;

        // Get the new character's prefab
        GameObject newCharacterPrefab = characters[newCharacter];

        // Remove effects from other characters
        foreach(PlayerInput playerInput in playerInputs) {
            if(playerInput.gameObject != currentPlayerObject) {
                ClearDebuffs(playerInput.transform.GetChild(0).gameObject);
            }
        }

        // Remove the current character
        Destroy(currentPlayerObject.transform.GetChild(0).gameObject);

        // Add the new character
        GameObject newCharacterObject = Instantiate(newCharacterPrefab, currentPlayerObject.transform);

        // Update the parent player object with new references to components of the new child character object
        currentPlayerObject.GetComponent<PlayerInputControls>().UpdateCombat(newCharacterObject.GetComponent<PlayerCombat>());
        currentPlayerObject.GetComponent<PlayerMovement>().SetNewAnimator(newCharacterObject.GetComponent<Animator>());

        // Flip the sprite if the player num is odd
        newCharacterObject.GetComponent<SpriteRenderer>().flipX = playerNum % 2 == 1;

        // Get the index of player (indexed at 1)
        if(int.TryParse(currentPlayerObject.name.Substring("Player".Length), out int index)) {
            // Update Player Name UI
            UIManager.instance.UpdatePlayerNames(index - 1, newCharacter.ToString());
        }
    }

    /// <summary>
    /// Clear character of all debuff effects
    /// </summary>
    /// <param name="characterObject">The character child object being cleansed</param>
    private void ClearDebuffs(GameObject characterObject) {
        foreach(Effect effect in characterObject.GetComponents<Effect>()) {
            if(!effect.IsBuff) {
                effect.Toggle(false);
                Destroy(effect);
            }
        }
    }
}
