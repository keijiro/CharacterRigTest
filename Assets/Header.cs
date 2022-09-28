using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

sealed class Header : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Transform _constraint = null;
    [SerializeField] Transform _target = null;
    [SerializeField] float _accel = 3;

    #endregion

    #region Private variables

    float3 _tween;

    #endregion

    #region MonoBehaviour implementation

    void Start()
      => _tween = _target.position;

    void Update()
      => _constraint.position = _tween =
           ExpTween.Step(_tween, _target.position, _accel);

    #endregion
}
