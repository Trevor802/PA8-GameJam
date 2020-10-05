using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PlayerTrace
{
    public Vector3 Position;

};

public class MonsterMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 10f;
    public float interval = 0.5f;
    public float monsterDelay = 3f;
    private float timer = 0f;

    private List<Transform> playerTraceBuffer = new List<Transform>(30);
    private int bufferIndex = 0;
    private int followIndex = 0;

    private bool monsterStart = false;
    private void Start()
    {
        if (player == null)
            throw new System.Exception("Player is not set.");

        
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(!monsterStart && timer >= monsterDelay)
        {
            monsterStart = true;
            Debug.Log("Monster starts");
        }

        if (monsterStart)
        {
            MarkPlayerTransform();
            FollowPlayerTrace();
        }
        // Follow up 
    }

    private void FollowPlayerTrace()
    {
        if (followIndex >= 30)
            followIndex = 0;

        Vector3 direction = (playerTraceBuffer[followIndex].position - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * speed);

        if(Vector3.Distance(transform.position, playerTraceBuffer[followIndex].position) <= 1f)
        {
            followIndex++;
        }
    }

    private void MarkPlayerTransform()
    {
        // Mark player's transform data
        if(playerTraceBuffer.Count < playerTraceBuffer.Capacity)
        {
            if (timer >= interval)
            {
                playerTraceBuffer.Add(player.transform);
                Debug.Log(playerTraceBuffer.Count);
                timer = 0;
            }
            
        }
        else
        {
            if (bufferIndex >= 30)
                bufferIndex = 0;

            if (timer >= interval)
            {
                playerTraceBuffer[bufferIndex] = player.transform;
                bufferIndex++;
                timer = 0;
            }
            
        }
        
    }
}
