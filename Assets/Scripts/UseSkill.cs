
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UseSkill : MonoBehaviour
{

    public GameManager gameManager;
    public Image abilityImage;
    public Button buton;
    public float cooldown = 5;
    bool isCoolDown = false;
    public string skill;



    private void Start()
    {
        abilityImage.fillAmount = 1;

        buton.onClick.AddListener(click);


        int randomIndex = Random.Range(0, gameManager.abilitySkills.Count);
        // skill = gameManager.abilitySkills[randomIndex];
    }

    private void click()
    {
        if (!isCoolDown && !gameManager.usingAbility)
        {
            abilityImage.fillAmount = 1;
            isCoolDown = true;


            /*
              { "enclume", "gravite", "rideaux", "pouete", "blague", "chaussuresGlissantes",
                        "accelerations", "ralentissement", "picInvisible", "inversionCommandes", "terrainFolie", 
                        //"sansDessusDessous" ,
                        "seisme", "chaussuresCollantes", "pointColle", "pointGlace", "banana"
                        };
            */

            if (skill == "enclume")
            {
                var mousePos = Input.mousePosition;
                gameManager.usingAbility = true;
                gameManager.SpawnAnvil(mousePos);
            }
            else if (skill == "gravite")
            {
                gameManager.StartGlobalCoolDown();
                gameManager.ManageSkill(skill);
            }
            else if (skill == "chaussuresGlissantes")
            {
                gameManager.StartGlobalCoolDown();
                gameManager.ManageSkill(skill);
            }
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
        else if (gameManager.usingAbility)
        {
            abilityImage.fillAmount = 0;
        }
        else if (gameManager.abilityCoolDown)
        {
            abilityImage.fillAmount = gameManager.abilityCoolDownOpacity;
        }
        else
        {
            abilityImage.fillAmount = 1;
        }
    }

}
