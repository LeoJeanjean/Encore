
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UseSkill : MonoBehaviour
{
    public Image abilityImage;
    public Button buton;
    public float cooldown = 5;
    bool isCoolDown = false;


    private void Start()
    {
        abilityImage.fillAmount = 1;

        buton.onClick.AddListener(click);
    }

    private void click()
    {
        if (isCoolDown == false)
        {
            abilityImage.fillAmount = 1;
            isCoolDown = true;
        }
    }

    private void Update()
    {
        CoolDown();
    }



    void CoolDown()
    {
        if (isCoolDown)
        {

            abilityImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if (abilityImage.fillAmount <= 0)
            {
                abilityImage.fillAmount = 1;
                isCoolDown = false;
            }
        }
    }

}
