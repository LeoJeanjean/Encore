using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public TMP_Text PingText;


    private void Update()
    {
        PingText.text = "Ping : " + PhotonNetwork.GetPing();
    }

    private void Awake()
    {
        GameCanvas.SetActive(true);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.RandomRange(-1f, 1f);

        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }



}
