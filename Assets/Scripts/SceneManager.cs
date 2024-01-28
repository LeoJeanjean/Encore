using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [HideInInspector] public static SceneController Instance;

    public PhotonView photonView;
    private GameObject sceneContainer;

    public GameObject levelStart;
    private ObstacleData levelStartData;
    public GameObject levelEnd;
    private ObstacleData levelEndData;

    private bool isLevelGenerated;

    //public int levelLength;
    private int levelLength = 5;

    public List<ObstacleData> obstacleGroupsList;

    public GameObject Character;

    private Vector2 obstacleSpawnPosition;

    public int levelsCleared = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        sceneContainer = GameObject.FindWithTag("SceneContainer");

        levelStartData = levelStart.GetComponent<ObstacleData>();
        levelEndData = levelEnd.GetComponent<ObstacleData>();
    }

    void Start()
    {
        var runner = PhotonNetwork.playerName.Split("/")[1];
        if (runner == "true")
        {
            GenerateLevel();
        }
    }


    public void GenerateLevel()
    {
        isLevelGenerated = true;

        Vector2 currentBeginning = levelStartData.end.transform.position;
        int previousIndex = -1;

        for (int i = 0; i < levelLength + levelsCleared * 2; i++)
        {
            int newObstacleIndex = Random.Range(0, obstacleGroupsList.Count); //pour chaque niveau fini, on augmente les chances d'avoir des niveaux difficiles
            if (levelsCleared < 10)
            {
                if (obstacleGroupsList[newObstacleIndex].difficulty == 0 && levelsCleared >= 1) newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
                if (obstacleGroupsList[newObstacleIndex].difficulty == 0 && levelsCleared >= 2) newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
                if (obstacleGroupsList[newObstacleIndex].difficulty == 0 && levelsCleared >= 3) newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
                if (obstacleGroupsList[newObstacleIndex].difficulty == 0 && levelsCleared >= 4) newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
            }
            else if (newObstacleIndex < 4)
            {
                newObstacleIndex = Random.Range(4, 7);
            }

            if (newObstacleIndex == previousIndex) newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
            previousIndex = newObstacleIndex;

            obstacleSpawnPosition = currentBeginning + obstacleGroupsList[newObstacleIndex].beginning.transform.position * new Vector2(-1, -1);

            PhotonNetwork.Instantiate(obstacleGroupsList[newObstacleIndex].name, obstacleSpawnPosition, Quaternion.identity, 0);

            currentBeginning = new Vector2(
                obstacleSpawnPosition.x + obstacleGroupsList[newObstacleIndex].end.transform.position.x,
                obstacleSpawnPosition.y + obstacleGroupsList[newObstacleIndex].end.transform.position.y
            );
        }
        obstacleSpawnPosition = currentBeginning + levelEndData.beginning.transform.position * new Vector2(-1, -1);
        PhotonNetwork.Instantiate(levelEnd.name, obstacleSpawnPosition, Quaternion.identity, 0);
    }

    public void DestroyLevel()
    {
        photonView.RPC("DestroyLevelRCP", PhotonTargets.AllBuffered);

    }

    [PunRPC]
    private void DestroyLevelRCP()
    {
        //destroy all gameobjects with tag obstacle
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        var runner = PhotonNetwork.playerName.Split("/")[1];
        if (runner == "true")
        {

            GenerateLevel();
        }
    }
}
