﻿using System;
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
    public Vector3 MovingDir => m_movingDir;
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
    public bool canTurn{get; set;} = false;
    private Vector3 m_lastGroundNormal;

    public AudioClip pickUp;
    private void Awake()
    {
		ResetDirection();
    }

    private void Start() {
        AudioManager.Instance.PlayMusic();
        if (inventory == null)
            throw new System.Exception("Inventory is not set for player");
        if (pickUp == null)
            throw new System.Exception("Pickup audio clip is not set for player");
    }

	public void ResetDirection(){
		m_movingDir = transform.forward;
	}

    private void TurnLeft(){
        if (!canTurn){
            return;
        }
		m_movingDir = Vector3.Cross(m_movingDir, transform.up);
        // transform.rotation = Quaternion.LookRotation(m_movingDir, transform.up);
		OnTurnLeft?.Invoke(m_movingDir);
    }

    private void TurnRight(){
        if (!canTurn){
            return;
        }
		m_movingDir = Vector3.Cross(transform.up, m_movingDir);
        // transform.rotation = Quaternion.LookRotation(m_movingDir, transform.up);
		OnTurnRight?.Invoke(m_movingDir);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A)){
            TurnLeft();
        }
        else if(Input.GetKeyDown(KeyCode.D)){
            TurnRight();
        }
        else if (Input.GetKeyDown(KeyCode.R)){
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
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
        
        if (mItemToPickup != null)
        {
            Debug.Log(mItemToPickup.Name);
            inventory.AddItem(mItemToPickup);
            mItemToPickup.OnPickup();
            //Hud.CloseMessagePanel("");
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
        var result = Physics.Raycast(origin, -transform.up, out hit, dist, LayerMask.GetMask("Soft", "Hard"));
        return result;
    }

    private void SlopeDetection(){
        RaycastHit frontHit;
        var _1 = transform.position + m_movingDir * Time.deltaTime;
        // var _1 = GetFrontPoint();
        var frontResult = CheckGround(_1, out frontHit);
        // RaycastHit centerHit;
        // var centerResult = CheckGround(transform.position, out centerHit);
        if (frontResult){
            var groundNormal = frontHit.normal;
            /*
            var _2 = Vector3.Dot(m_movingDir, groundNormal);
            // Floor
            if (Utilities.FastApproximately(_2, 0, 0.15f)){
                return;
            }
            // Incoming Upslope
            else if (_2 < 0){
                // TODO: 1 frame freeze
                if (_2 < -0.5)
                    VelocityAlignToGround(groundNormal);
                else
                    RotationAlignToGround(groundNormal);
            }
            // Incoming Downslope
            else if(_2 > 0){
                // TODO: Need attach
                if (_2 > 0.5)
                    VelocityAlignToGround(groundNormal);
                else
                    RotationAlignToGround(groundNormal);
            }
            */
            var layerName = LayerMask.LayerToName(frontHit.transform.gameObject.layer);
            if (layerName == "Hard"){
                VelocityAlignToGround(groundNormal);
            }
            else if (layerName == "Soft"){
                var _2 = Vector3.Dot(m_lastGroundNormal, groundNormal);
                if (!Utilities.FastApproximately(_2, 1f, 0.0001f)){
                    RotationAlignToGround(groundNormal);
                    m_lastGroundNormal = groundNormal;
                }
            }
            else{

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
        //if (item != null)
        {
           // Hud.CloseMessagePanel("");
            mItemToPickup = null;
        }
    }
}
