using System.Collections.Generic;
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

    public int levelLength;

    public List<ObstacleData> obstacleGroupsList;

    public GameObject Character;

    private Vector2 obstacleSpawnPosition;

    public int levelsCleared = 0;

    void Awake()
    {
        sceneContainer = GameObject.FindWithTag("SceneContainer");

        levelStartData = levelStart.GetComponent<ObstacleData>();
        levelEndData = levelEnd.GetComponent<ObstacleData>();
    }

    void Start()
    {
        GenerateLevel();
    }


    public void GenerateLevel()
    {

        Vector2 currentBeginning = levelStartData.end.transform.position;

        for (int i = 0; i < levelLength; i++)
        {

            int newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);

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

}
