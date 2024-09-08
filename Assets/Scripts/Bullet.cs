using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision == null)
            return;

        if(collision.gameObject == null)
            return;

        switch(collision.gameObject.layer) {
            case 6:
                GameObject.Destroy(this.gameObject);
                break;
            case 7:
                GameObject.Destroy(this.gameObject);
                GameObject.Destroy(collision.gameObject);
                break;
            case 8:
                GameObject.Destroy(this.gameObject);
                Debug.Log("Player");
                // Deal damage
                break;
            default:
                break;
        }
    }
}
