using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MovePlayerAnimations : MonoBehaviour {

        // Use this for initialization
        private StateManager state_manager;
        private Animator anim;




        void Start() {
            state_manager = this.GetComponent<StateManager>();
            anim = this.GetComponent<Animator>();
        }

        public void HandleMovementAnimations()
        {
            if (state_manager.vertical != 0 || state_manager.horizontal != 0)
                anim.SetBool("Walk", true);
            else
                anim.SetBool("Walk", false);
            if (state_manager.is_run && (state_manager.vertical != 0 || state_manager.horizontal != 0))
                anim.SetBool("Run", true);
            else
                anim.SetBool("Run", false);
        }
    }
}
