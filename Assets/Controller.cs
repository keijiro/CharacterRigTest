using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Klak.Math;

sealed class Controller : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] float _maxSpeed = 3;
    [SerializeField] float _accel = 6;

    #endregion

    #region Private variables

    // Input / normalized speed vector
    (float2 input, float2 normalized) _speed;

    // Animation driver
    WalkAnimation _anim;

    #endregion

    #region Player input callback

    public void OnPlayerMove(InputAction.CallbackContext ctx)
      => _speed.input = math.normalizesafe((float2)ctx.ReadValue<Vector2>());

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => _anim = GetComponent<WalkAnimation>();

    void Update()
    {
        // Smooth input
        _speed.normalized =
          ExpTween.Step(_speed.normalized, _speed.input, _accel);

        // Position step
        var pos = (float3)transform.position;
        pos.xz += _speed.normalized * _maxSpeed * Time.deltaTime;
        transform.position = pos;

        // Rotation step
        if (math.length(_speed.normalized) > 0.1f)
        {
            var rot = transform.rotation;
            var dir = math.float3(_speed.normalized, 0).xzy;
            var target = quaternion.LookRotation(dir, math.float3(0, 1, 0));
            rot = ExpTween.Step(rot, target, _accel);
            transform.rotation = rot;
        }

        // Walk animation update
        _anim.Amplitude = math.length(_speed.normalized);
    }

    #endregion
}
