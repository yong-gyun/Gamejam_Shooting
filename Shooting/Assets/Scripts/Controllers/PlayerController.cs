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
                    anim.CrossFade("Attack", 0.1f);
                    break;
                case Define.PlayerState.Die:
                    break;
            }
        }
    }

    [SerializeField] Define.PlayerState _state;
    Rigidbody rb;
    Animator anim;
    [SerializeField] GameObject bulletOrigin;
    [SerializeField] Transform firePos;

    float maxHp;
    float hp;
    float damage;
    float moveSpeed;
    float rotSpeed;
    float rollPower;
    bool isAction;
    
    Vector3 dir;
    Define.Weapon weaponType = Define.Weapon.Unknow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        //bulletOrigin = Resources.Load<GameObject>("Prefabs/Bullet");
        firePos = transform.Find("FirePos");

        maxHp = 100f;
        hp = maxHp;
        damage = 10f;
        moveSpeed = 10f;
        rotSpeed = 40f;

        State = Define.PlayerState.Idle;
    }

    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        dir = (Vector3.right * horizontal) + (Vector3.forward * vertical);
       
        if (State == Define.PlayerState.Die)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            State = Define.PlayerState.Attack;
        }

        switch (State)
        {
            case Define.PlayerState.Idle:
                UpdateIdle();
                break;
            case Define.PlayerState.Move:
                UpdateMove();
                break;
            case Define.PlayerState.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        if(dir != Vector3.zero)
            State = Define.PlayerState.Move;
    }

    void UpdateMove()
    {
        if (dir != Vector3.zero)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }
        else
        {
            State = Define.PlayerState.Idle;
        }

        //rb.velocity = dir * moveSpeed;
    }

    void UpdateAttack()
    {
        if(!isAction)
            StartCoroutine(CoWaitForAnimState_Idle());
    }

    void OnAttack()
    {
        Instantiate(bulletOrigin, firePos.position, Quaternion.identity);
    }

    IEnumerator CoWaitForAnimState_Idle()
    {
        isAction = true;

        while(true)
        {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.2f)
                break;

            yield return null;
        }

        isAction = false;
        State = Define.PlayerState.Idle;
        Debug.Log("Idle");
    }
    
}
