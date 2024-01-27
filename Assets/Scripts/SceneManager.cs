using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [HideInInspector] public static SceneController Instance;
    private GameObject sceneContainer;

    public GameObject levelStart;
    private ObstacleData levelStartData;
    public GameObject levelEnd;
    private ObstacleData levelEndData;

    public List<ObstacleData> obstacleGroupsList;

    public GameObject Character;

    private Vector2 obstacleSpawnPosition;

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }

        sceneContainer = GameObject.FindWithTag("SceneContainer");

        levelStartData = levelStart.GetComponent<ObstacleData>();
        levelEndData = levelEnd.GetComponent<ObstacleData>();
    }

    void Start(){
        GenerateLevel();
        SpawnPlayer();
    }

    public void SpawnPlayer(){

    }

    public void GenerateLevel(){
        int levelLength = Random.Range(5, 7);
        
        Vector2 currentBeginning = levelStartData.end.transform.position;

        for(int i=0; i<levelLength; i++){

            int newObstacleIndex = Random.Range(0, obstacleGroupsList.Count);
            //currentbeginning is world position, to it you add negative of local offset of beginning of next item
            obstacleSpawnPosition = currentBeginning + obstacleGroupsList[newObstacleIndex].beginning.transform.position * new Vector2(-1,-1);
            //obstacleSpawnPosition = currentBeginning; le prob c'est que j'utilise les valeurs de l'objet précédentavec l'id du nouveau
            GameObject newObject = Instantiate<GameObject>(obstacleGroupsList[newObstacleIndex].gameObject, obstacleSpawnPosition, Quaternion.identity,sceneContainer.transform);
            //currentBeginning = obstacleGroupsList[newObstacleIndex].end;
            currentBeginning = new Vector2(
                newObject.transform.position.x + obstacleGroupsList[newObstacleIndex].end.transform.position.x,
                newObject.transform.position.y + obstacleGroupsList[newObstacleIndex].end.transform.position.y
            );
        }
        obstacleSpawnPosition = currentBeginning + levelEndData.beginning.transform.position * new Vector2(-1,-1);
        Instantiate<GameObject>(levelEnd, obstacleSpawnPosition, Quaternion.identity,sceneContainer.transform);
    }
}
