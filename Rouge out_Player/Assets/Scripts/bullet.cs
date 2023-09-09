using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed=3;

    public float distance=0.5f;
    public LayerMask isLayer;
    private float lifetime = 5f;  
    private float spawnTime; 
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Player")
            {
                Debug.Log("Gun Hitttt");
            }
            DestroyBullet();
        }
        if (transform.rotation.y == 0)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right *-1* speed * Time.deltaTime);
        }
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
