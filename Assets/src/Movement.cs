using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    Vector2 direction;
    Vector3 position;
    bool updatePosition = false;
    double lastUpdate;
    public Vector2 Direction
    {
        set
        {
            direction = value.sqrMagnitude < 1f ? value : value.normalized;
            SetDirection(direction, transform.position, NetworkTime.time);
        }
    }
    public bool Running;
    Animator animator;
    Rigidbody2D body;
    float stuckTime = 0f;
    public bool IsStuck => stuckTime > .5f;

    float facing = -1f;


    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    [ClientRpc(channel = Channels.Unreliable)]
    void SetDirection(Vector2 direction, Vector3 position, double time)
    {
        if (isServer)
            return;
        if (time > lastUpdate)
        {
            lastUpdate = time;
            this.position = Vector3.Lerp(position, this.position, .2f);
            updatePosition = true;
            this.direction = direction;
        }
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
        stuckTime = transform.position == position ? 0f : stuckTime + Time.fixedDeltaTime;

        if (isServer)
            position = transform.position;
        if (direction != Vector2.zero)
        {
            if (Mathf.Sign(direction.x) != facing)
            {
                facing = Mathf.Sign(direction.x);
                StartCoroutine(Flip());
            };
        }
        position += (Vector3)direction * Time.fixedDeltaTime * (Running ? runSpeed : walkSpeed);
        body.MovePosition(position);
        if (isServer)
        {
            animator.SetBool("Walking", direction.sqrMagnitude > .1f);
            animator.SetBool("Running", direction.sqrMagnitude > .1f && Running);
        }
    }
}
