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

    #region Private members

    // Input / normalized speed vector
    (float2 input, float2 normalized) _speed;

    // Animation driver
    WalkAnimation _anim;

    // 
    float2 ProjectToUnitCircle(float2 v)
      => v / math.max(1, math.length(v));

    #endregion

    #region Player input callback

    public void OnPlayerMove(InputAction.CallbackContext ctx)
      => _speed.input = ProjectToUnitCircle(ctx.ReadValue<Vector2>());

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
        var vel = _speed.normalized * _maxSpeed;
        vel *= math.lerp(0.5f, 1, math.length(_speed.normalized));
        pos.xz += vel * Time.deltaTime;
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
