using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

sealed class Controller : MonoBehaviour
{
    [SerializeField] Transform _hip = null;
    [SerializeField] Transform _head = null;
    [SerializeField] Transform _handL = null;
    [SerializeField] Transform _handR = null;
    [SerializeField] Transform _legL = null;
    [SerializeField] Transform _legR = null;
    [Space]
    [SerializeField] uint _seed = 100;
    [SerializeField, Range(0, 20)] float _walkSpeed = 10;
    [SerializeField, Range(0, 2)] float _armSwing = 0.5f;
    [SerializeField, Range(0, 0.5f)] float _stride = 0.12f;
    [SerializeField, Range(0, 0.2f)] float _footUp = 0.06f;
    [SerializeField, Range(0, 0.2f)] float _hipUp = 0.02f;
    [SerializeField, Range(0, 1)] float _hipShake = 0.2f;
    [SerializeField, Range(0, 0.2f)] float _noise = 0.02f;

    float _time;

    void Update()
    {
        // PRNG
        var seed = _seed;
        var hash = new XXHash(seed++);

        // Time parameters
        var t_noise = hash.Float3(0.9f, 1.1f, 1) * Time.time * 0.3f + 100;

        // Hip target
        {
            var walk = math.sin(_time * 2 + math.PI * 0.25f) * _hipUp;
            var und = Noise.Float3(t_noise * 8, seed++) * _noise;
            _hip.localPosition = und + math.float3(0, walk - 0.2f * _noise, 0);
            _hip.localRotation = quaternion.RotateY(math.sin(_time) * -_hipShake);
        }

        // Head target
        _head.localPosition = Noise.Float3(t_noise * 1.4f, seed++) * 0.5f;

        // Hand targets
        {
            var phi = math.sin(_time) * _armSwing;
            _handL.parent.localRotation = quaternion.RotateY(-phi);
            _handR.parent.localRotation = quaternion.RotateY(+phi);
        }

        // Leg targets
        {
            var y = math.cos(_time) * -_footUp;
            var z = math.sin(_time) * _stride;
            _legL.localPosition = math.float3(0, math.min(0, -y), +z);
            _legR.localPosition = math.float3(0, math.min(0, +y), -z);
        }

        // Time step
        _time += Time.deltaTime * _walkSpeed;
    }
}
