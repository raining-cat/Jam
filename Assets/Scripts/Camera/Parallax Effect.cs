using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //Starting position for parallax game object
    Vector2 startingPosition;

    //Start Z value of the parallax game object
    float startingZ;

    //Distance that the camera has moved from the starting position of the parallax object
    Vector2 canMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zdistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z + (zdistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //The further the object from the player, the fastest the ParallaxEffect object will move
    //Drag its Z value closer to the target to make it move slower
    float parallaxFactor => Mathf.Abs(zdistanceFromTarget) / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + canMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x,newPosition.y, startingZ);
    }
}
