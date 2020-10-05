using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnVolume : MonoBehaviour
{
    private PlayerController m_player;
    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerController>();
        if (player){
            m_player = player;
            player.canTurn = true;
            player.OnTurnLeft += DisableTurn;
            player.OnTurnRight += DisableTurn;
        }
    }

    private void DisableTurn(Vector3 vec) => m_player.canTurn = false;

    private void OnTriggerExit(Collider other) {
        var player = other.GetComponent<PlayerController>();
        if (player){
            player.canTurn = false;
            player.OnTurnLeft -= DisableTurn;
            player.OnTurnRight -= DisableTurn;
            m_player = null;
        }
    }

}
