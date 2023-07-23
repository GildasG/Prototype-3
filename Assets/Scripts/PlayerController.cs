using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public GameManager gameManager;
    public MoveLeft moveLeftScript;

    public float jumpForce = 10;
    public float gravityModifier;
    private int jumpCount = 0;
    public bool isOnGround = true;
    public bool gameOver = false;
    public bool dash = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moveLeftScript = GetComponent<MoveLeft>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !gameOver && jumpCount == 0)
        {
            jumpCount += 1;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        else if (Input.GetButtonDown("Jump") && !gameOver && jumpCount == 1)
        {
            jumpCount += 1;
            playerRb.AddForce(Vector3.up * jumpForce*0.75f, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        PlayerDash();
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    void PlayerDash()
    {
        if (Input.GetButton("Fire1"))
        {
            dash = true;
            playerAnim.SetFloat("Speed_Multiplier", 1.5f);
        }
        else
        {
            dash = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }
    }
}
