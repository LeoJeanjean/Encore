using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatformScript : MonoBehaviour
{
    public bool isMoving;
    public float newPositionX;
    public float newPositionY;
    public float movementSpeed;

    public bool isRotating;
    public float rotationSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = gameObject.transform;
        
        if(isMoving) transform.DOLocalMove(new Vector2(newPositionX,newPositionY), movementSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        if(isRotating) transform.DORotate(new Vector3(0, 360, 0), rotationSpeed).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
