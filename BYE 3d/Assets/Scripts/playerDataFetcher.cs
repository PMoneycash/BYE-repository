using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerDataFetcher : MonoBehaviour
{
    [Header("Who to read from")]
    public GameObject player;  // drag your main player GO here

    [Header("Who to write to")]
    public Animator anim;      // this model's Animator (auto-found if left empty)

    // Cached refs
    Rigidbody playerRb;
    PlayerMovement2DIn3D playerMove;

    // Hashes for speed
    int speedHash    = Animator.StringToHash("Speed");
    int groundedHash = Animator.StringToHash("IsGrounded");

    void Awake()
    {
        if (!anim) anim = GetComponent<Animator>();               // grab local Animator by default
        BindPlayer(player);
    }

    // Call this if you swap the player at runtime
    public void BindPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        playerRb   = player ? player.GetComponent<Rigidbody>() : null;
        playerMove = player ? player.GetComponent<PlayerMovement2DIn3D>() : null;

        if (!playerRb)   Debug.LogWarning("PlayerDataFetcher: Rigidbody not found on player.");
        if (!playerMove) Debug.LogWarning("PlayerDataFetcher: PlayerMovement2DIn3D not found on player.");
        if (!anim)       Debug.LogWarning("PlayerDataFetcher: Animator not assigned/found on this model.");
    }

    void Update()
    {
        if (!anim || !playerRb || !playerMove) return;

        

        float horizontalSpeed = Mathf.Abs(playerRb.velocity.x); // scale if your blend tree expects larger values
        anim.SetFloat(speedHash, horizontalSpeed);
        anim.SetBool(groundedHash, playerMove.isGrounded);

        bool noKeys = !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
        bool Jump = Input.GetKey(KeyCode.Space);
        bool attack = Input.GetMouseButtonDown(0);

        anim.SetBool("Jump", Jump);
        anim.SetBool("keysOff", noKeys);
        anim.SetBool("attack", attack);
    }
}
