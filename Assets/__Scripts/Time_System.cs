using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_System : MonoBehaviour
{
    public float totalTime;

    private float time = 0;

    private int hour;
    private int minute;
    private int second;

    public Text timeGT;
    // Start is called before the first frame update
    void Start()
    {
        GameObject scoreGO = GameObject.Find("TimeCounter");
        timeGT = scoreGO.GetComponent<Text>();
        timeGT.text = "Time: 00:00:00";
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        hour = (int)time / 3600;
        minute = ((int)time - hour * 3600) / 60;
        second = (int)time - hour * 3600 - minute * 60;

        timeGT.text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
    }


}
