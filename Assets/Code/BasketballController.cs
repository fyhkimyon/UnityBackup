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

    //skills
    public static bool isCooling;
    public bool inSkill;
    public float skillDuring;
    public float speedAdd = 1.0f;
    public int missSub = 0;

    private void Start()
    {
        skillDuring = 0;
        isCooling = false;
        inSkill = false;
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
        //Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //transform.position += direction * MoveSpeed * Time.deltaTime * speedAdd;
        //transform.LookAt(transform.position + direction);
        Vector3 dv = Target.parent.position - transform.position;
        Vector3 mp = transform.position;
        mp.y = 0;
        dv.y = 0;
        dv = dv.normalized;
        Vector3 direction = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            direction += dv;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction -= dv;
        }

        float angle = Mathf.PI / 2;
        Vector3 dv2 = new Vector3(dv.x * Mathf.Cos(angle) - dv.z * Mathf.Sin(angle), 0, dv.x * Mathf.Sin(angle) + dv.z * Mathf.Cos(angle));

        if (Input.GetKey(KeyCode.A))
        {
            direction += dv2;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction -= dv2;
        }

        transform.position += direction * MoveSpeed * Time.deltaTime * speedAdd;

        transform.LookAt(Target.parent.position);
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
                level -= missSub;
                if (level < 0) level = 0;
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
                UiController.bar.fillAmount = 0f;
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
        if (speedAdd > 1.0)
        {
            speedAdd -= Time.fixedDeltaTime / (1 + 8 * speedAdd);
            if (speedAdd <= 1.0f) speedAdd = 1.0f;
        }

        if (!IsBallInHands && !IsBallFlying)
        {
            stuckTime += Time.fixedDeltaTime;
            if (stuckTime > 5f)
            {
                IsBallInHands = true;
                stuckTime = 0;
            }
        }

        if (IsBallInHands && Input.GetKey(KeyCode.Space))
        {
            spacePressTime += Time.fixedDeltaTime;
            UiController.bar.fillAmount += Time.fixedDeltaTime / 1.8f;
            if (spacePressTime >= 1.8f)
            {
                spacePressTime = 1.8f;
            }
            if (UiController.bar.fillAmount >= 1.0)
            {
                UiController.bar.fillAmount = 1.0f;
            }
        }

        if (!isCooling)
        {
            if (Input.GetKey(KeyCode.J))
            {
                skillDuring = 10;
                missSub = 1;
                UiController.skills.text = "shooting add";
                inSkill = true;
                isCooling = true;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                skillDuring = 10;
                speedAdd = 1.8f;
                UiController.skills.text = "speed up";
                inSkill = true;
                isCooling = true;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                UiController.time += 10.0f;
                inSkill = true;
                isCooling = true;
            }
        }

        if (inSkill)
        {
            skillDuring -= Time.fixedDeltaTime;
            if (skillDuring <= 0)
            {
                skillDuring = 0;
                inSkill = false;
                missSub = 0;
                UiController.skills.text = "no skill";
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
