using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Define.PlayerState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;

            switch(_state)
            {
                case Define.PlayerState.Idle:
                    anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.PlayerState.Move:
                    anim.CrossFade("Move", 0.1f);
                    break;
                case Define.PlayerState.Jump:
                    break;
                case Define.PlayerState.Attack:
                    break;
                case Define.PlayerState.Die:
                    break;
            }
        }
    }

    [SerializeField] Define.PlayerState _state;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    GameObject bulletOrigin;
    Transform firePos;

    float maxHp;
    float hp;
    float damage;
    float moveSpeed;
    float rollPower;

    Define.Weapon weaponType = Define.Weapon.Unknow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        bulletOrigin = Resources.Load<GameObject>("Prefabs/Bullet");
        firePos = transform.Find("FirePos");


        maxHp = 100f;
        hp = maxHp;
        damage = 10f;
        moveSpeed = 7.5f;
    }

    void Update()
    {
        if (State == Define.PlayerState.Die)
            return;

        UpdateMove();
        UpdateAttack();
    }

    void UpdateMove()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = (Vector3.right * horizontal) + (Vector3.forward * vertical);

        if (dir != Vector3.zero)
        {
            State = Define.PlayerState.Move;
        }
        else
        {
            State = Define.PlayerState.Idle;
        }

        //rb.velocity = dir * moveSpeed;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void UpdateAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            State = Define.PlayerState.Attack;
        }
    }

    void OnAttack()
    {

    }

    void OnDodge()
    {

    }
    
}
