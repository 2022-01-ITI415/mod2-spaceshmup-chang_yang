using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_System : MonoBehaviour
{
    public float totalTime;
    public float levelTimeInterval = 30;
    public float level_1_ESPS = 0.5f;
    public float level_2_ESPS = 1f;
    public float level_3_ESPS = 1.5f;
    public float level_4_ESPS = 2f;

    private float time = 0;
    private int level = 0;

    private int hour;
    private int minute;
    private int second;

    private Text timeGT;
    private Text levelGT;
    // Start is called before the first frame update
    void Start()
    {
        GameObject scoreGO = GameObject.Find("TimeCounter");
        timeGT = scoreGO.GetComponent<Text>();
        timeGT.text = "Time: 00:00:00";

        GameObject levelGO = GameObject.Find("LevelCounter");
        levelGT = levelGO.GetComponent<Text>();
        levelGT.text = "Level: 0/5";
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;

        hour = (int)time / 3600;
        minute = ((int)time - hour * 3600) / 60;
        second = (int)time - hour * 3600 - minute * 60;

        timeGT.text = "Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        levelGT.text = "Level: " + level.ToString() + "/5";

        if (time > levelTimeInterval && time <= (levelTimeInterval * 2))
        {
            Main.S.enemySpawnPerSecond = level_1_ESPS;
            level = 1;
        }
        else if (time > (levelTimeInterval * 2) && time <= (levelTimeInterval * 3))
        {
            Main.S.enemySpawnPerSecond = level_2_ESPS;
            level = 2;
        }
        else if (time > (levelTimeInterval * 3) && time <= (levelTimeInterval * 4))
        {
            Main.S.enemySpawnPerSecond = level_3_ESPS;
            level = 3;
        }
        else if (time > (levelTimeInterval * 4) && time <= (levelTimeInterval * 5))
        {
            Main.S.enemySpawnPerSecond = level_4_ESPS;
            level = 4;
        }
        else if (time > (levelTimeInterval * 5))
        {
            Main.S.health = 10;
            level = 5;
        }
    }


}
