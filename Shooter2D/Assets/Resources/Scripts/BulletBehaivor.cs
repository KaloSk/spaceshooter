using System.Collections.Generic;
using UnityEngine;

public class BulletBehaivor : MonoBehaviour {
    
    public int bulletSpeed = 1;
    SpriteRenderer bulletSprite;
    
    /*Sounds Behaivor*/
    public AudioClip bulletSound;
    public AudioClip bulletExplode;
    public int bulletDamage = 1;
    public int bulletType = 1;

    public GameObject bulletExplosion;

    //Current AudioSource
    AudioSource audioSource;
    
    // Use this for initialization
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(bulletSound);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if(!transform.gameObject.CompareTag("EnemyBullet")){
            if (otherRenderer != null && other.gameObject.CompareTag("SimpleEnemy"))
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<EnemyBehaivor>().DecreaseLife(bulletDamage);
                audioSource = transform.gameObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(bulletExplode);
                Destroy(transform.gameObject);
                if (bulletType == 3)
                {
                    Destroy(Instantiate(bulletExplosion, transform.position, transform.rotation), 3f);
                }
            }
        } 

        if(transform.gameObject.CompareTag("EnemyBullet")){
            if (otherRenderer != null && other.gameObject.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                //player.GetComponent<AudioSource>().PlayOneShot(destroyedEnemy);
                player.GetComponent<PlayerBehaivor>().DecreaseLife(bulletDamage);
                Destroy(gameObject);
            }
        }

    }
}
