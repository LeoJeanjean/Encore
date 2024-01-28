using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Sprite[] sprites;
    public AudioClip[] sounds;

    public PhotonView photonView;

    public GameObject RunnerPrefab;
    public GameObject ShooterCanvas;

    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public TMP_Text PingText;

    public GameObject anvilPrefab;

    public GameObject bananaPrefab;

    public int target = 30;

    public float abilityCoolDownLenght = 1.5f;

    public float abilityCoolDownOpacity = 0f;

    public bool abilityCoolDown = false;

    public bool usingAbility = false;

    public List<string> abilitySkills = new List<string> { };

    public AudioSource audioSource;

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


    private void Start()
    {
        /*  
        "Rideaux",
        "Pouet",
        "PicsInvisibles",
        "TerrainFolie", 
        "ChaussuresQuiCollent",
        "PointColle",
        "pointGlace",
        "Blague", 
        "Gravite",     
        */

        abilitySkills = new List<string>  {
                "Enclume",
                "Acceleration",
                "Rallentissement",
                "GameplayReverse",
                "UpsideDown",
                "ChaussuresGlissantes",
                "Banane",
                "Seisme",
                "Pouet"
            };
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

    public void SpawnBanana(Vector3 mousePos)
    {
        PhotonNetwork.Instantiate(bananaPrefab.name, mousePos, Quaternion.identity, 0);
    }

    public void ManageSkill(string skill)
    {
        if (skill == "gravite")
        {
            StartGlobalCoolDown();
            photonView.RPC("Gravite", PhotonTargets.AllBuffered);
        }
        if (skill == "acceleration")
        {
            StartGlobalCoolDown();
            photonView.RPC("Acceleration", PhotonTargets.AllBuffered);
        }

        if (skill == "ralentissement")
        {
            StartGlobalCoolDown();
            photonView.RPC("Ralenti", PhotonTargets.AllBuffered);
        }

        if (skill == "gameplayreverse")
        {
            StartGlobalCoolDown();
            photonView.RPC("GameplayReverse", PhotonTargets.AllBuffered);
        }

        if (skill == "upsidedown")
        {
            StartGlobalCoolDown();
            photonView.RPC("UpsideDown", PhotonTargets.AllBuffered);
        }

        if (skill == "chaussuresglissantes")
        {
            StartGlobalCoolDown();
            photonView.RPC("ChaussuresGlissantes", PhotonTargets.AllBuffered);
        }

        if (skill == "seisme")
        {
            StartGlobalCoolDown();
            photonView.RPC("Seisme", PhotonTargets.AllBuffered);
        }


        if (skill == "pouet")
        {
            StartGlobalCoolDown();
            photonView.RPC("Pouet", PhotonTargets.AllBuffered);
        }
    }

    // GOOD
    [PunRPC]
    public void Gravite()
    {
        Debug.Log("Graviti");
        if (!player)
        {
            return;
        }
        player.rb.gravityScale *= 10;

        StartCoroutine(ResetGravityScale());
    }

    private IEnumerator ResetGravityScale()
    {
        yield return new WaitForSeconds(4.0f);
        player.rb.gravityScale /= 10;
    }


    // GOOD
    [PunRPC]
    public void Acceleration()
    {
        Debug.Log("Accelere");
        if (!player)
        {
            return;
        }
        player.speed *= 4;
        player.maxSpeed *= 4;

        StartCoroutine(ResetAcceleration());
    }

    private IEnumerator ResetAcceleration()
    {
        yield return new WaitForSeconds(4.0f);
        player.speed /= 4;
        player.maxSpeed /= 4;
    }


    // GOOD
    [PunRPC]
    public void Ralenti()
    {
        Debug.Log("Ralenti");
        if (!player)
        {
            return;
        }
        player.speed /= 4;
        player.maxSpeed /= 4;

        StartCoroutine(ResetRalenti());
    }

    private IEnumerator ResetRalenti()
    {
        yield return new WaitForSeconds(4.0f);
        player.speed *= 4;
        player.maxSpeed *= 4;
    }


    // GOOD
    [PunRPC]
    public void GameplayReverse()
    {
        Debug.Log("Reverse Gameplay");
        if (!player)
        {
            return;
        }
        player.reverse = true;

        StartCoroutine(ResetGameplayReverse());
    }

    private IEnumerator ResetGameplayReverse()
    {
        yield return new WaitForSeconds(8.0f);
        player.reverse = false;
    }

    // GOOD
    [PunRPC]
    public void UpsideDown()
    {
        Debug.Log("UpsideDown");
        if (!player)
        {
            return;
        }
        var cam = player.GetComponentInChildren<Camera>();

        cam.transform.Rotate(180f, 0f, 0f);
        StartCoroutine(ResetUpsideDown());

    }

    private IEnumerator ResetUpsideDown()
    {
        var cam = player.GetComponentInChildren<Camera>();
        yield return new WaitForSeconds(5.0f);

        cam.transform.rotation = Quaternion.identity;
    }

    // GOOD
    [PunRPC]
    public void ChaussuresGlissantes()
    {
        if (!player)
        {
            return;
        }
        player.slippery = true;
        StartCoroutine(ResetChaussuresGlissantes());
    }

    private IEnumerator ResetChaussuresGlissantes()
    {
        yield return new WaitForSeconds(2.0f);
        player.slippery = false;
    }




    // GOOD
    [PunRPC]
    public void Seisme()
    {
        Debug.Log("Seisme");
        if (!player)
        {
            return;
        }
        var cam = player.GetComponentInChildren<Camera>();
        var cameraShakeScript = cam.GetComponent<CameraShake>();
        playSound("Seisme");
        if (cameraShakeScript != null)
        {
            cameraShakeScript.StartShake(4);
        }
    }



    [PunRPC]
    public void Pouet()
    {
        Debug.Log("Pouet");
        playSound("Pouet");
    }

    public void playSoundAll(string soundName)
    {
        photonView.RPC("playSoundRPC", PhotonTargets.AllBuffered, soundName);

    }

    [PunRPC]
    private void playSoundRPC(string soundName)
    {
        Debug.Log(soundName);

        foreach (AudioClip sound in sounds)
        {
            if (sound.name == soundName)
            {
                audioSource.clip = sound;

                audioSource.Play();
                break;
            }
        }

    }


    public void playSound(string soundName)
    {
        Debug.Log(soundName);

        foreach (AudioClip sound in sounds)
        {
            if (sound.name == soundName)
            {
                audioSource.clip = sound;

                audioSource.Play();
                break;
            }
        }
    }

}
