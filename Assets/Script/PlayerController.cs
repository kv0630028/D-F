using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick virtualJoystick;

    public bool m_grounded = true;
    float m_jumpForce = 7.5f;
    private float moveSpeed = 5;
    private float m_timeSinceAttack;
    private float startingHeight;
    private float maxHeight;
    private int m_currentAttack;
    private Rigidbody2D m_body2d;

    Animator anim;



    public void Start()
    {
        anim = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(m_grounded == false)
        {
            if (transform.position.y > maxHeight)
            {
                maxHeight = transform.position.y;                              
            }           
            if (maxHeight - transform.position.y > maxHeight - startingHeight)
            {
                m_grounded = true;
                anim.SetBool("Grounded", m_grounded);
                m_body2d.gravityScale = 0;                
                m_body2d.Sleep();                
            }
        }
        anim.SetFloat("AirSpeedY", m_body2d.velocity.y);
        m_timeSinceAttack += Time.deltaTime;

        float x = virtualJoystick.Horizontal;   // 좌/우 이동
        float y = virtualJoystick.Vertical;     // 위/아래 이동

        if (x != 0 || y != 0)
        {            
            if (x >= 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            anim.SetInteger("AnimState", 1);            
            Vector3 move = new Vector3(x, y, 0).normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
            startingHeight += move.y;
        }
        else
        {
            anim.SetInteger("AnimState", 0);
        }
    }
    public void Action1()
    {
        m_currentAttack++;

        if (m_timeSinceAttack >= 0.25f)
        {
            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            anim.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
    }
    public void Action2_1()
    {
        anim.SetTrigger("Block");
        anim.SetBool("IdleBlock", true);

    }
    public void Action2_2()
    {
        anim.SetBool("IdleBlock", false);
    }

    public void Action3()
    {
        if(m_grounded == true)
        {
            startingHeight = transform.position.y;
            maxHeight = transform.position.y;

            anim.SetTrigger("Jump");
            m_grounded = false;
            anim.SetBool("Grounded", m_grounded);
            m_body2d.gravityScale = 1f;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);            
        }
        
    }
}

