using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool LoadLevel = false;
    public string LevelName;
    private bool m_canUse = true;
    private float m_portalCooldown = 1f;
    public Portal Exit;
    private void OnTriggerEnter(Collider other) {
        if (!m_canUse)
            return;
        if (LoadLevel){
            SceneManager.LoadScene(LevelName);
            return;
        }
        var player = other.GetComponent<PlayerController>();
        if (player != null){
            Exit.Out(player);
        }
    }

    private void Out(PlayerController player){
        m_canUse = false;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.ResetDirection();
        player.OnResetDirection(player.MovingDir);
        Invoke("ResetPortal", m_portalCooldown);
    }

    private void ResetPortal() => m_canUse = true;
}
