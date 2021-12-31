using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "testScore")
        {
            BasketballController.score += BasketballController.rewardScore;
            if (BasketballController.rewardScore == 2)
                UiController.time += 1.5f;
            else if (BasketballController.rewardScore == 3)
                UiController.time += 2.5f;
            else
                UiController.time += 3.5f;
            BasketballController.rewardScore = 0;
            BasketballController.calcScore = true;
        }
    }
}
