using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static UIManager instance = null;

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
    }
    #endregion

    [SerializeField]
    private List<GameObject> playerHealthBars;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerHealth()
    {
        int min = Mathf.Min(GameManager.instance.Players.transform.childCount, playerHealthBars.Count);
        for(int i = 0; i < min; i++)
        {
            float healthPercent = GameManager.instance.Players.transform.GetChild(i).GetComponent<PlayerCombat>().HealthPercentage;
            playerHealthBars[i].transform.localScale = new Vector3(healthPercent, 1.0f, 1.0f);
        }
    }
}