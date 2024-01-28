using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimationReceive : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool resetStateOnReceive = false;


    void Start() {
        if(World.GetInstance().offlineMode) {
            this.enabled = false;
            return;
        }
        animator = gameObject.GetComponentInChildren<Animator>(true);
    }

    public void SetFloat(string property, float value) {
        animator.SetFloat(property, value);
    }

    public void SetAnimationProperty(int animId, float value) {
        if(animId >= 0 && animId < animator.parameters.Length) {
            if(resetStateOnReceive) {
                ClearAnimParams();
            }

            AnimatorControllerParameter anim = animator.parameters[animId];
            //Debug.Log("Updating animation: " + transform.name + " " + anim.name + "=" + value);

            switch(anim.type) {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(anim.name, value);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(anim.name, (int)value);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(anim.name, (int)value == 1);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(anim.name);
                    break;
            }
        }
    }

    public void ClearAnimParams() {
        for(int i = 0; i < animator.parameters.Length; i++) {
            AnimatorControllerParameter anim = animator.parameters[i];
            if(anim.type == AnimatorControllerParameterType.Bool) {
                animator.SetBool(anim.name, false);
            }
        }
    }
}
