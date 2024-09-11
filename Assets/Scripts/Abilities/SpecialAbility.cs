using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Passive,
    Active
}

public class SpecialAbility : MonoBehaviour
{
    [SerializeField]
    private string abilityName;
    internal AbilityType abilityType;

    public AbilityType AbilityType { get { return abilityType; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
