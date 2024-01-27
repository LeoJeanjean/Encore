using TMPro;
using UnityEngine;

public class RidiculeGaugeScript : MonoBehaviour
{
    [HideInInspector] public float ridiculeJaugeAmount = 0;
    public TextMeshProUGUI ridiculeJaugeText;
    public TextMeshProUGUI hilarityMultiplierText;

    private float invincibility = 1f;
    private float hilarityMultiplier = 1f; // each damage taken makes the next hit funnier and more damaging
    private float hilarityResetTimerMax = 3f; //
    private float hilarityResetTimer;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        hilarityResetTimer = 0;
    }

    void Update(){
        if(!isDead){
            if(hilarityResetTimer > 0) hilarityResetTimer -= Time.deltaTime;
            else if(hilarityMultiplier > 1) { 
                hilarityMultiplier -= 1f; 
                hilarityResetTimer = hilarityResetTimerMax;
            }else hilarityResetTimer = 0;

            if(ridiculeJaugeAmount > 0) ridiculeJaugeAmount -= Time.deltaTime;

            if(invincibility > 0) invincibility -= Time.deltaTime;

            ridiculeJaugeText.SetText("Ridicule: "+Mathf.FloorToInt(ridiculeJaugeAmount));
            hilarityMultiplierText.SetText("Multiplicateur: x"+hilarityMultiplier);
        }
    }

    public void TakeDamage(){
        if(invincibility <= 0 && !isDead){
            ridiculeJaugeAmount += 10*hilarityMultiplier;
            hilarityMultiplier += 1;
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
