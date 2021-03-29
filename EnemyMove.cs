using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsulecollider;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 3);
    }

    
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //지형체크 
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
            Turn();  
    }

    //재귀 함수 (딜레이 없이 재귀함수를 사용하는 것은 아주 위험함)
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

        //Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip sprite
        if(nextMove != 0)
        spriteRenderer.flipX = nextMove == 1;
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;

        //nextMove *=-1;
        CancelInvoke();
        Invoke("Think", 2);

    }

    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capsulecollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
        //Destroy
        Invoke("DeAcitve", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}

