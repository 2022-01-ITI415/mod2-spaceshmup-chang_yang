using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static public int score = 100;
    public Text gt;
    void Start()
    {
        
    }

    void Awake(){
        if (PlayerPrefs.HasKey("HighScore")){
            score = PlayerPrefs.GetInt("HighScore");
            PlayerPrefs.SetInt("HighScore",score);
        }
    }
    void Update()
    {
        gt = this.GetComponent <Text>();
        gt.text = "High Score: " +score;
        if (score > PlayerPrefs.GetInt("HighScore")){
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
