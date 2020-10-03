using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private InterruptibleCoroutine m_camRotCoroutine = default;
	private float m_rotationSec = 0.5f;
    void Start()
    {
        var player = GetComponentInParent<PlayerController>();
        player.OnTurnLeft.AddListener(OnTurnLeft);
        player.OnTurnRight.AddListener(OnTurnRight);
    }

    private void OnTurnLeft(){
        var prevTargetRot = m_camRotCoroutine.StopCoroutine(this, false);
		var targetRot = prevTargetRot is null ? Quaternion.LookRotation(-transform.right) :
			(Quaternion)prevTargetRot * Quaternion.Euler(0, -90f, 0);
		InterruptibleCoroutine.Callback afterRot = (bool execute) => {
			if (execute){
				transform.rotation = targetRot;
			}
			return targetRot;
		};
		m_camRotCoroutine = StartCoroutine(gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);

    }

	private void OnTurnRight(){
		var prevTargetRot = m_camRotCoroutine.StopCoroutine(this, false);
		var targetRot = prevTargetRot is null ? Quaternion.LookRotation(transform.right) :
			(Quaternion)prevTargetRot * Quaternion.Euler(0, 90f, 0);
		InterruptibleCoroutine.Callback afterRot = (bool execute) => {
			if (execute){
				transform.rotation = targetRot;
			}
			return targetRot;
		};
		m_camRotCoroutine = StartCoroutine(gameObject.Turn(targetRot, afterRot, m_rotationSec)).WrapAction(afterRot);
	}
}
