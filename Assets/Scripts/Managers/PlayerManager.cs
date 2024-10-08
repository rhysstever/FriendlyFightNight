using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Character
{
    Rhys,
    Grace,
    Sam,
    Con
}

public class PlayerManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static PlayerManager instance = null;

    // Awake is called even before start
    private void Awake()
    {
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

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerLeft -= AddPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        characters = new Dictionary<Character, GameObject>();
        foreach(GameObject character in characterPrefabs)
        {
            characters.Add(character.GetComponent<PlayerCombat>().CharacterName, character);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        playerInputs.Add(playerInput);
        playerInput.gameObject.name = "Player" + playerInputs.Count;
        playerInput.transform.position = spawnPoints[playerInputs.Count - 1].position;
    }

    public void ChangeCharacter(GameObject currentCharacter, int characterChange)
    {
        // Find the current character
        Character characterName = currentCharacter.transform.GetChild(0).GetComponent<PlayerCombat>().CharacterName;
        bool spriteFlipX = currentCharacter.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;

        // Get the next character
        int characterIndex = (int)characterName;
        int newCharacterIndex = characterIndex + characterChange;

        if(newCharacterIndex >= characters.Count)
            newCharacterIndex = 0;
        else if(newCharacterIndex < 0)
            newCharacterIndex = characters.Count - 1;

        // Character newCharacter = (Character)newCharacterIndex;
        Character newCharacter = (Character)newCharacterIndex;
        GameObject newCharacterPrefab = characters[newCharacter];

        // Remove the current character
        Destroy(currentCharacter.transform.GetChild(0).gameObject);

        // Add the new character
        GameObject newCharacterObject = Instantiate(newCharacterPrefab, currentCharacter.transform);
        currentCharacter.GetComponent<PlayerInputControls>().UpdateCombat(newCharacterObject.GetComponent<PlayerCombat>());
        currentCharacter.GetComponent<PlayerMovement>().UpdateAnimator(newCharacterObject.GetComponent<Animator>());
        newCharacterObject.GetComponent<SpriteRenderer>().flipX = spriteFlipX;
        currentCharacter.gameObject.name = newCharacter.ToString();

        // Update UI
        UIManager.instance.UpdatePlayerNames();
    }
}
