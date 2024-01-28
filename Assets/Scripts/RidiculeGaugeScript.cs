using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class RidiculeGaugeScript : MonoBehaviour
{
    [HideInInspector] public float ridiculeJaugeAmount = 35f;
    //public TextMeshProUGUI ridiculeJaugeText;
    public TextMeshProUGUI hilarityMultiplierText;

    public GameManager gameManager;

    private Image crowdUIImage;
    public List<Sprite> crowdLevels;

    private float invincibility = 1f;
    private float hilarityMultiplier = 1f; // each damage taken makes the next hit funnier and more damaging
    private float hilarityResetTimerMax = 3f; //
    private float hilarityResetTimer;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        hilarityResetTimer = 0;

        crowdUIImage = GameObject.FindWithTag("Crowd").GetComponent<Image>();

        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (!isDead)
        {
            if (hilarityResetTimer > 0) hilarityResetTimer -= Time.deltaTime;
            else if (hilarityMultiplier > 1)
            {
                hilarityMultiplier -= 0.5f;
                hilarityResetTimer = hilarityResetTimerMax;
            }
            else hilarityResetTimer = 0;

            if (ridiculeJaugeAmount > 0) ridiculeJaugeAmount -= Time.deltaTime;

            if (invincibility > 0) invincibility -= Time.deltaTime;

            //ridiculeJaugeText.SetText("Ridicule: "+Mathf.FloorToInt(ridiculeJaugeAmount));
            hilarityMultiplierText.SetText("x" + hilarityMultiplier);

            switch (ridiculeJaugeAmount)
            {
                case < 10:
                    crowdUIImage.sprite = crowdLevels[0];
                    break;
                case float n when (n >= 10 && n < 20):
                    crowdUIImage.sprite = crowdLevels[1];
                    break;
                case float n when (n >= 20 && n < 30):
                    crowdUIImage.sprite = crowdLevels[2];
                    break;
                case float n when (n >= 30 && n < 40):
                    crowdUIImage.sprite = crowdLevels[3];
                    break;
                case float n when (n >= 40 && n < 50):
                    crowdUIImage.sprite = crowdLevels[3];
                    break;
                case float n when (n >= 50 && n < 60):
                    crowdUIImage.sprite = crowdLevels[4];
                    break;
                case float n when (n >= 60 && n < 70):
                    crowdUIImage.sprite = crowdLevels[5];
                    break;
                case float n when (n >= 70 && n < 80):
                    crowdUIImage.sprite = crowdLevels[6];
                    break;
                case float n when (n >= 80 && n < 90):
                    crowdUIImage.sprite = crowdLevels[7];
                    break;
                case float n when (n >= 90 && n < 100):
                    crowdUIImage.sprite = crowdLevels[7];
                    break;
                default:
                    crowdUIImage.sprite = crowdLevels[8];
                    break;
            }
        }
    }

    public void TakeDamage()
    {
        if (invincibility <= 0 && !isDead)
        {
            ridiculeJaugeAmount += 10 * hilarityMultiplier;


            switch (ridiculeJaugeAmount)
            {
                case < 10:
                    gameManager.playSound("Nul4");
                    break;
                case float n when (n >= 10 && n < 20):
                    gameManager.playSound("Nul3");
                    break;
                case float n when (n >= 20 && n < 30):
                    gameManager.playSound("Nul3");
                    break;
                case float n when (n >= 30 && n < 40):
                    gameManager.playSound("Rire2");
                    break;
                case float n when (n >= 40 && n < 50):
                    gameManager.playSound("Rire1");
                    break;
                case float n when (n >= 50 && n < 60):
                    gameManager.playSound("Rire1");
                    break;
                case float n when (n >= 60 && n < 70):
                    gameManager.playSound("Rire2");
                    break;
                case float n when (n >= 70 && n < 80):
                    gameManager.playSound("Rire2");
                    break;
                case float n when (n >= 80 && n < 90):
                    gameManager.playSound("Rire3");
                    break;
                case float n when (n >= 90 && n < 100):
                    gameManager.playSound("Rire3");
                    break;
                default:
                    gameManager.playSound("Rire4");
                    break;
            }

            hilarityMultiplier += 0.5f;
            hilarityResetTimer = hilarityResetTimerMax;
            if (ridiculeJaugeAmount >= 100) RidiculeDeath();
            invincibility = 1f;
        }
    }

    void RidiculeDeath()
    {
        Debug.Log("DEAD!");
        isDead = true;
        GameObject.FindWithTag("Player").GetComponent<Player>().Respawn(0);
    }
}
