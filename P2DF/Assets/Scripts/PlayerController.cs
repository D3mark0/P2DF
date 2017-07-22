using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int playerNumber = 1;
    public float maxSpeed = 10;
    bool grounded = false;
    bool crunch;
    bool[] attack = new bool[7];
    float[] attackTimer = new float[7];
    float[] attackRate = new float[7];
    bool dashf;
    bool dashb;
    float dashRate;
    float dashTimer;

    public AudioSource JumpSound;
    public AudioSource DashSound;
    public AudioSource HurtSound;

    /*
     *0 punch-L
     *1 punch-H
     *2 punch-HC
     *3 kick-L
     *4 kick-LC
     *5 kick-H
     *6 kick-HC
     *7 crunch-p
     *8 crunch-k
    
    */
    Rigidbody2D rb;
    Animator anim;

    Transform enemy;

    public bool damage = false;
    public bool block = false;
    public float noBlock = 1;
    public float noCrunchBlock = 1.5f;
    public float noDamage = 1;
    public bool dead = false;
    float noDamageTimer;
    float noBlockTimer;
    float buttonCooler = 0.5f;
    int buttonCount = 0;

    public float distance;
    public float actDistance;
    float horizontal;
    float vertical;
    float groundRadius = 0.3f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float jumpForce = 100f;
    public float dashForce = 50f;



	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject pl in players)
        {
            if(pl.transform != this.transform)
            {
                enemy = pl.transform;
            }
        }
	}

    void Update()
    {
        //jump
        if (grounded && vertical > 0.1f)
        {
            anim.SetBool("grounded", false);
            rb.AddForce(new Vector2(0, jumpForce));
            JumpSound.Play();

        }

        Flip();
        Damaged();
        Block();
        CheckDistance();
    }

    void Flip()
    {
        if (transform.position.x > enemy.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = Vector3.one;
    }

    void Damaged()
    {
        if(damage)
        {
            //HurtSound.playOnAwake();
            vertical = 0;
            //rb.AddForce(new Vector2(hitForce, 0));
            noDamageTimer += Time.deltaTime;
            if(noDamageTimer > noDamage)
            {
                damage = false;
                noDamageTimer = 0;
            }
        }
    }
    void Block()
    {
        if (block)
        {
            vertical = 0;
            rb.velocity = Vector3.zero;
            noBlockTimer += Time.deltaTime;
            if (noBlockTimer > noCrunchBlock)
            {
                block = false;     
                noBlockTimer = 0;
            }
        }
        if(distance < actDistance)
        {
            foreach(bool at in enemy.transform.root.GetComponent<PlayerController>().attack)
            {
                if(at && Input.GetAxis("Horizontal" + playerNumber.ToString())<0 && playerNumber == 1)
                {
                    block = true;
                }
                else if(at && Input.GetAxis("Horizontal" + playerNumber.ToString())>0 && playerNumber == 2)
                {
                    block = true;
                }
            }
        }
    }

    void CheckDistance()
    {
        distance = Mathf.Abs(transform.position.x - enemy.position.x);
        anim.SetFloat("distance", distance);

    }

	// Update is called once per frame
	void FixedUpdate () {

        //check grounded?
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);

        if (!grounded) return;
        //get horizontal and vertical
        horizontal = Input.GetAxis("Horizontal" + playerNumber.ToString());
        vertical = Input.GetAxis("Vertical" + playerNumber.ToString());

        //crunch or not?
        crunch = (vertical < -0.1f);
        anim.SetBool("crunch", crunch);
        if (!crunch)
            rb.velocity = new Vector2(horizontal * maxSpeed, rb.velocity.y);
        else
        {
            rb.velocity = Vector3.zero;
            if (block)
            {
                vertical = 0;
                rb.velocity = Vector3.zero;
                noBlockTimer += Time.deltaTime;
                if (noBlockTimer > noBlock-0.5)
                {
                    block = false;
                    noBlockTimer = 0;
                }
            }
            if (distance < actDistance)
            {
                foreach (bool at in enemy.transform.root.GetComponent<PlayerController>().attack)
                {
                    if (at && Input.GetAxis("Horizontal" + playerNumber.ToString()) < 0)
                    {
                        block = true;

                    }
                }
            }
        }

  
        anim.SetFloat("horizontal", horizontal);

        if (Input.GetButtonDown("Horizontal" + playerNumber.ToString()))
        {
            if(horizontal > 0 && transform.name == "Player1")
            {
                if (buttonCooler > 0 && buttonCount == 1)
                {
                    dashRate = 0.4f;
                    dashTimer = 0;
                    dashf = true;
                    anim.SetBool("dash-F", dashf);
                }
                else
                {
                    buttonCooler = 0.5f;
                    buttonCount += 1;
                } 
            }
            else if (horizontal < 0 && transform.name == "Player1")
            {
                if (buttonCooler > 0 && buttonCount == 1)
                {
                    dashRate = 0.4f;
                    dashTimer = 0;
                    dashb = true;
                    anim.SetBool("dash-B", dashb);
                }
                else
                {
                    buttonCooler = 0.5f;
                    buttonCount += 1;
                } 
            }
            else if(horizontal < 0 && transform.name == "Player2")
            {
                if (buttonCooler > 0 && buttonCount == 1)
                {
                    dashRate = 0.4f;
                    dashTimer = 0;
                    dashf = true;
                    anim.SetBool("dash-F", dashf);
                }
                else
                {
                    buttonCooler = 0.5f;
                    buttonCount += 1;
                } 
            }
            else if(horizontal > 0 && transform.name == "Player2")
            {
                if (buttonCooler > 0 && buttonCount == 1)
                {
                    dashRate = 0.4f;
                    dashTimer = 0;
                    dashb = true;
                    anim.SetBool("dash-B", dashb);
                }
                else
                {
                    buttonCooler = 0.5f;
                    buttonCount += 1;
                } 
            }
            
        }
        if(buttonCooler > 0)
        {
            buttonCooler -= Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }
        if(dashf)
        {
            if(playerNumber == 1)
                rb.AddForce(new Vector2(dashForce, 0));
            else
                rb.AddForce(new Vector2(-dashForce, 0));
            dashTimer += Time.deltaTime;
            if(dashTimer > dashRate)
            {
                dashTimer = 0;
                dashf = false;
                anim.SetBool("dash-F", dashf);
                rb.velocity = Vector3.zero;
            }
        }
        if(dashb)
        {
            if (playerNumber == 1)
                rb.AddForce(new Vector2(-dashForce, 0));
            else
                rb.AddForce(new Vector2(dashForce, 0));
            dashTimer += Time.deltaTime;
            if (dashTimer > dashRate)
            {
                dashTimer = 0;
                dashb = false;
                anim.SetBool("dash-B", dashb);
                rb.velocity = Vector3.zero;
            }
        }
        //attack
        if(Input.GetButton("Punch-L" + playerNumber.ToString()))
        {
            attackRate[0] = 0.3f;
            attack[0] = true;
            attackTimer[0] = 0;
            anim.SetBool("punch-L", attack[0]);
        }
        if(attack[0])
        {
            vertical = 0;
            rb.velocity = Vector3.zero;
            attackTimer[0] += Time.deltaTime;

            if(attackTimer[0] > attackRate[0])
            {
                attackTimer[0] = 0;
                attack[0] = false;
                anim.SetBool("punch-L", attack[0]);
            }
        }

        if (Input.GetButton("Punch-H" + playerNumber.ToString()))
        {
            attack[1] = true;
            attackTimer[1] = 0;

                attackRate[1] = 0.6f;
                anim.SetBool("punch-H", attack[1]);

            
        }
        if (attack[1])
        {
            vertical = 0;
            rb.velocity = Vector3.zero;
            attackTimer[1] += Time.deltaTime;

            if (attackTimer[1] > attackRate[1])
            {
                attackTimer[1] = 0;
                attack[1] = false;
                anim.SetBool("punch-H", attack[1]);
                anim.SetBool("punch-HC", attack[1]);
            }
        }

        if (Input.GetButton("Kick-L" + playerNumber.ToString()))
        {
            attack[3] = true;
            attackTimer[3] = 0;
                attackRate[3] = 0.4f;
                anim.SetBool("kick-L", attack[3]);

        }
        if (attack[3])
        {
            vertical = 0;
            rb.velocity = Vector3.zero;
            attackTimer[3] += Time.deltaTime;

            if (attackTimer[3] > attackRate[3])
            {
                attackTimer[3] = 0;
                attack[3] = false;
                anim.SetBool("kick-L", attack[3]);
                anim.SetBool("kick-LC", attack[3]);
            }
        }

        if (Input.GetButton("Kick-H" + playerNumber.ToString()))
        {
            attack[5] = true;
            attackTimer[5] = 0;

                attackRate[5] = 0.4f;
                anim.SetBool("kick-H", attack[5]);


        }
        if (attack[5])
        {
            vertical = 0;
            rb.velocity = Vector3.zero;
            attackTimer[5] += Time.deltaTime;

            if (attackTimer[5] > attackRate[5])
            {
                attackTimer[5] = 0;
                attack[5] = false;
                anim.SetBool("kick-H", attack[5]);
                anim.SetBool("kick-HC", attack[5]);
            }
        }
	}
}
