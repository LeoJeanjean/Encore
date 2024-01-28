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
        if(collision.gameObject.tag == "Player"){
            GameObject character =  collision.gameObject;
            if(character.GetComponent<RidiculeGaugeScript>().ridiculeJaugeAmount > 30){
                character.GetComponent<Player>().Respawn(1);
                Debug.Log("prochain niveau.");
            }else{
                character.GetComponent<Player>().Respawn(10);
                Debug.Log("pas assez marrant.");
            }
        }else{
            Debug.Log("not a player");
        }
    }
}
