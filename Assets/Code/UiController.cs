using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public static float time;
    public static bool isGameOver;
    Text remainTime;
    Text Score;
    public static Text skills;
    public static Image bar;
    
    private void Awake()
    {
        time = 30f;
        isGameOver = false;
        remainTime = GameObject.Find("remainTime").GetComponent<Text>();
        Score = GameObject.Find("Score").GetComponent<Text>();
        skills = GameObject.Find("skills").GetComponent<Text>();
        bar = GameObject.Find("bar").GetComponent<Image>();
        bar.fillAmount = 0f;
    }

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        int t = Mathf.FloorToInt(time);
        if (t < 0)
        {
            isGameOver = true;
            SceneManager.LoadScene("MainScene");
            return;
        }
        if (BasketballController.calcScore)
        {
            Score.text = "分数:" + BasketballController.score;
            BasketballController.calcScore = false;
        }
        remainTime.text = "倒计时:" + t + "秒";
    }
}
