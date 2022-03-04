using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public GameObject message;
    public GameObject attention;
    public int changeColor = 0;

    private int timer;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = GameObject.Find("_MainCamera").GetComponent<Time_System>().time;
        if (time > 5 && time < 6)
        {
            message.SetActive(false);
        }
        else if (time > 170 && time <= 175)
        {
            timer = (int)time;
            attention.SetActive(true);
            ChangeColor();
        }
        else if (time > 175)
        {
            attention.SetActive(false);
        }
    }

    void ChangeColor()
    {
        if (timer % 2 == 1)
        {
            attention.GetComponent<Text>().color = Color.white;
        }
        else
        {
            attention.GetComponent<Text>().color = Color.red    ;
        }
    }
}
