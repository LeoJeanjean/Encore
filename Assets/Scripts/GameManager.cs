using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject RunnerPrefab;
    public GameObject ShooterCanvas;

    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public TMP_Text PingText;

    public GameObject anvilPrefab;

    public int target = 30;

    private void Update()
    {

        if (Application.targetFrameRate != target)
            Application.targetFrameRate = target;
        PingText.text = "Ping : " + PhotonNetwork.GetPing();
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

        PhotonNetwork.Instantiate(RunnerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }


    public void SpawnAnvil(Vector3 mousePos)
    {
        PhotonNetwork.Instantiate(anvilPrefab.name, mousePos, Quaternion.identity, 0);
    }


}
