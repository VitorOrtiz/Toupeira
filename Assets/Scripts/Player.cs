using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [HideInInspector]
    public Vector3 velocity;
    public float accel;
    public float Deaccel;
    public float speed;

    float currentangle;
    public float currentspeed;
    float targetangle;
    float targetspeed;
    float rotatespeed;

    public bool Active;
    
    public bool Dead;
    
    bool Istouching;
    bool attack;
    bool nearBottom;
    public float BottomX;
    #region CollisionCheck

    public int Checkcount;
    public float ModelLength;
    public float ModelWidth;
    #endregion
    #region Singleton
    public static Player player;

    private void Awake()
    {
        player = this;
        
    }
    #endregion
    private bool DebugMode;
    // Use this for initialization
    void Start () {
        velocity = transform.right;
        targetspeed = speed;
        currentspeed = targetspeed;
        rotatespeed = .03f;
        Active = true;
    }

    private void Update()
    {
        if(Active)
        Movement();
        // currentspeed = speed;
        if (Input.GetKeyDown(KeyCode.F1))
            DebugMode = !DebugMode;
    }
    void FixedUpdate () {
        if (Active && !Dead)
        {
            velocity = transform.right;
            if (currentspeed != targetspeed) //
            {
                currentspeed += SetSpeed(currentspeed, targetspeed, accel);
                //currentspeed = Mathf.Lerp(currentspeed, targetspeed, .1f);
                // Debug.Log(currentspeed +" "+ targetspeed);
            }
            transform.Translate(velocity * currentspeed * Time.deltaTime);
            Rotate();
            if(!DebugMode)
            Collision();
            if (transform.position.y <= -7)
            {
                transform.position += Vector3.down * 3;
                Die();
            }
        }
        
	}
    
    void Movement()
    {
        nearBottom = transform.position.y < BottomX;
        if (Input.GetMouseButton(0))
        {
            
            Istouching = true;
            
            attack = true;
            targetspeed = speed;
            
            if(!nearBottom)
            {
                targetangle = 315;
                rotatespeed = .03f;
            }
              else if (nearBottom)
            {
                targetangle = 0;
                rotatespeed = .1f;
            }
        }
      
        if(Input.GetMouseButtonUp(0))
        {
            Istouching = false;
        }
        if (DistanttoSurface() )
        {
            if (!Istouching)
            {
                if (attack)
                {
                    targetspeed = speed * 3;
                    rotatespeed = .03f;
                }
                else
                {
                    rotatespeed = .03f / (currentspeed - 1);
                    targetspeed = speed;
                }
                targetangle = 45;
            }
        }
        else if (!DistanttoSurface())
        {
            rotatespeed = .02f;
            attack = false;
            targetspeed = speed *2f;
            targetangle = 315;
        }
    }

    void Collision()
    {
        GameObject coll = CheckCol();
        if (coll == null)
            return;
        if(coll.tag == "Enemy")
        {
            coll.GetComponent<Enemy_man>().Die();
        }
        else if(coll.tag == "Drill" || coll.tag == "Rock")
        {
            Die();
        }
    }
    void Die()
    {
        Active = false;
        Dead = true;

    }

    bool DistanttoSurface()
    {
        Vector3 Origin = transform.position + velocity;
        Origin += transform.up * ModelWidth;
        return Physics.Raycast(transform.position +velocity, Vector3.up, 1 << 9);
       // {
            //float dist = Vector2.Distance(new Vector2(hit.point.x, hit.point.y), new Vector2(transform.position.x, transform.position.y));
           // return true;
      //  }
       // return false;

    }
    void Rotate()
    {
        float curangle = transform.eulerAngles.z;
        if (curangle != targetangle)
        {
          //  Quaternion newrotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, curangle + (targetangle - curangle) * .5f);
            float newangle = Mathf.LerpAngle(curangle, targetangle, rotatespeed);
            
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y,newangle);
        }
    }
    float SetSpeed(float CurrentSpeed, float TargetSpeed, float Deacceleration)
    {    
        float dif = CurrentSpeed - TargetSpeed;
        float x = Deacceleration * Time.deltaTime * (dif < 0 ? 1 : -1);

        if (Mathf.Abs(dif) < Mathf.Abs(x))
            return dif;
        return x;
    }
    GameObject CheckCol()
    {
        int count =Checkcount;
        Vector3 dir = transform.right;

        for (int i = 0;i < count;i++)
        {
            Vector3 origin = transform.position + dir * (ModelLength/2) ; // multiplicar por distancia até a ponta do modelo
           // float mult = 1 -  * i;
            origin += transform.up* ModelWidth *(i != 0?1 -2f/(count-1) * i:1);
            float CheckL = currentspeed / 30 + ModelLength/2;
            Vector3 newdirection = origin + dir *CheckL;
            Debug.DrawLine(origin, newdirection);
            RaycastHit hit;
            if (Physics.Raycast(origin,newdirection,out hit,CheckL,1<<10))
                return hit.transform.gameObject;
        }
        return null;
    }

    public void ResetAll()
    {
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        targetangle = 0;
        rotatespeed = .03f;
        velocity = transform.right;
        targetspeed = speed;
        currentspeed = targetspeed;
        attack = false;
        Dead = false;
        
    }
    public void Activate()
    {
        Active = true;
    }
}
