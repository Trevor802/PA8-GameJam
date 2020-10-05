using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Inventory inventory;
    public bool LoadLevel = false;
    public string LevelName;
    public GameObject ExitPoint;
    private bool m_canUse = true;
    private float m_portalCooldown = 1f;
    public Portal Exit;
    private void OnTriggerEnter(Collider other) {
        //if (!m_canUse)
        //    return;
        //if (LoadLevel)
        //{
        //    SceneManager.LoadScene(LevelName);
        //    return;
        //}
        //var player = other.GetComponent<PlayerController>();
        //if (player != null)
        //{
        //    Exit.Out(player);
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F) && inventory.HasItem("Hook"))
        {
            if (!m_canUse)
                return;
            if (LoadLevel)
            {
                SceneManager.LoadScene(LevelName);
                return;
            }
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Exit.Out(player);
            }
        }

    }

    private void Out(PlayerController player){
        m_canUse = false;
        player.transform.position = ExitPoint.transform.position;
        player.transform.rotation = ExitPoint.transform.rotation;
        player.ResetDirection();
        player.OnResetDirection(player.MovingDir);
        Invoke("ResetPortal", m_portalCooldown);
    }

    private void ResetPortal() => m_canUse = true;

}
