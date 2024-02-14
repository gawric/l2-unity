﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : BaseAnimationController {
    private PlayerController _pc;
    private static PlayerAnimationController _instance;
    public static PlayerAnimationController Instance { get { return _instance; } }

    protected override void Initialize() {
        base.Initialize();

        if (_instance == null) {
            _instance = this;
        }
    }

    private void Update() {
        //UpdateAnimator();
    }
    
    private void UpdateAnimator() {
        SetFloat("speed", _pc.CurrentSpeed, false);

        /*Jump */
        if(InputManager.Instance.IsInputPressed(InputType.Jump) && IsCurrentState("Idle")) {
            CameraController.Instance.StickToBone = true;
            SetBool("IdleJump", true);
        } else {
            SetBool("IdleJump", false);
        }

        /* RunJump */
        if(IsNextState("RunJump")) {
            CameraController.Instance.StickToBone = true;
        }
        if(InputManager.Instance.IsInputPressed(InputType.Jump) && IsCurrentState("Run") || IsCurrentState("RunJump") && !IsAnimationFinished(0)) {
            CameraController.Instance.StickToBone = true;
            SetBool("RunJump", true);
        } else {
            SetBool("RunJump", false);
        }

        /* Run */
        if(IsNextState("Run")) {
            CameraController.Instance.StickToBone = false;
        }
        if((InputManager.Instance.IsInputPressed(InputType.Move) || _pc.RunningToTarget)
            && (IsCurrentState("Idle") || IsAnimationFinished(0)) && _pc.CanMove) {
            SetBool("Moving", true);
            
        } else {
            SetBool("Moving", false);
        }

        /* Sit */
        if(IsNextState("SitTransition")) {
            CameraController.Instance.StickToBone = true;
        }
        if(InputManager.Instance.IsInputPressed(InputType.Sit) && (IsCurrentState("Run") || IsCurrentState("Idle"))) {
            CameraController.Instance.StickToBone = true;
            _pc.CanMove = false;
            SetBool("Sit", true);
        } else {
            SetBool("Sit", false);
        }

        /* SitWait */

        if(IsNextState("SitWait")) {
            CameraController.Instance.StickToBone = true;
        }
        if(IsCurrentState("SitTransition") && IsAnimationFinished(0)) {
            CameraController.Instance.StickToBone = true;
            SetBool("SitWait", true);
        }


        /* Stand */
        if((InputManager.Instance.IsInputPressed(InputType.Sit) || InputManager.Instance.IsInputPressed(InputType.Move))
            && (IsCurrentState("SitWait"))) {
            CameraController.Instance.StickToBone = true;
            SetBool("Stand", true);
            SetBool("SitWait", false);
        } else {
            SetBool("Stand", false);
        }

        if(IsCurrentState("Stand") && IsAnimationFinished(0)) {
            CameraController.Instance.StickToBone = false;
            _pc.CanMove = true;
        }

        /* Idle */
        if(!InputManager.Instance.IsInputPressed(InputType.Move)
            && !_pc.RunningToTarget
            && (IsCurrentState("Run") || IsAnimationFinished(0) || IsCurrentState("Idle"))
            && !IsCurrentState("SitTransition")
            && !IsCurrentState("Sit")
            && !IsCurrentState("SitWait")) {
            CameraController.Instance.StickToBone = false;
            SetBool("Idle", true);
        } else {
            SetBool("Idle", false);
        }
    }

    private bool IsAnimationFinished(int layer) {
        return _animator.GetCurrentAnimatorStateInfo(layer).normalizedTime > 0.95f;
    }

    private bool IsCurrentState(string state) {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(state);
    }

    private bool IsNextState(string state) {
        return _animator.GetNextAnimatorStateInfo(0).IsName(state);
    }

    private void SetBool(string name, bool value) {
        if(_animator.GetBool(name) != value) {
            _animator.SetBool(name, value);
            if(!World.Instance.OfflineMode) {
                EmitAnimatorInfo(name, value ? 1 : 0);
            }
        }
    }

    private void SetFloat(string name, float value, bool share) {
        if(Mathf.Abs(_animator.GetFloat(name) - value) > 0.2f) {
            _animator.SetFloat(name, value);
            if(!World.Instance.OfflineMode && share) {
                EmitAnimatorInfo(name, value);
            }
        }
    }

    private void SetInteger(string name, int value) {
        if(_animator.GetInteger(name) != value) {
            _animator.SetInteger(name, value);
            if(!World.Instance.OfflineMode) {
                EmitAnimatorInfo(name, value);
            }
        }
    }

    private void SetTrigger(string name) {
        _animator.SetTrigger(name);
        if(!World.Instance.OfflineMode) {
            EmitAnimatorInfo(name, 0);
        }
    }

    private int SerializeAnimatorInfo(string name) {
        List<AnimatorControllerParameter> parameters = new List<AnimatorControllerParameter>(_animator.parameters);

        int index = parameters.FindIndex(a => a.name == name);

        return index;
    }

    private void EmitAnimatorInfo(string name, float value) {
        int index = SerializeAnimatorInfo(name);
        if(index != -1) {
            _animator.GetComponentInParent<NetworkTransformShare>().ShareAnimation((byte)index, value);
        }
    }
}

