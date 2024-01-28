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
        skill = gameManager.abilitySkills[randomIndex];

        foreach (string sprite in gameManager.abilitySkills)
        {
            Debug.Log(sprite);
        }
        abilityImage.fillAmount = 1;
        buton.onClick.AddListener(click);

        foreach (Sprite sprite in gameManager.sprites)
        {
            if (sprite.name.ToLower() == skill.ToLower())
            {
                var img = buton.GetComponent<Image>();
                img.sprite = sprite;
                break;
            }

            if (skill == "enclume" && sprite.name == "Encart_Pouvoir_Enclume")
            {
                var img = buton.GetComponent<Image>();
                img.sprite = sprite;
                break;
            }
        }
    }

    private void ResetSkill()
    {
        abilityImage.fillAmount = 1;

        buton.onClick.AddListener(click);



        int randomIndex = Random.Range(0, gameManager.abilitySkills.Count);
        skill = gameManager.abilitySkills[randomIndex];

        abilityImage.fillAmount = 1;
        buton.onClick.AddListener(click);


        foreach (Sprite sprite in gameManager.sprites)
        {
            if (sprite.name.ToLower() == skill.ToLower())
            {
                var img = buton.GetComponent<Image>();
                img.sprite = sprite;
                break;
            }
        }
    }

    private void click()
    {
        if (!isCoolDown && !gameManager.usingAbility)
        {
            abilityImage.fillAmount = 1;
            isCoolDown = true;


            if (skill.ToLower() == "enclume")
            {
                var mousePos = Input.mousePosition;
                gameManager.usingAbility = true;
                gameManager.SpawnAnvil(mousePos);
                ResetSkill();

            }

            if (skill.ToLower() == "banane")
            {
                var mousePos = Input.mousePosition;
                gameManager.usingAbility = true;
                gameManager.SpawnBanana(mousePos);
                ResetSkill();

            }
            else
            {
                gameManager.StartGlobalCoolDown();
                gameManager.ManageSkill(skill.ToLower());
                ResetSkill();

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
