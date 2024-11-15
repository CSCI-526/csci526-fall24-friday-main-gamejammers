using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLimitedMove : MonoBehaviour
{

    public float minSpeed = 0.5f;
    private float speed;
    private GameObject player;
    public Material farObstacle;
    public Material nearObstacle;
    private Grapple gp;
    public Vector3 pointA;
    //new Vector3(28.5F, 8.2F, 168.2F);
    public Vector3 pointB;
    //new Vector3(-34.7F, 8.2F, 168.2F);

    void Start()
    {

      player = GameObject.FindWithTag("Player");
      if (player == null) {
          Debug.LogError("Player1 tag not found! Make sure your player object has the correct tag.");
          return;
      }
      gp = player.GetComponent<Grapple>();
      Vector3 scale = transform.localScale;

      float scaleFactor = (scale.x + scale.y + scale.z) / 3f;

      speed =  minSpeed * scaleFactor;
    }

    void Update()
    {
      // transform.Translate(speed * Time.deltaTime * Vector3.right);
      transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time/2.5f, 1));
      
      float distance = Vector3.Distance(player.transform.position, transform.position);

      if (distance < gp.maxGrappleDistance) {
          GetComponent<Renderer>().material = nearObstacle;
      } else {
          GetComponent<Renderer>().material = farObstacle;
      }

    }
}

