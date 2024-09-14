using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private List<PlayerInput> playerInputs;
    [SerializeField]
    private List<Transform> spawnPoints;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        playerInputs.Add(playerInput);
        playerInput.transform.position = spawnPoints[playerInputs.Count - 1].position;
    }
}
