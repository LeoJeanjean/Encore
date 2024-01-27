using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PhotonView photonView;

    public GameObject RunnerPrefab;
    public GameObject ShooterCanvas;

    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public TMP_Text PingText;

    public GameObject anvilPrefab;

    public int target = 30;

    public float abilityCoolDownLenght = 1.5f;

    public float abilityCoolDownOpacity = 0f;

    public bool abilityCoolDown = false;

    public bool usingAbility = false;

    public List<string> abilitySkills = new List<string>
            { "enclume", "gravite", "rideaux", "pouete", "blague", "chaussuresGlissantes",
            "accelerations", "ralentissement", "picInvisible", "inversionCommandes", "terrainFolie", 
            //"sansDessusDessous" ,
            "seisme", "chaussuresCollantes", "pointColle", "pointGlace", "banana"
            };


    private Player player;

    private void Update()
    {
        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
        PingText.text = "Ping : " + PhotonNetwork.GetPing();

        if (abilityCoolDown)
        {
            if (abilityCoolDownOpacity <= 0)
            {
                abilityCoolDownOpacity = 1f;
                abilityCoolDown = false;
            }
            else
            {
                abilityCoolDownOpacity -= 1 / abilityCoolDownLenght * Time.deltaTime;
            }
        }

    }



    public void StartGlobalCoolDown()
    {
        abilityCoolDown = true;
        abilityCoolDownOpacity = 1f;
    }


    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
        GameCanvas.SetActive(true);
        var runner = PhotonNetwork.playerName.Split("/")[1];
        Debug.Log(PhotonNetwork.playerName);
        Debug.Log(runner);
        if (runner == "true")
        {
            SpawnPlayer();
        }
        else
        {
            ShooterCanvas.SetActive(true);
            SceneCamera.SetActive(false);
        }
    }

    public void SpawnPlayer()
    {

        float randomValue = Random.RandomRange(-1f, 1f);

        var playerGameObject = PhotonNetwork.Instantiate(RunnerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);

        player = playerGameObject.GetComponent<Player>();
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }


    public void SpawnAnvil(Vector3 mousePos)
    {
        PhotonNetwork.Instantiate(anvilPrefab.name, mousePos, Quaternion.identity, 0);
    }

    public void ManageSkill(string skill)
    {
        if (skill == "gravite")
        {
            StartGlobalCoolDown();
            photonView.RPC("Gravite", PhotonTargets.AllBuffered);
        }
        if (skill == "chaussuresGlissantes")
        {
            StartGlobalCoolDown();
            ManageSkill(skill);
        }
    }

    [PunRPC]
    public void Gravite()
    {
        if (!player)
        {
            return;
        }
        player.rb.gravityScale *= 10;

        StartCoroutine(ResetGravityScale());
    }

    private IEnumerator ResetGravityScale()
    {
        yield return new WaitForSeconds(2.0f);
        player.rb.gravityScale /= 10;
    }


    [PunRPC]
    public void slipperyShoes()
    {
        if (!player)
        {
            return;
        }
        var collider = player.GetComponent<BoxCollider2D>();


        StartCoroutine(ResetSlipperyShoes());

        ChangeColliderFriction(collider, 5f);
    }

    private void ChangeColliderFriction(BoxCollider2D collider, float frictionValue)
    {
        PhysicsMaterial2D newMaterial = new PhysicsMaterial2D
        {
            friction = frictionValue
        };

        collider.sharedMaterial = newMaterial;
    }

    private IEnumerator ResetSlipperyShoes()
    {
        var collider = player.GetComponent<BoxCollider2D>();
        yield return new WaitForSeconds(2.0f);
        ChangeColliderFriction(collider, 0.4f);
    }




}
