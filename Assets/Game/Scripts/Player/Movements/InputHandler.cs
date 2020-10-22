using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour
    {

        float horizontal;
        float vertical;


        StateManager states;
        CameraManager camManager;
        MovePlayerAnimations player_anim;

        float delta;
        
        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();
            if ((player_anim = GetComponent<MovePlayerAnimations>()) == null)
                print("Please put the animation script");
            Cursor.visible = false;
            camManager = CameraManager.singleton;
            camManager.Init(this.transform);
        }

         
        void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            if (player_anim != null)
                player_anim.HandleMovementAnimations();
            states.FixedTick(delta);
            camManager.Tick(delta);
        }

        void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
        }
        



        void GetInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);
        }
    }
}
