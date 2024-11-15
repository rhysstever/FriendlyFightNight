using System.Collections;
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

    public Character Focus(int playerNum) {
        isFocusedByPlayers[playerNum] = true;
        playerSelectors[playerNum].SetActive(true);

        return character;
    }

    public void Unfocus(int playerNum) {
        isFocusedByPlayers[playerNum] = false;
        playerSelectors[playerNum].SetActive(false);
    }

    public void Reset() {
        for(int i = 0; i < playerSelectors.Count; i++) {
            isFocusedByPlayers[i] = false;
            playerSelectors[i].SetActive(false);
        }
    }
}
