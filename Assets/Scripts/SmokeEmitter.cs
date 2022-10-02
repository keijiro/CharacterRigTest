using UnityEngine;
using Unity.Mathematics;

sealed class SmokeEmitter : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] AnimationCurve
      _emissionRate = AnimationCurve.Linear(0, 0, 1, 1);

    #endregion

    #region Private variables

    ParticleSystem _fx;
    float3 _prev;
    float _accum;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _fx = GetComponent<ParticleSystem>();
        _prev = transform.position;
    }

    void LateUpdate()
    {
        var dt = Time.deltaTime;
        var pos = (float3)transform.position;
        var vel = math.length(pos - _prev) / dt;

        _accum += _emissionRate.Evaluate(vel) * dt;

        var count = (int)_accum;
        if (count > 0)
        {
            _fx.Emit(count);
            _accum -= count;
        }

        _prev = pos;
    }

    #endregion
}
