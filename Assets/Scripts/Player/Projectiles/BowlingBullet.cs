using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBullet : MonoBehaviour {
    public float lifeSpan;
    private float timer;

    // Start is called before the first frame update
    void Start() {
        timer = 0.0f;
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
                Debug.Log("Player");
                // Deal damage
                break;
            case 9: // Other specials
                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
