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
    private float startTimer = 0f;

    private List<Vector3> playerTraceBuffer = new List<Vector3>(1000);
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
        if (!monsterStart)
            startTimer += Time.deltaTime;

        if(!monsterStart && startTimer >= monsterDelay)
        {
            monsterStart = true;
            Debug.Log("Monster starts");
        }

        MarkPlayerTransform();
        if (monsterStart)
        {
            FollowPlayerTrace();
            
        }
        // Follow up 
    }

    

    private void FollowPlayerTrace()
    {
        if (followIndex >= 1000)
            followIndex = 0;

        Vector3 direction = (playerTraceBuffer[followIndex] - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * speed);

        float dis = Vector3.Distance(transform.position, playerTraceBuffer[followIndex]);
        //Debug.Log(dis);
        if (dis <= 0.5f)
        {
            followIndex++;
        }
    }

    private void NavigateToPlayer()
    {

    }

    private void MarkPlayerTransform()
    {
        // Mark player's transform data
        if(playerTraceBuffer.Count < playerTraceBuffer.Capacity)
        {
            if (timer >= interval)
            {
                Vector3 temp = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                playerTraceBuffer.Add(temp);
                //Debug.Log(playerTraceBuffer.Count);
                timer = 0;
            }
            
        }
        else
        {
            if (bufferIndex >= 1000)
                bufferIndex = 0;

            if (timer >= interval)
            {
                Vector3 temp = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                playerTraceBuffer[bufferIndex] = temp;
                bufferIndex++;
                timer = 0;
            }
            
        }
        
    }
}
