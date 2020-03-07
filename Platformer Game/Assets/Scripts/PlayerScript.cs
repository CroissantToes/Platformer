using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text win;
    public Text life;

    private int scoreValue = 0;
    private int lifeValue = 3;

    public AudioSource music;
    public AudioClip clip1;
    public AudioClip clip2;

    public Animator anim;

    private bool facingRight = true;

    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        win.text = "";
        life.text = "Lives: " + lifeValue.ToString();
        music.clip = clip1;
        music.loop = true;
        music.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

    }

    private void Update()
    {
        if (onGround == false)
        {
            anim.SetInteger("State", 2);
        }

        if (onGround == true && !Input.anyKey)
        {
            anim.SetInteger("State", 0);
        }

        if (onGround == true && Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (onGround == true && Input.GetKey(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4)
            {
                transform.position = new Vector2(0, 50);
                lifeValue = 3;
            }
            if(scoreValue == 8)
            {
                win.text = "You win! Game made by Chris Santos";
                music.loop = false;
                music.clip = clip2;
                music.Play();
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            lifeValue -= 1;
            life.text = "Lives: " + lifeValue.ToString();
            Destroy(collision.collider.gameObject);
            if(lifeValue <= 0)
            {
                win.text = "You Lose! Press R to restart";
                this.gameObject.SetActive(false);
                Destroy(this);
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = true;
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
                onGround = false;
            }
        }

        if(collision.collider.tag == "Fall")
        {
            if (scoreValue >= 4)
                transform.position = new Vector2(0, 50);
            else
                transform.position = new Vector2(0, 0);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
