using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaivor : MonoBehaviour {

    AudioSource audioSource;
    /*Sounds Behaivor*/
    public AudioClip audioBeingHeal;
    public AudioClip beingDamage;
    public AudioClip immuneDamage;
    public AudioClip destroyedEnemy;

    /*PLAYER*/
    public int maxPlayerLifePoints = 5;
    public int playerLifePoints = 5;    
    public int playerPoints = 0;
    public float playerSpeed = 1;

    public List<int> PowerUpList;
    public List<Text> TextList;
    
    // BulletList
    public List<GameObject> bulletList = new List<GameObject>();
    int bulletIndex = 0;

    // Private Pamameters
    List<Axis> axisList = new List<Axis>();

    // Ship Color
    public SpriteRenderer spriteRenderer;

    private Transform bs1 = null;
    private Transform bs2 = null;

    private Transform scenter = null;

    private Transform sd1 = null;
    private Transform sd2 = null;
    
    // Other Parameters
    public Transform sightObject;

    public LineRenderer sightLine;
    Vector3 mouseWorldPos;

    public Text lifeText;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        axisList.Add(new Axis("Horizontal", KeyCode.A, KeyCode.D));
        axisList.Add(new Axis("Vertical", KeyCode.S, KeyCode.W));
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * GetAxis("Horizontal") * playerSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * GetAxis("Vertical") * playerSpeed * Time.deltaTime, Space.World);
        
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        //transform.up = (mouseWorldPos - transform.position).normalized;/*new*/

        /*if (Vector3.Distance(mouseWorldPos, transform.position) >= 1)
        {
            sightCursor.position = mouseWorldPos;
        }
        else
        {
            sightCursor.position = transform.position + sightObject.up;
        }*/

        sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
        sd2 = transform.Find("SightDirection2").GetComponent<Transform>();

        sd1.up = (mouseWorldPos - transform.position).normalized;
        sd2.up = (mouseWorldPos - transform.position).normalized;

        sightLine.SetPositions(new Vector3[] { transform.position, transform.position + sd1.up * 3 });
        sightLine.SetPositions(new Vector3[] { transform.position, transform.position + sd2.up * 3 });
        
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (maxPlayerLifePoints > playerLifePoints)
            {
                DecreasePowerUp(PowerUpParameters.HEAL);
                gameObject.GetComponent<AudioSource>().PlayOneShot(audioBeingHeal);
                playerLifePoints +=1;
            }
        } else if(Input.GetKeyDown(KeyCode.X)){
            if(PowerUpList[PowerUpParameters.BOMB]>0){
                DecreasePowerUp(PowerUpParameters.BOMB);
                ShootMisile();
            }
        }

        lifeText.text = "Life: " + (playerLifePoints < 0 ? 0 : playerLifePoints).ToString();
    }

    void LateUpdate()
    {
        Transform sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
        Transform sd2 = transform.Find("SightDirection2").GetComponent<Transform>();

        sightObject.position = (Vector3.Distance(mouseWorldPos, transform.position) >= 1) ? mouseWorldPos : transform.position + sd1.up;
        sightObject.position = (Vector3.Distance(mouseWorldPos, transform.position) >= 1) ? mouseWorldPos : transform.position + sd2.up;


    }

    void Shoot()
    {
        sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
        sd2 = transform.Find("SightDirection2").GetComponent<Transform>();

        Destroy(Instantiate(bulletList[bulletIndex], sd1.Find("Cannon").position, sd1.rotation), 1.5f);
        Destroy(Instantiate(bulletList[bulletIndex], sd2.Find("Cannon").position, sd2.rotation), 1.5f);

        bs1 = transform.Find("BasicShooter1").GetComponent<Transform>();
        Destroy(Instantiate(bulletList[bulletIndex], bs1.Find("Cannon").position, bs1.rotation), 1.5f);
        bs2 = transform.Find("BasicShooter2").GetComponent<Transform>();
        Destroy(Instantiate(bulletList[bulletIndex], bs2.Find("Cannon").position, bs2.rotation), 1.5f);

        //CamMovement cam = Camera.main.GetComponent<CamMovement>();
        //cam.speed = 25;
        //cam.impulseDirection = sightDirection.up;
    }

    void ShootMisile()
    {
        scenter = transform.Find("SightCenter").GetComponent<Transform>();

        Destroy(Instantiate(bulletList[3], scenter.Find("Cannon").position, scenter.rotation), 1f);

        //CamMovement cam = Camera.main.GetComponent<CamMovement>();
        //cam.speed = 25;
        //cam.impulseDirection = sightDirection.up;
    }

    public int GetAxis(string axisName)
    {
        if(axisList!=null && axisList.Count != 0)
        {
            Axis axis = axisList.Find(target => target.name == axisName);
            int axisValue = 0;
            if (Input.GetKey(axis.negative))
            {
                axisValue += -1;
            }
            if (Input.GetKey(axis.positive))
            {
                axisValue += 1;
            }
            return axisValue;
        }
        return 0;
    }

    public int getLifePoints()
    {
        return playerLifePoints;
    }

    public GameObject gameOver;
    public GameController gameController;

    public void DecreaseLife(int damage)
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (damage == 0)
        {
            audioSource.PlayOneShot(immuneDamage);
        }
        else
        {
            audioSource.PlayOneShot(beingDamage);
            playerLifePoints -= damage;
            
            if (playerLifePoints <= 0)
            {
                gameController.StopSpawn();
                gameOver.SetActive(true);
                Destroy(gameObject, 0.1f);
            }
        }
    }
    
    public void IncreasePowerUp(int powerUpType)
    {
        PowerUpList[powerUpType] += 1;
        TextList[powerUpType].text = PowerUpList[powerUpType].ToString();
    }

    public void DecreasePowerUp(int powerUpType)
    {
        PowerUpList[powerUpType] -= 1;
        TextList[powerUpType].text = PowerUpList[powerUpType].ToString();
    }

    GameObject currentWall;
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("Wall"))
        {
            currentWall = collision.gameObject;
        }*/
    }
}
