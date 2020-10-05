using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Inventory inventory;
    public HUD Hud;
    private IInventoryItem mItemToPickup = null;

    private Vector3 m_movingDir = Vector3.forward;
    private Vector3 m_gravityDir = Vector3.down;
	[SerializeField]
	private float m_movingSpeed = 10f;
	[HideInInspector]
	public UnityAction<Vector3> OnTurnLeft;
	[HideInInspector]
	public UnityAction<Vector3> OnTurnRight;
	[HideInInspector]
	public UnityAction<Vector3> OnResetDirection;
	[SerializeField]
	private float m_jumpHeight = 2f;
	private float m_verticalSpeed;
    private float m_gravityValue = 20f;
    private bool isGrounded{get; set;} = false;
    private float m_turnCooldown = 0.5f;
    private bool m_canTurn = true;
    private void Awake()
    {
		ResetDirection();
    }

	private void ResetDirection(){
		m_movingDir = transform.forward;
	}

    private void TurnLeft(){
        if (!m_canTurn){
            return;
        }
        m_canTurn = false;
        Invoke("RefreshTurnCooldown", m_turnCooldown);
		m_movingDir = Vector3.Cross(m_movingDir, transform.up);
		OnTurnLeft?.Invoke(m_movingDir);
    }

    private void TurnRight(){
        if (!m_canTurn){
            return;
        }
        m_canTurn = false;
        Invoke("RefreshTurnCooldown", m_turnCooldown);
		m_movingDir = Vector3.Cross(transform.up, m_movingDir);
		OnTurnRight?.Invoke(m_movingDir);
    }

    private void RefreshTurnCooldown(){
        m_canTurn = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            TurnLeft();
        }
        else if(Input.GetKeyDown(KeyCode.D)){
            TurnRight();
        }
		// Movement
        {
			var vel = m_movingDir * m_movingSpeed;
            // m_verticalSpeed = Vector3.Dot(GetComponent<Rigidbody>().velocity, transform.up);
            if (!isGrounded){
			    m_verticalSpeed -= m_gravityValue * Time.deltaTime;
            }
            else{
                if (Input.GetKeyDown(KeyCode.Space)){
                    m_verticalSpeed = Mathf.Sqrt(2 * m_gravityValue * m_jumpHeight);
                }
                else if (m_verticalSpeed < 0){
                    m_verticalSpeed = 0;
                }
            }
            vel += transform.up * m_verticalSpeed;
			Move(vel);
        }
        
        if (Input.GetKeyDown(KeyCode.F) && mItemToPickup != null)
        {
            inventory.AddItem(mItemToPickup);
            mItemToPickup.OnPickup();
            Hud.CloseMessagePanel("");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            inventory.SwitchItem();
        }
        Debug.DrawLine(transform.position, transform.position + m_movingDir * 10f, Color.red);
    }

    private void Move(Vector3 velocity){
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private bool CheckGround(Vector3 origin, out RaycastHit hit, float extraDepth = 0.5f){
        var dist = GetComponent<CapsuleCollider>().height / 2f + extraDepth;
        var result = Physics.Raycast(origin, -transform.up, out hit, dist);
        return result;
    }

    private void SlopeDetection(){
        RaycastHit frontHit;
        var _1 = transform.position + m_movingDir * Time.deltaTime;
        var frontResult = CheckGround(_1, out frontHit);
        RaycastHit centerHit;
        var centerResult = CheckGround(transform.position, out centerHit);
        // if (frontResult && centerResult && frontHit.normal == centerHit.normal){
        //     var _3 = Vector3.Dot(transform.up, frontHit.normal);
        //     if (!Utilities.FastApproximately(_3, 1, 0.01f)){
        //         RotationAlignToGround(frontHit.normal);
        //     }
        // }
        if (frontResult){
            var groundNormal = frontHit.normal;
            var _2 = Vector3.Dot(m_movingDir, groundNormal);
            // Floor
            if (Utilities.FastApproximately(_2, 0, 0.01f)){
                return;
            }
            // Incoming Upslope
            else if (_2 < 0){
                // TODO: 1 frame freeze
                if (_2 < -0.7)
                    VelocityAlignToGround(groundNormal);
                else
                    RotationAlignToGround(groundNormal);
            }
            // Incoming Downslope
            else if(_2 > 0){
                // TODO: Need attach
                if (_2 > 0.7)
                    VelocityAlignToGround(groundNormal);
                else
                    RotationAlignToGround(groundNormal);
            }
        }
    }

    private void VelocityAlignToGround(Vector3 floorNormal){
        var newDir = Vector3.ProjectOnPlane(m_movingDir, floorNormal);
        m_movingDir = newDir.normalized;
    }
    private void RotationAlignToGround(Vector3 floorNormal){
        var newDir = Vector3.ProjectOnPlane(m_movingDir, floorNormal);
        m_movingDir = newDir.normalized;
        transform.rotation = Quaternion.LookRotation(m_movingDir, floorNormal);
        OnResetDirection(m_movingDir);
    }

    private Vector3 GetFrontPoint() => transform.position + 
        Vector3.ProjectOnPlane(m_movingDir, transform.up).normalized * GetComponent<CapsuleCollider>().radius;

    private Vector3 GetBackPoint() => transform.position + 
        Vector3.ProjectOnPlane(-m_movingDir, transform.up).normalized * GetComponent<CapsuleCollider>().radius;

    private void FixedUpdate() {
        RaycastHit hit;
        isGrounded = CheckGround(transform.position, out hit, 0.1f) 
            || CheckGround(GetFrontPoint(), out hit, 0.1f)
            || CheckGround(GetBackPoint(), out hit, 0.1f);
        SlopeDetection();
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
    //    if(item != null)
    //    {
    //        inventory.AddItem(item);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            mItemToPickup = item;
            Hud.OpenMessagePanel("");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            Hud.CloseMessagePanel("");
            mItemToPickup = null;
        }
    }
}
