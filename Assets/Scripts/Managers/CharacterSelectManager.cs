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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCharacterSelections() {
        // Clear all of the ui parent's children
        for(int i = uiParent.childCount - 1; i >= 0; i--) { 
            Destroy(uiParent.GetChild(i).gameObject);
        }

        Vector2 fullSize = new Vector2(
            pack1CharacterSelections[0].transform.localScale.x * itemsPerRow + offset.x * (itemsPerRow - 1),
            pack1CharacterSelections[0].transform.localScale.y * (totalCharacters / itemsPerRow) + offset.y * (totalCharacters / itemsPerRow - 1)
            );

        foreach(PlayerPack characterPack in characterPacks.Keys) {
            // Loop through the character pack and display each character
            for(int i = 0; i < characterPacks[characterPack].Count; i++) {
                GameObject characterObject = characterPacks[characterPack][i];

                // Figure out the position for the character 
                Vector2 position = new Vector2((-fullSize.x / itemsPerRow) + offset.x * (i % itemsPerRow), (-fullSize.y / 2) - offset.y * (i / itemsPerRow));

                Instantiate(characterObject, position, Quaternion.identity, uiParent);
            }
        }
    }

    public void ShowCharacterSelectors(int playerCount) {
        uiParent.transform.GetChild(0).GetComponent<CharacterSelectItem>().Focus(playerCount - 1);
    }

    public bool CanUpdateSelection(int playerNum) {
        return !playerReadyStatuses[playerNum];
    }

    public void UpdateCharacterSelection(Vector2 move, int playerNum) {
        if(!CanUpdateSelection(playerNum)) {
            return;
        }

        changedSelectionTimer += Time.deltaTime;

        if(move.magnitude > 0.5f
            && changedSelectionTimer >= changedSelectionRate) {
            float threshold = 0.25f;

            if(Mathf.Abs(move.x) > threshold) {
                if(move.x > 0) {
                    MoveFocusRight(playerNum);
                } else {
                    MoveFocusLeft(playerNum);
                }
                changedSelectionTimer = 0.0f;
            } else if(Mathf.Abs(move.y) > threshold) {
                if(move.y > 0) {
                    MoveFocusDown(playerNum);
                } else {
                    MoveFocusUp(playerNum);
                }
                changedSelectionTimer = 0.0f;
            }
        }
    }

    public void Submit(int playerNum) {
        // Update the player's ready status and UI
        playerReadyStatuses[playerNum] = true;
        UIManager.instance.UpdateCharacterSelectPlayerSubText();

        // Check if at least one player is NOT ready
        foreach(bool playerReady in playerReadyStatuses) {
            if(!playerReady) {
                return;
            }
        }

        // If all players are ready, move on to the map select
        GameManager.instance.ChangeMenuState(MenuState.MapSelect);
    }

    private void MoveFocusRight(int playerNum) {
        int newFocus = playerFocusIndecies[playerNum] + 1;

        if(newFocus >= totalCharacters) {
            newFocus %= totalCharacters;
        }

        UpdateSelection(newFocus, playerNum);
    }

    private void MoveFocusLeft(int playerNum) {
        int newFocus = playerFocusIndecies[playerNum] - 1;

        if(newFocus < 0) {
            newFocus += totalCharacters;
        }

        UpdateSelection(newFocus, playerNum);
    }

    private void MoveFocusDown(int playerNum) {
        int newFocus = playerFocusIndecies[playerNum] + itemsPerRow;

        if(newFocus >= totalCharacters) {
            newFocus %= totalCharacters;
        }

        UpdateSelection(newFocus, playerNum);
    }

    private void MoveFocusUp(int playerNum) {
        int newFocus = playerFocusIndecies[playerNum] - itemsPerRow;

        if(newFocus < 0) {
            newFocus += totalCharacters;
        }

        UpdateSelection(newFocus, playerNum);
    }

    private void UpdateSelection(int newFocus, int playerNum) {
        // Deselect the old focus
        uiParent.GetChild(playerFocusIndecies[playerNum]).GetComponent<CharacterSelectItem>().Unfocus(playerNum);
        // Update focus within the bounds of the available characters
        playerFocusIndecies[playerNum] = newFocus;
        // Update the Character the player is focused on
        playerCharacterSelections[playerNum] = uiParent.GetChild(playerFocusIndecies[playerNum]).GetComponent<CharacterSelectItem>().Focus(playerNum);
        // Change the Character for the player
        PlayerManager.instance.ChangeCharacter(playerCharacterSelections[playerNum], playerNum);
    }
}
