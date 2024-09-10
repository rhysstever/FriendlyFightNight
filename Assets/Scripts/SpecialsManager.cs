using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    Con,
    Grace,
    Rhys,
    Sam
}

public class SpecialsManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static SpecialsManager instance = null;

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
    private GameObject rhysSpecialBullet;
    [SerializeField]
    private Vector2 rhysSpecialBulletSpeed;
    [SerializeField]
    private float rhysSpecialBulletLifespan, graceSpecialHealAmount;

    public float RhysSpecialBulletLifespan { get { return rhysSpecialBulletLifespan;} }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseSpecial(GameObject playerObject, Character character)
    {
        switch(character)
        {
            case Character.Rhys:
                playerObject.GetComponent<PlayerCombat>().Fire(
                    rhysSpecialBullet, 
                    rhysSpecialBulletSpeed, 
                    FirePosition.Up,
                    new Vector2(0.45f, 0.25f));
                break;
            case Character.Grace:
                playerObject.GetComponent<PlayerCombat>().Heal(graceSpecialHealAmount);
                break;
            default:
                Debug.Log("Default Special Used");
                break;
        }
    }
}
