using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour {

    public float MoveSpeed = 10;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    // variables
    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;
    public static int score = 0;
    public static int rewardScore = 0;
    private Vector3 delta;
    private Vector3 hoopPosition;
    private Vector3 miss;
    private int level;
    private float stuckTime;
    public static bool calcScore;
    public float spacePressTime;

    private void Start()
    {
        spacePressTime = 0;
        stuckTime = 0f;
        level = 0;
        score = 0;
        rewardScore = 0;
        calcScore = false;
        hoopPosition = new Vector3(0f, 0f, 8f);
    }

    // Update is called once per frame
    void Update() 
    {    
        // walking
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        // ball in hands
        if (IsBallInHands) 
        {
            // hold over head
            if (Input.GetKey(KeyCode.Space)) {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;
                delta = transform.position - hoopPosition;
                delta.y = 0;
                // look towards the target
                transform.LookAt(Target.parent.position);
            }

            // dribbling
            else 
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space)) 
            {
                IsBallInHands = false;
                IsBallFlying = true;
                float distance = delta.sqrMagnitude;
                if (distance < 20 || distance > 400) level = 4;
                else if (distance < 30 || distance > 300) level = 3;
                else if (distance < 100) level = 0;
                else if (distance < 180) level = 1;
                else level = 2;
                if (distance < 300) rewardScore = 2;
                else if (distance < 450) rewardScore = 3;
                else rewardScore = 4;
                System.Random rng = new System.Random();
                float missx = (float)(rng.NextDouble() - 0.5f) * level;
                float missy = ((float)rng.NextDouble() - (spacePressTime - 0.6f)) * 0.5f * level;
                float missz = (float)(rng.NextDouble() - 0.5f - (spacePressTime - 0.6f) * 0.5f) * level;
                miss = new Vector3(missx, missy, missz);
                //Debug.Log("distance:" + distance);
                T = 0;
                spacePressTime = 0;
            }
        }

        // ball in the air
        if (IsBallFlying) 
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc + miss;

            // moment when ball arrives at the target
            if (t01 >= 1) 
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    // when ball stack in the hoop
    private void FixedUpdate()
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            stuckTime += Time.fixedDeltaTime;
            if (stuckTime > 5f)
            {
                IsBallInHands = true;
                stuckTime = 0;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            spacePressTime += Time.fixedDeltaTime;
            if (spacePressTime >= 1.8f)
            {
                spacePressTime = 1.8f;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!IsBallInHands && !IsBallFlying) 
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            stuckTime = 0;
        }
    }
}
