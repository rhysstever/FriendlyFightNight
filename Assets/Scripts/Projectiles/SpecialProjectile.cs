using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialProjectile : MonoBehaviour {
    private float timer, damage, lifeSpan;

    // Start is called before the first frame update
    void Start() {
        timer = 0.0f;

        Debug.Log(gameObject.GetComponent<Rigidbody2D>().gravityScale);
    }

    private void FixedUpdate() {
        timer += Time.deltaTime;
        if(timer >= lifeSpan)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision == null)
            return;

        if(collision.gameObject == null)
            return;

        switch(collision.gameObject.layer) {
            case 6: // Walls
                break;
            case 7: // Bullets
                GameObject.Destroy(collision.gameObject);
                break;
            case 8: // Players
                GameObject.Destroy(this.gameObject);
                collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(damage);
                break;
            case 9: // Other specials
                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    public void SetValues(float damage, float lifespan) {
        this.damage = damage;
        this.lifeSpan = lifespan;
    }
}
