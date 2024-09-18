﻿using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _lerpTargetPos;
    [SerializeField] private float _x, _y = 0;
    private Vector3 _targetPos;
    private LayerMask _collisionMask;

    [SerializeField] private Transform _target;

    [Header("Camera controls")]
    [SerializeField] private bool _smoothCamera = true;
    [SerializeField] private Vector3 _camOffset = new Vector3(0, 0.8f, 0);
    [SerializeField] private float _smoothness = 8f;
    [SerializeField] private float _camSpeed = 25f;

    [Header("Zoom controls")]
    [SerializeField] private float _minDistance = .5f;
    [SerializeField] private float _maxDistance = 10f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _camDistance = 2f;
    [SerializeField] private float _currentDistance = 0;

    [Header("Bone stickiness")]
    [SerializeField] private bool _stickToBone = false;
    [SerializeField] private Transform _rootBone;
    [SerializeField] private float _rootBoneHeight = 0;

    [SerializeField] private CameraCollisionDetection _collisionDetector;

    public Transform Target { get { return _target; } set { _target = value; } }

    public bool StickToBone { get { return _stickToBone; } set { _stickToBone = value; } }
    public float CurrentDistance { get { return _currentDistance; } }
    public float MaxDistance { get { return _maxDistance; } }


    private static CameraController _instance;
    public static CameraController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }

    private void Start()
    {
        _lerpTargetPos = Vector3.zero;
    }

    public void SetMask(LayerMask collisionMask)
    {
        this._collisionMask = collisionMask;
    }

    private void Update()
    {
        if (_target != null && _collisionDetector != null)
        {
            UpdateZoom();
        }
    }

    private void LateUpdate()
    {
        if (_target != null && _collisionDetector != null)
        {
            UpdateInputs();
            UpdatePosition();
        }
    }

    void FixedUpdate()
    {
        if (_target != null && _collisionDetector != null)
        {
            _collisionDetector.DetectCollision(_camDistance);
        }
    }

    public void SetTarget(GameObject go)
    {
        _target = go.transform;
        transform.position = _targetPos;

        _rootBone = _target.transform.FindRecursive(child => child.tag == "Root");
        _rootBoneHeight = _rootBone.position.y - _target.position.y;
        _collisionDetector = new CameraCollisionDetection(GetComponent<Camera>(), _target, _camOffset, _collisionMask);
    }

    public bool IsObjectVisible(Transform target)
    {
        RaycastHit hit;
        Vector3[] cameraClips = _collisionDetector.GetCameraViewPortPoints();
        bool visible = false;
        for (int i = 0; i < cameraClips.Length; i++)
        {
            if (!Physics.Linecast(cameraClips[i], target.position + 1f * Vector3.up, out hit, _collisionMask))
            {
                visible = true;
                break;
            }
        }

        if (visible == false)
        {
            return false;
        }

        Vector3 viewPort = Camera.main.WorldToViewportPoint(target.position);
        bool insideView = viewPort.x <= 1 && viewPort.x >= 0 && viewPort.y <= 1 && viewPort.y >= 0 && viewPort.z >= -0.2f;
        return insideView;
    }

    private void UpdateInputs()
    {
        if (InputManager.Instance.TurnCamera)
        {
            Vector2 mouseAxis = InputManager.Instance.CameraAxis;
            // float logX = Mathf.Pow(Mathf.Abs(mouseAxis.x), 0.7f);
            // float logY = Mathf.Pow(Mathf.Abs(mouseAxis.y), 0.7f);
            // float diffX = mouseAxis.x < 0 ? -logX : logX;
            // float diffY = mouseAxis.y < 0 ? -logY : logY;
            // _x += mouseAxis.x == 0 ? 0 : diffX * _camSpeed * Time.deltaTime;
            // _y -= mouseAxis.y == 0 ? 0 : diffY * _camSpeed * Time.deltaTime;

            _x += Input.GetAxis("Mouse X") * _camSpeed;
            _y -= Input.GetAxis("Mouse Y") * _camSpeed;
            _y = ClampAngle(_y, -90, 90);
        }
    }

    private void UpdateZoom()
    {
        float scrollAxis = InputManager.Instance.ZoomAxis;
        _camDistance = Mathf.Clamp(_camDistance - scrollAxis * _zoomSpeed * 0.1f, _minDistance, _maxDistance);
    }

    private void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(_y, _x, 0);
        transform.rotation = rotation;

        if (_collisionDetector.AdjustedDistance > Vector3.Distance(_targetPos + _camOffset, transform.position))
        {
            _currentDistance += (_collisionDetector.AdjustedDistance - _currentDistance) / 0.2f * Time.deltaTime;
        }
        else
        {
            _currentDistance -= (_currentDistance - _collisionDetector.AdjustedDistance) / 0.075f * Time.deltaTime;
        }

        float boneOffset = 0;
        if (_stickToBone)
        {
            boneOffset = _rootBone.position.y - _target.position.y - _rootBoneHeight;
        }

        _targetPos = new Vector3(_camOffset.x, boneOffset + _camOffset.y, _camOffset.z) + _target.position;

        if (_smoothCamera)
        {
            if (_lerpTargetPos == Vector3.zero)
            {
                _lerpTargetPos = _targetPos;
            }
            _lerpTargetPos = Vector3.Lerp(_lerpTargetPos, _targetPos, _smoothness * Time.deltaTime);
        }
        else
        {
            _lerpTargetPos = _targetPos;
        }

        Vector3 adjustedPosition = rotation * (Vector3.forward * -_currentDistance) + _lerpTargetPos;

        transform.position = adjustedPosition;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}