using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaivor : MonoBehaviour {

    public int EnemyCrushDamage = 1;

    public List<GameObject> EnemyPrize;

    /*Sounds Behaivor*/
    public AudioClip beingDamage;
    public AudioClip immuneDamage;
    public AudioClip destroyedEnemy;

    //Current AudioSource
    AudioSource audioSource;

    public int enemyLifePoints = 5;

    //Explosion
    public GameObject explosion;

    // Use this for initialization
    void Start () {
		
	}
	
    private int nextUpdate = 1;

	// Update is called once per frame
	void Update () {

        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            enemyShoot();
        }


    }

    public GameObject enemyBullet;

    void enemyShoot(){
        Transform sd = transform.Find("SightDirection").GetComponent<Transform>();
        Destroy(Instantiate(enemyBullet, sd.Find("Cannon").position, sd.rotation), 1f);
    }
    
    public void DecreaseLife(int damage)
    {
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        if (damage == 0)
        {
            audioSource.PlayOneShot(immuneDamage);
        }
        else
        {
            audioSource.PlayOneShot(beingDamage);
            enemyLifePoints -= damage;
            if (enemyLifePoints <= 0)
            {
                audioSource.PlayOneShot(destroyedEnemy);
                int c = Random.Range(-1, 2);
                if (c != -1) {
                    if (Random.Range(0, 100) < 35) {
                        Instantiate(EnemyPrize[c], transform.position, transform.rotation);
                    }
                }
                ExplodeEnemy();
                Camera.main.GetComponent<CanvasBehaivor>().score+=1;
            }
        }
    }

    void LateUpdate()
    {
        if (enemyLifePoints <= 0) {
            Destroy(gameObject);
        }
    }

    public int getLifePoints()
    {
        return enemyLifePoints;
    }

    public void ExplodeEnemy()
    {
        Destroy(Instantiate(explosion, transform.position, transform.rotation), 0.1f);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null && other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            player.GetComponent<AudioSource>().PlayOneShot(destroyedEnemy);
            player.GetComponent<PlayerBehaivor>().DecreaseLife(EnemyCrushDamage);
            DecreaseLife(enemyLifePoints);
        }
    }

}
