using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    private enum Direction 
    {
        Up,Right,Left
    }

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerInput playerInput;

    [Header("��Ծ")]
    public int stepPoint;
    private int pointResult;
    [Header("��Ծ")]
    public float jumpDistance;
    private float moveDistance;
    private Vector2 destination;
    private Vector2 touchPosition;
    private Direction dir;


    private bool buttonHeld;

    private bool isJump;

    private bool canJump;

    private bool isDead;

    //�ж����ͼ�ⷵ�ص�����
    private RaycastHit2D[] result = new RaycastHit2D[2];
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (isDead)
        {
            DisableInput();
            return;
        }
        if (canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        if(isJump)
            rb.position = Vector2.Lerp(transform.position,destination,0.134f);
        //Debug.Log("JUMP123123" + rb.position);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !isJump)
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up * 0.1f, Vector2.zero, result);

            bool inWater = true;
            foreach(var hit in result)
            {
                if (hit.collider == null) continue;
                if (hit.collider.CompareTag("Wood"))
                {
                    //����ľ���ƶ�
                    transform.parent = hit.collider.transform;
                    inWater = false;
                }
            }
            //û��ľ����Ϸ����
            if (inWater && !isJump)
            {
                Debug.Log("GAMEOVER!");
                isDead = true;
            }
        }
        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("Game Over");
            isDead = true;
        }

        if (!isJump && other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            isDead = true;
        }

        if (isDead)
        {
            //�㲥֪ͨ��Ϸ����
            EventHandler.CallGameOverEvent();
        }
    }

    #region INPUT ����ص�����
    public void Jump(InputAction.CallbackContext context)
   {
        //ִ����Ծ����Ծ�ľ��룬��¼������������Ծ����Ч
        if (context.performed && !isJump)
        {
            moveDistance = jumpDistance;
            //Debug.Log("JUMP" + context);
            //ִ����Ծ
            //Debug.Log(destination);
            canJump = true;

            AudioManager.instance.SetJumpClip(0);
        }
        if (dir == Direction.Up && context.performed && !isJump)
        {
            pointResult += stepPoint;
        }
        
   }
   public void LongJump(InputAction.CallbackContext context)
   {
        if (context.performed && !isJump) 
        {
            moveDistance = jumpDistance * 2;
            buttonHeld = true;
            AudioManager.instance.SetJumpClip(1);
        }
        if (context.canceled && buttonHeld && !isJump)
        {
            // ִ����Ծ
            //Debug.Log("LONG JUMP!" + " " + moveDistance);
            if (dir == Direction.Up)
            {
                pointResult += stepPoint * 2;
            }
            buttonHeld = false;
            canJump = true;
        }
    }
    public void GetTouchPosition(InputAction.CallbackContext context)
   {
        if (context.performed)
        { //Debug.Log(context.ReadValue<Vector2>());
            touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //Debug.Log(touchPosition);
            var offset = ((Vector3)touchPosition - transform.position).normalized;

            if (Mathf.Abs(offset.x) <= 0.7f)
            {
                dir = Direction.Up;
            }
            else if (offset.x < 0.7)
            {
                dir = Direction.Left;
            }
            else if (offset.x > 0.7)
            {
                dir = Direction.Right;
            }
        }
    }
    #endregion

    /// <summary>
    /// ����ִ����Ծ���� 
    /// </summary>
    private void TriggerJump()
    {
        //:����ƶ����򣬲��Ŷ���
        canJump = false;
        switch (dir)
        {
            case Direction.Up:
                //�����л����ҷ��򶯻�
                anim.SetBool("isSide", false); 
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = Vector3.one;
                break;
            case Direction.Right:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y);
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Direction.Left:
                anim.SetBool("isSide", true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y);
                transform.localScale = Vector3.one;
                break;
            default:
                break;
        }

        anim.SetTrigger("Jump");

    }
    #region Animation Event
    public void JumpAnimationEvent()
    {
        //������Ծ��Ч
        AudioManager.instance.PLayJumpFx();
        //�ı�״̬
        isJump = true;

        //�޸�����ͼ��
        sr.sortingLayerName = "Front";
        transform.parent = null;
    }

    public void FinishJumpAnimationEvent()
    {
        isJump = false;

        //�޸�����ͼ��
        sr.sortingLayerName = "Middle";
        if(dir == Direction.Up && !isDead)
        {
            //TODO:�÷֡�������ͼ���

            EventHandler.CallGetPointEvent(pointResult);

            //Debug.Log("�ܵ÷�" + pointResult);
        }
    }
    #endregion

    private void DisableInput()
    {
        playerInput.enabled = false;
    }
}
