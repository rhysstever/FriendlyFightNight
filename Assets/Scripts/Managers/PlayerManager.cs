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
    /// Adds a PlayerInput to the scene and updates its values
    /// </summary>
    /// <param name="playerInput">The PlayerInput being added to the scene</param>
    public void AddPlayer(PlayerInput playerInput) {
        if(GameManager.instance.CurrentMenuState == MenuState.CharacterSelect) {
            playerInputs.Add(playerInput);
            playerInput.gameObject.name = "Player" + playerInputs.Count;
            playerInput.transform.position = spawnPoints[playerInputs.Count - 1].position;
            UIManager.instance.UpdateAllPlayerUI();
        }
    }

    public void ChangeCharacter(Character newCharacter, int playerNum) {
        GameObject currentPlayerObject = playerInputs[playerNum].gameObject;
        ChangeCharacter(currentPlayerObject, newCharacter);
    }

    /// <summary>
    /// Changes a character object to another character
    /// </summary>
    /// <param name="currentCharacter">The current character object</param>
    /// <param name="characterChange">How index of the current character will change</param>
    public void ChangeCharacter(GameObject currentCharacter, Character newCharacter) {
        // Use the current character to get the sprite facing direction
        bool spriteFlipX = currentCharacter.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;

        // Get the new character's prefab
        GameObject newCharacterPrefab = characters[newCharacter];

        // Remove the current character
        Destroy(currentCharacter.transform.GetChild(0).gameObject);

        // Add the new character
        GameObject newCharacterObject = Instantiate(newCharacterPrefab, currentCharacter.transform);

        // Update the parent player object with new references to components of the new child character object
        currentCharacter.GetComponent<PlayerInputControls>().UpdateCombat(newCharacterObject.GetComponent<PlayerCombat>());
        currentCharacter.GetComponent<PlayerMovement>().SetNewAnimator(newCharacterObject.GetComponent<Animator>());

        // Ensure the sprite is facing the same way as it was before
        newCharacterObject.GetComponent<SpriteRenderer>().flipX = spriteFlipX;

        // Get the index of player (indexed at 1)
        if(int.TryParse(currentCharacter.name.Substring("Player".Length), out int index)) {
            // Update Player Name UI
            UIManager.instance.UpdatePlayerNames(index - 1, newCharacter.ToString());
        }
    }

    public int GetPlayerNum(GameObject parentObject) {
        for(int i = 0; i < playerInputs.Count; i++) {
            if(playerInputs[i].gameObject == parentObject) {
                return i;
            }
        }

        return -1;
    }
}
