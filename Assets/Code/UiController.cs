using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    public float time;
    public static bool isGameOver;
    Text remainTime;
    
    private void Awake()
    {
        time = 10f;
        isGameOver = false;
        remainTime = GameObject.Find("remainTime").GetComponent<Text>();
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
        remainTime.text = "倒计时:" + t + "秒";
    }
}
