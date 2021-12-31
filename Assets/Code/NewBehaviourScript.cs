using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    Text YourScore;
    // Start is called before the first frame update
    void Start()
    {
        YourScore = GameObject.Find("YourScore").GetComponent<Text>();
        YourScore.text = "您的分数为:" + BasketballController.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
