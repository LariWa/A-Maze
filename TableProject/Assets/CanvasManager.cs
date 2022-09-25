using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public TMP_InputField inputIP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BaseClient.instance.isConnected)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void OnClickConnect()
    {
        BaseClient.instance.Init(inputIP.text);
    }
}
