using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //References
    public Rigidbody2D playerRb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float moveSpeed = 9f;
    public float acceleration = 13f;
    public float deceleration = 16f;
    public float velocity = 0.96f;
    public Vector2 moveInput;

    public float gravityScale = 1.8f;

    public bool isJumping = false;
    public float jumpForce = 28f;
    public float jumpCutMultiplier = 0.4f;
    public float gravityScaleModifier = 2f;

    public float jumpBufferTime = 0.1f;
    public float jumpCoyoteTime = 0.15f;

    public float lastGroundedTime;
    public float lastJumpTime;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        groundCheck = transform.GetChild(0).GetComponent<Transform>();
        playerRb.gravityScale = gravityScale;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");

        if (Physics2D.OverlapBox(groundCheck.position, groundCheck.localScale, 0, groundLayer))
        {
            lastGroundedTime = jumpCoyoteTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            lastJumpTime = jumpBufferTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpCut();
        }

        if (playerRb.velocity.y < 0)
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        #region Run
        float targetSpeed = moveInput.x * moveSpeed;
        float speedDif = targetSpeed - playerRb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velocity) * Mathf.Sign(speedDif);

        playerRb.AddForce(movement * Vector2.right);
        #endregion
        #region Timer
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        #endregion
        #region Jump
        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            Jump();
            lastJumpTime = jumpBufferTime;
        }
        #endregion

    }

    private void Jump()
    {
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0f;
        lastJumpTime = 0f;
        isJumping = true; 
    }

    private void JumpCut()
    {
        if (playerRb.velocity.y > 0 && isJumping) 
        {
            playerRb.AddForce(Vector2.down * playerRb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        lastJumpTime = 0f;
    }
}
