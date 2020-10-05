using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PlayerController>()){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
