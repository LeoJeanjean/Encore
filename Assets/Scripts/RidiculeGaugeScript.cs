using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RidiculeGaugeScript : MonoBehaviour
{
    private float ridiculeJaugeAmount = 35f;
    public TextMeshProUGUI ridiculeJaugeText;
    public TextMeshProUGUI hilarityMultiplierText;

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
    }

    void Update(){
        if(!isDead){
            if(hilarityResetTimer > 0) hilarityResetTimer -= Time.deltaTime;
            else if(hilarityMultiplier > 1) { 
                hilarityMultiplier -= 0.5f; 
                hilarityResetTimer = hilarityResetTimerMax;
            }else hilarityResetTimer = 0;

            if(ridiculeJaugeAmount > 0) ridiculeJaugeAmount -= Time.deltaTime;

            if(invincibility > 0) invincibility -= Time.deltaTime;

            ridiculeJaugeText.SetText("Ridicule: "+Mathf.FloorToInt(ridiculeJaugeAmount));
            hilarityMultiplierText.SetText("x"+hilarityMultiplier);

            switch (ridiculeJaugeAmount)
            {
                case <10:
                    Debug.Log("sous 10");
                    crowdUIImage.sprite = crowdLevels[0];
                    break;
                case float n when (n >= 10 && n < 20):
                    crowdUIImage.sprite = crowdLevels[1];
                    Debug.Log("Entre 10 et 19");
                    break;
                case float n when (n >= 20 && n < 30):
                    crowdUIImage.sprite = crowdLevels[2];
                    Debug.Log("Entre 20 et 29");
                    break;
                case float n when (n >= 30 && n < 40):
                    crowdUIImage.sprite = crowdLevels[3];
                    Debug.Log("Entre 30 et 39");
                    break;
                case float n when (n >= 40 && n < 50):
                    crowdUIImage.sprite = crowdLevels[3];
                    Debug.Log("Entre 40 et 49");
                    break;
                case float n when (n >= 50 && n < 60):
                    crowdUIImage.sprite = crowdLevels[4];
                    Debug.Log("Entre 50 et 59");
                    break;
                case float n when (n >= 60 && n < 70):
                    crowdUIImage.sprite = crowdLevels[5];
                    Debug.Log("Entre 60 et 69");
                    break;
                case float n when (n >= 70 && n < 80):
                    crowdUIImage.sprite = crowdLevels[6];
                    Debug.Log("Entre 70 et 79");
                    break;
                case float n when (n >= 80 && n < 90):
                    crowdUIImage.sprite = crowdLevels[7];
                    Debug.Log("Entre 80 et 89");
                    break;
                case float n when (n >= 90 && n < 100):
                    crowdUIImage.sprite = crowdLevels[7];
                    Debug.Log("Entre 90 et 99");
                    break;
                default:
                    crowdUIImage.sprite = crowdLevels[8];
                    Debug.Log("égal à/plus de 100");
                    break;
            }
        }
    }

    public void TakeDamage(){
        if(invincibility <= 0 && !isDead){
            ridiculeJaugeAmount += 10*hilarityMultiplier;
            hilarityMultiplier += 0.5f;
            hilarityResetTimer = hilarityResetTimerMax;
            if(ridiculeJaugeAmount >= 100) RidiculeDeath();
            invincibility = 1f;
        }
    }

    void RidiculeDeath(){
        Debug.Log("DEAD!");
        isDead = true;
    }
}
