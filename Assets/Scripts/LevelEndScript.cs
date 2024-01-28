using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelEndScript : MonoBehaviour
{
    private Collider2D end;
    
    void Awake (){
        end = transform.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        GameObject character =  collision.gameObject;
        if(character.GetComponent<RidiculeGaugeScript>().ridiculeJaugeAmount > 30){
            character.GetComponent<Player>().Respawn(1);
        }else{
            //switch player roles here
        }
    }
}
