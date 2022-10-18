using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class healthBar : MonoBehaviour
{
    public static healthBar instance;
    Image HealthBar;
    Mouvement player;
    float fillAmount;
    int spidersNb;
    public Text dead;
    float time;
    bool died;
    public Mouvement movement;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dead.enabled = false;
        player = (Mouvement) GameObject.FindGameObjectWithTag("Player").GetComponent("Mouvement");
        spidersNb = player.spidersNb;

        HealthBar = GetComponent<Image>();
        fillAmount = HealthBar.fillAmount;
        died = false;

    }
    public void resetHealthBar()
    {
        dead.enabled = false;
        player = (Mouvement)GameObject.FindGameObjectWithTag("Player").GetComponent("Mouvement");
        spidersNb = player.spidersNb;

        HealthBar = GetComponent<Image>();
        fillAmount = HealthBar.fillAmount;
        died = false;
        movement.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        fillAmount = HealthBar.fillAmount;
        spidersNb = player.spidersNb;
        print(spidersNb);
        HealthBar.fillAmount -= 0.0005f*spidersNb;

        if (HealthBar.fillAmount == 0){
            if (!died){
                died = true;
                time = Time.time;
            }
            dead.enabled =  true;
            if (Time.time > 3 + time) {
                movement.enabled = false;
                //TODO in same scene for restarting
                //SceneManager.LoadScene("GameOver");
            }
        }
        
    }
}
