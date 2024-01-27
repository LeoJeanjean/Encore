using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : Photon.MonoBehaviour
{
   public PhotonView photonView;

   [SerializeField] private string VersionName = "0.1";
   [SerializeField] private GameObject UserNameMenu;
   [SerializeField] private GameObject RoomMenu;
   [SerializeField] private GameObject ConnectPannel;

   [SerializeField] private TMP_InputField CreateGameInput;
   [SerializeField] private TMP_InputField JoinGameInput;
   [SerializeField] private TMP_InputField UserNameInput;

   [SerializeField] private GameObject ContinueButton;

   [SerializeField] private TMP_Text p1Text;
   [SerializeField] private TMP_Text p2Text;

   [SerializeField] private Button StartButton;

   private bool _p1;
   private string _name;

   private void Awake()
   {
      PhotonNetwork.ConnectUsingSettings(VersionName);


      StartButton.onClick.AddListener(StartGame);

   }

   private void Start()
   {
      UserNameMenu.SetActive(true);
   }

   private void OnConnectedToMaster()
   {
      PhotonNetwork.JoinLobby(TypedLobby.Default);

      Debug.Log("Connected");
   }


   public void ChangeUserNameInput()
   {
      if (UserNameInput.text.Length >= 3)
      {
         ContinueButton.SetActive(true);
      }
      else
      {
         ContinueButton.SetActive(false);
      }
   }

   public void SetUserName()
   {
      UserNameMenu.SetActive(false);
      PhotonNetwork.playerName = UserNameInput.text;
   }


   public void CreateGame()
   {
      PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
      _p1 = true;
      _name = PhotonNetwork.playerName;
      RoomMenu.SetActive(true);
      ConnectPannel.SetActive(false);
   }

   public void JoinGame()
   {
      PhotonNetwork.JoinRoom(JoinGameInput.text);
      _p1 = false;
      _name = PhotonNetwork.playerName;
      RoomMenu.SetActive(true);
      ConnectPannel.SetActive(false);
   }


   private void StartGame()
   {
      PhotonNetwork.LoadLevel("MainGame");
   }

   private void OnJoinedRoom()
   {

      photonView.RPC("UpdateText", PhotonTargets.AllBuffered, _p1, _name);
   }

   [PunRPC]
   private void UpdateText(bool p1, string name)
   {
      Debug.Log(p1);
      Debug.Log(name);

      if (p1)
      {
         p1Text.text = "Joueur 1 : " + name;
      }
      else
      {
         p2Text.text = "Joueur 2 : " + name;
      }
   }
}
