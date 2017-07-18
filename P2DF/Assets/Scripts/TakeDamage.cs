using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour {

    public int damage;
    public string title;

    public void BackToTitle()
    {
        Application.LoadLevel(title);
    }
	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.root.GetComponent<PlayerController>().block)
        {
            col.transform.root.GetComponentInChildren<Animator>().SetTrigger("block");
        }
        else
        {
            if (col.transform.root != transform.root && col.tag != "Ground" && !col.isTrigger)
            {
                col.transform.root.GetComponent<PlayerController>().damage = true;
                col.transform.root.GetComponentInChildren<Animator>().SetTrigger("damaged");
                if (col.transform.root.GetComponent<PlayerController>().playerNumber == 1)
                {
                    HealthManager1.HurtPlayer(damage);
                    if(HealthManager1.playerHealth <= 0)
                    {
                        col.transform.root.GetComponent<PlayerController>().dead = true;
                        col.transform.root.GetComponentInChildren<Animator>().SetBool("dead", true);
                        Invoke("BackToTitle", 2f);
                       // col.transform.root.GetComponent<PlayerController>().HurtSound.Play();
                    }
                }
                else
                {
                    HealthManager2.HurtPlayer(damage);
                    if (HealthManager2.playerHealth <= 0)
                    {
                        col.transform.root.GetComponent<PlayerController>().dead = true;
                        col.transform.root.GetComponentInChildren<Animator>().SetBool("dead", true);
                        Invoke("BackToTitle", 2f);
                       // col.transform.root.GetComponent<PlayerController>().HurtSound.Play();
                    }
                }

            }
        }
        
    }
}
