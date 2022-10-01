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
        var pos = (float3)transform.position;
        var vel = math.length(pos - _prev) / Time.deltaTime;

        _accum += _emissionRate.Evaluate(vel);

        _fx.Emit((int)_accum);
        _accum -= math.floor(_accum);

        _prev = pos;
    }

    #endregion
}
