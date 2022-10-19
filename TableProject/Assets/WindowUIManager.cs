using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowUIManager : MonoBehaviour
{
    public static WindowUIManager instance { get; private set; }
    public TMP_InputField inputAnswer;
    public Canvas window;

    public BaseServer server;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        server = FindObjectOfType<BaseServer>();    
        this.window.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        server = FindObjectOfType<BaseServer>();   
    }
    public void showRiddle()
    {
        this.window.gameObject.SetActive(true);
        server = FindObjectOfType<BaseServer>();  
    }

    public void OnClickAnswer()
    {
        server = FindObjectOfType<BaseServer>(); 
        Net_RiddleAnswerMsg msg = new Net_RiddleAnswerMsg(inputAnswer.text == "piou");
        BaseClient.instance.SendToServer(msg);
        this.window.gameObject.SetActive(false);
    }
}
