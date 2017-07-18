using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager2 : MonoBehaviour
{

    public static int playerHealth;
    public int maxPlayerHealth = 200;
    public Slider healthBar;

    // Use this for initialization
    void Start()
    {
        healthBar = GetComponent<Slider>();
        playerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerHealth;
    }

    public static void HurtPlayer(int damaged)
    {
        playerHealth -= damaged;
    }

}

