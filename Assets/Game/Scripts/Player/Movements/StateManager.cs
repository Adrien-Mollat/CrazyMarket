using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        private GameObject activeModel;
        //public Transform Player_Body;

        [Header("Inputs")]
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public Vector3 moveDir;

        [Header("Stats")]

        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;

        //Anim name

        [Header("States")]

        //bool

        [HideInInspector]
        public bool onGround, is_run, is_dodge;

        [HideInInspector]
        public Rigidbody rigid;


        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;

        public void Init()
        {
            activeModel = this.gameObject;
            rigid = GetComponent<Rigidbody>();
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);
        }

        private void movement_manager(float targetSpeed)
        {
            //if (onGround)
            rigid.velocity = moveDir * (targetSpeed * moveAmount);

            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;
        }

        public void FixedTick(float d)
        {
            float targetSpeed = moveSpeed;
            delta = d;

            rigid.drag = (moveAmount > 0 || onGround == false) ? 0 : 4;

            //Run speed manager

            if (Input.GetKey(KeyCode.LeftShift))
                is_run = true;
            else
                is_run = false;
            if (is_run)
                targetSpeed = runSpeed;

            movement_manager(targetSpeed);
        }

        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();
        }

        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;
            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);
            if(Physics.Raycast(origin,dir,out hit,dis,ignoreLayers))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }
                return r;
        }
    }
}
