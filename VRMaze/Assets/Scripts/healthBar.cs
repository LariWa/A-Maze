using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class healthBar : MonoBehaviour
{
    Image HealthBar;
    Mouvement player;
    float fillAmount;
    int spidersNb;
    public Text dead;
    float time;
    bool died;

    // Start is called before the first frame update
    void Start()
    {
        dead.enabled = false;
        player = (Mouvement) GameObject.FindGameObjectWithTag("Player").GetComponent("Mouvement");
        spidersNb = player.spidersNb;

        HealthBar = GetComponent<Image>();
        fillAmount = HealthBar.fillAmount;
        died = false;

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

                SceneManager.LoadScene("GameOver");
            }
        }
        
    }
}
