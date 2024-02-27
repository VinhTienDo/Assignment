using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public Rigidbody2D rigidbody2D;
    private float xInput;
    public float moveSpeed;
    public float jumpForce;
    public Animator anim;
    [SerializeField] private bool isRun;

    private int facingDirection = 1;
    private bool facingRight = true;

    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] float jumpSpeed = 10f;
    bool isSpacePressed = false;

    public Text Diem;
    public Text Mau;
    int score = 0;
    int heart = 2;

    public AudioClip jump;
    public AudioClip collectCoin;
    public AudioClip collectheart;
    public AudioClip triggerTrap;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Diem = GameObject.Find("Diem").GetComponent<Text>();
        Mau = GameObject.Find("Mau").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        FlipController();
        AnimatorController();
        CollisionCheck();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("va cham vao: " + other.gameObject.tag);

        if (other.gameObject.tag == "coin")
        {
            score+=5;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(collectCoin);
            Diem.text = "Score: " + score.ToString();
        }
        else if (other.gameObject.tag == "heart")
        {
            heart++;
            audioSource.PlayOneShot(collectheart);
            Destroy(other.gameObject);
            Mau.text = "x " + heart.ToString();

        }
        else if (other.gameObject.tag == "trap")
        {
            heart--;
            Mau.text = "x " + heart.ToString();
            audioSource.PlayOneShot(triggerTrap);
            if (heart <= 0)
            {
                Application.LoadLevel("Menu");
            }
        }
        else if (other.gameObject.tag == "enemy")
        {
            heart--;
            Mau.text = "x " + heart.ToString();
            if (heart <= 0)
            {
                Application.LoadLevel("Menu");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("va cham vao: " + other.gameObject.tag);
        if (other.gameObject.tag == "door")
        {
            SceneManager.LoadScene("Menu");
        }
        else if (other.gameObject.tag == "nextlevel")
        {
            
            SceneManager.LoadScene("Scene2");
        }
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            //isSpacePressed = true; // Đánh dấu rằng nút Space đã được nhấn
            //rigidbody2D.AddForce(new Vector2(0, 6f), ForceMode2D.Impulse);
            //Debug.Log("click");
        }
        //else if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    isSpacePressed = false; // Đánh dấu rằng nút Space không được nhấn nữa
        //}
        //if (isSpacePressed)
        //{
        //    rigidbody2D.AddForce(new Vector2(0, 6f), ForceMode2D.Force); // Sử dụng ForceMode2D.Force để áp dụng lực liên tục
        //}
    }
    private void Movement()
    {
        rigidbody2D.velocity = new Vector2(xInput * moveSpeed, rigidbody2D.velocity.y);
    }

    // test
    private void Jump()
    {
        if (isGrounded)
        {
            audioSource.PlayOneShot(jump);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
        }

    }
    private void AnimatorController()
    {
        if (rigidbody2D.velocity.x != 0)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        anim.SetFloat("yVelocity", rigidbody2D.velocity.y);
        anim.SetBool("isRun", isRun);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rigidbody2D.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rigidbody2D.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
