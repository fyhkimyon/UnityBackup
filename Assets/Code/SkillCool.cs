using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCool : MonoBehaviour
{
    [SerializeField] public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (BasketballController.isCooling)
        {
            if (image.fillAmount == 1)
            {
                image.fillAmount = 0;
            }
            image.fillAmount += Time.fixedDeltaTime / 20.0f;
            if (image.fillAmount >= 1)
            {
                image.fillAmount = 1.0f;
                BasketballController.isCooling = false;
            }
        }
    }
}
