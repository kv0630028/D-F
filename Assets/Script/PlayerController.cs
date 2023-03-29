using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private VirtualJoystick virtualJoystick;
    private float moveSpeed = 5;
    private float m_timeSinceAttack;
    private int m_currentAttack;

    Animator anim;



    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
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
            transform.position += new Vector3(x, y, 0).normalized * moveSpeed * Time.deltaTime;
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
        anim.SetBool("noBlood", false);
    }
}

