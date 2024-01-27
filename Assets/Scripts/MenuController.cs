using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
   [SerializeField] private string VersionName = "0.1";
   [SerializeField] private GameObject UserNameMenu;
   [SerializeField] private GameObject ConnectPannel;

   [SerializeField] private TMP_InputField CreateGameInput;
   [SerializeField] private TMP_InputField JoinGameInput;
   [SerializeField] private TMP_InputField UserNameInput;

   [SerializeField] private GameObject StartButton;


   private void Awake()
   {
      PhotonNetwork.ConnectUsingSettings(VersionName);
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
         StartButton.SetActive(true);
      }
      else
      {
         StartButton.SetActive(false);
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

   }

   public void JoinGame()
   {
      RoomOptions roomOptions = new RoomOptions();
      roomOptions.MaxPlayers = 2;
      PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
   }

   private void OnJoinedRoom()
   {
      PhotonNetwork.LoadLevel("MainGame");
   }
}
