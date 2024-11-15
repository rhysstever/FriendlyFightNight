using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectItem : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> playerSelectors;

    [SerializeField]
    private Character character;

    [SerializeField]
    private Sprite characterSprite;

    private List<bool> isFocusedByPlayers;

    // Start is called before the first frame update
    void Start()
    {
        isFocusedByPlayers = new List<bool>();
        for(int i = 0; i < playerSelectors.Count; i++) {
            isFocusedByPlayers.Add(false);
            Unfocus(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Focus the item for a player
    /// </summary>
    /// <param name="playerNum">The player's index</param>
    /// <returns>The Character of the item</returns>
    public Character Focus(int playerNum) {
        isFocusedByPlayers[playerNum] = true;
        playerSelectors[playerNum].SetActive(true);

        return character;
    }

    /// <summary>
    /// Unfocus the item for a player
    /// </summary>
    /// <param name="playerNum">The player's index</param>
    public void Unfocus(int playerNum) {
        isFocusedByPlayers[playerNum] = false;
        playerSelectors[playerNum].SetActive(false);
    }

    /// <summary>
    /// Reset the focus (unfocus) of the item for all players
    /// </summary>
    public void Reset() {
        for(int i = 0; i < playerSelectors.Count; i++) {
            isFocusedByPlayers[i] = false;
            playerSelectors[i].SetActive(false);
        }
    }
}
