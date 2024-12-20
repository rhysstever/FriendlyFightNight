using System.Collections.Generic;
using UnityEngine;

public enum PlayerPack {
    Pack1
}

public class CharacterSelectManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static CharacterSelectManager instance = null;

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
    private Transform uiParent;
    [SerializeField]
    private List<GameObject> pack1CharacterSelections;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private int itemsPerRow;

    private List<int> playerFocusIndecies;
    private List<bool> playerReadyStatuses;
    private List<Character> playerCharacterSelections;

    private int totalCharacters;
    private Dictionary<PlayerPack, List<GameObject>> characterPacks;

    private float changedSelectionRate;
    private float changedSelectionTimer;

    public List<bool> PlayerReadyStatuses { get { return playerReadyStatuses; } }

    // Start is called before the first frame update
    void Start()
    {
        playerReadyStatuses = new List<bool>();
        playerFocusIndecies = new List<int>();
        playerCharacterSelections = new List<Character>();

        for(int i = 0; i < PlayerManager.instance.PlayerInputManager.maxPlayerCount; i++) {
            playerReadyStatuses.Add(false);
            playerFocusIndecies.Add(0);
            playerCharacterSelections.Add(Character.Rhys);
        }        

        characterPacks = new Dictionary<PlayerPack, List<GameObject>>();
        characterPacks.Add(PlayerPack.Pack1, pack1CharacterSelections);

        totalCharacters = 0;
        foreach(List<GameObject> characterPackList in characterPacks.Values) { 
            totalCharacters += characterPackList.Count;
        }

        changedSelectionRate = 0.25f;
        changedSelectionTimer = changedSelectionRate;
    }

    /// <summary>
    /// Create a grid of selectable character objects
    /// </summary>
    public void DisplayCharacterSelections() {
        // Clear all of the ui parent's children
        for(int i = uiParent.childCount - 1; i >= 0; i--) { 
            Destroy(uiParent.GetChild(i).gameObject);
        }

        // Calculate the full size based on the number of selectable character and the offets
        Vector2 fullSize = new Vector2(
            pack1CharacterSelections[0].transform.localScale.x * itemsPerRow + offset.x * (itemsPerRow - 1),
            pack1CharacterSelections[0].transform.localScale.y * (totalCharacters / itemsPerRow) + offset.y * (totalCharacters / itemsPerRow - 1));

        foreach(PlayerPack characterPack in characterPacks.Keys) {
            // Loop through the character pack and display each character
            for(int i = 0; i < characterPacks[characterPack].Count; i++) {
                GameObject characterObject = characterPacks[characterPack][i];

                // Figure out the position for the character 
                Vector2 position = new Vector2(
                    (-fullSize.x / itemsPerRow) + offset.x * (i % itemsPerRow), 
                    (-fullSize.y / 2) - offset.y * (i / itemsPerRow));

                Instantiate(characterObject, position, Quaternion.identity, uiParent);
            }
        }
    }

    /// <summary>
    /// Shows a character selector based on the last player joined
    /// </summary>
    /// <param name="playerCount">The current player count</param>
    public void ShowCharacterSelectors(int playerCount) {
        uiParent.transform.GetChild(0).GetComponent<CharacterSelectItem>().Focus(playerCount - 1);
    }

    /// <summary>
    /// Gets whether the current player can update its focused character (they havent ready'ed up yet)
    /// </summary>
    /// <param name="playerIndex">The player's number (based 0)</param>
    /// <returns>Whether the current player can update its focused character</returns>
    private bool CanUpdateSelection(int playerIndex) {
        return !playerReadyStatuses[playerIndex];
    }

    /// <summary>
    /// Update a player's focused character object
    /// </summary>
    /// <param name="move">The input move from the controller</param>
    /// <param name="playerIndex">The player number that is changing its focus</param>
    public void UpdateCharacterSelection(Vector2 move, int playerIndex) {
        // Exit early if the character focus cannot be updated
        if(!CanUpdateSelection(playerIndex)) {
            return;
        }

        // Update the time since the last focus change
        changedSelectionTimer += Time.deltaTime;

        // If the move input is strong enough and there has been enough time since the last focus change
        if(move.magnitude > 0.5f
            && changedSelectionTimer >= changedSelectionRate) {
            float threshold = 0.25f;

            // Move the focus based on the move input
            if(Mathf.Abs(move.x) > threshold) {
                if(move.x > 0) {
                    MoveFocusRight(playerIndex);
                } else {
                    MoveFocusLeft(playerIndex);
                }
                // Reset the timer
                changedSelectionTimer = 0.0f;
            } else if(Mathf.Abs(move.y) > threshold) {
                if(move.y > 0) {
                    MoveFocusDown(playerIndex);
                } else {
                    MoveFocusUp(playerIndex);
                }
                // Reset the timer
                changedSelectionTimer = 0.0f;
            }
        }
    }

    /// <summary>
    /// Ready up a player
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    public void Submit(int playerIndex) {
        // Update the player's ready status and UI
        playerReadyStatuses[playerIndex] = true;
        UIManager.instance.UpdateCharacterSelectPlayerSubText(playerIndex, playerCharacterSelections[playerIndex]);

        // Check if at least one player is NOT ready
        foreach(bool playerReady in playerReadyStatuses) {
            if(!playerReady) {
                return;
            }
        }

        // If all players are ready, move on to the map select
        GameManager.instance.ChangeMenuState(MenuState.MapSelect);
    }

    /// <summary>
    /// Moves a player's focus to the item to the right
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    private void MoveFocusRight(int playerIndex) {
        int newFocus = playerFocusIndecies[playerIndex] + 1;

        if(newFocus >= totalCharacters) {
            newFocus %= totalCharacters;
        }

        UpdateFocusData(newFocus, playerIndex);
    }

    /// <summary>
    /// Moves a player's focus to the item to the left
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    private void MoveFocusLeft(int playerIndex) {
        int newFocus = playerFocusIndecies[playerIndex] - 1;

        if(newFocus < 0) {
            newFocus += totalCharacters;
        }

        UpdateFocusData(newFocus, playerIndex);
    }

    /// <summary>
    /// Moves a player's focus to the item below
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    private void MoveFocusDown(int playerIndex) {
        int newFocus = playerFocusIndecies[playerIndex] + itemsPerRow;

        if(newFocus >= totalCharacters) {
            newFocus %= totalCharacters;
        }

        UpdateFocusData(newFocus, playerIndex);
    }

    /// <summary>
    /// Moves a player's focus to the item above
    /// </summary>
    /// <param name="playerIndex">The player's index</param>
    private void MoveFocusUp(int playerIndex) {
        int newFocus = playerFocusIndecies[playerIndex] - itemsPerRow;

        if(newFocus < 0) {
            newFocus += totalCharacters;
        }

        UpdateFocusData(newFocus, playerIndex);
    }

    /// <summary>
    /// Update the focus data of a player
    /// </summary>
    /// <param name="newFocus">The index of the new focused item</param>
    /// <param name="playerIndex">The player's index</param>
    private void UpdateFocusData(int newFocus, int playerIndex) {
        // Deselect the old focus
        uiParent.GetChild(playerFocusIndecies[playerIndex]).GetComponent<CharacterSelectItem>().Unfocus(playerIndex);
        // Update focus within the bounds of the available characters
        playerFocusIndecies[playerIndex] = newFocus;
        // Update the Character the player is focused on
        playerCharacterSelections[playerIndex] = uiParent.GetChild(playerFocusIndecies[playerIndex]).GetComponent<CharacterSelectItem>().Focus(playerIndex);
        // Change the Character for the player
        PlayerManager.instance.ChangeCharacter(playerIndex, playerCharacterSelections[playerIndex]);
    }
}
