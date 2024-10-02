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
    private List<GameObject> playerHealthBars, playerSpecialBars;

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
        int min = Mathf.Min(PlayerManager.instance.PlayerInputs.Count, playerHealthBars.Count);
        for(int i = 0; i < min; i++)
        {
            float healthPercent = PlayerManager.instance.PlayerInputs[i].transform.GetChild(0).GetComponent<PlayerCombat>().HealthPercentage;
            healthPercent = Mathf.Clamp(healthPercent, healthPercent, 1.0f);
            playerHealthBars[i].transform.localScale = new Vector3(healthPercent, 1.0f, 1.0f);
        }
    }

    public void UpdatePlayerSpecial()
    {
        int min = Mathf.Min(PlayerManager.instance.PlayerInputs.Count, playerSpecialBars.Count);
        for(int i = 0; i < min; i++)
        {
            Transform child = PlayerManager.instance.PlayerInputs[i].transform.GetChild(0);
            if(child.GetComponent<ActiveAbility>() != null)
            {
                float specialPercent = child.GetComponent<ActiveAbility>().SpecialPercentage;
                specialPercent = Mathf.Clamp(specialPercent, specialPercent, 1.0f);
                playerSpecialBars[i].transform.localScale = new Vector3(specialPercent, 1.0f, 1.0f);
            }
        }
    }
}
