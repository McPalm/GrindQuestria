using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    Vector2 direction;
    public Vector2 Direction { set => direction = value.sqrMagnitude < 1f ? value : value.normalized; }
    public bool Running;
    Animator animator;
    Rigidbody2D body;

    float facing = -1f;


    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    IEnumerator Flip()
    {
        for(float f = 0f; f < 1f; f += Time.deltaTime * 10f)
        {
            yield return null;
            transform.localScale = new Vector3(1f - f, 1f, 1f);
        }
        transform.localEulerAngles = new Vector3(0f, facing < 0f ? 0f : 180f, 0f);
        for (float f = 0f; f < 1f; f += Time.deltaTime * 10f)
        {
            yield return null;
            transform.localScale = new Vector3(f, 1f, 1f);
        }
        transform.localScale = Vector3.one;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(direction != Vector2.zero)
        {
            if (Mathf.Sign(direction.x) != facing)
            {
                facing = Mathf.Sign(direction.x);
                StartCoroutine(Flip());
            };
        }
        body.MovePosition(transform.position + (Vector3)direction * Time.fixedDeltaTime * (Running ? runSpeed : walkSpeed));
        animator.SetBool("Walking", direction.sqrMagnitude > .1f);
        animator.SetBool("Running", direction.sqrMagnitude > .1f && Running);
    }
}
