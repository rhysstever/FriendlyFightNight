using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;
    public GameObject source;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision == null)
            return;

        if(collision.gameObject == null)
            return;

        switch(collision.gameObject.layer) {
            case 6: // Walls
                GameObject.Destroy(this.gameObject);
                break;
            case 7: // Other bullets
                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(collision.gameObject);
                break;
            case 8: // Players
                if(source.GetComponent<EffectOverTime>() != null 
                    && source.GetComponent<EffectOverTime>().Attribute == Attribute.Damage) {
                    source.GetComponent<EffectOverTime>().UseSpecial(collision.gameObject);
                }
                collision.gameObject.transform.GetChild(0).GetComponent<PlayerCombat>().TakeDamage(damage);
                GameObject.Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
}
