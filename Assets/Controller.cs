using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

sealed class Controller : MonoBehaviour
{
    [SerializeField] Transform _head = null;
    [SerializeField] Transform _body = null;
    [SerializeField] Transform _handL = null;
    [SerializeField] Transform _handR = null;
    [SerializeField] Transform _legL = null;
    [SerializeField] Transform _legR = null;
    [SerializeField] uint _seed = 100;

    float _hipHeight;

    void Start()
      => _hipHeight = _body.localPosition.z;

    void Update()
    {
        // PRNG
        var seed = _seed;
        var hash = new XXHash(seed++);

        // Time parameters
        var t_walk = Time.time * 10;
        var t_noise = hash.Float3(0.9f, 1.1f, 1) * Time.time * 0.3f + 100;

        // Hip
        {
            var vert = 0.95f * _hipHeight;
            var walk = math.sin(t_walk * 2 + math.PI * 0.25f) * 0.02f;
            var und = Noise.Float3(t_noise * 0.6f, seed++) * 0.03f;
            _body.localPosition = und + math.float3(0, 0, vert + walk);
        }

        // Head
        _head.localRotation =
          Noise.Rotation(t_noise * 0.3f, math.float3(0.3f, 0.5f, 0.3f), seed++);

        // Hand IK
        {
            var phi = math.sin(t_walk) * 0.5f;
            _handL.parent.localRotation = quaternion.RotateY(-phi);
            _handR.parent.localRotation = quaternion.RotateY(+phi);
        }

        // Leg IK
        {
            var y = math.cos(t_walk) * -0.06f;
            var z = math.sin(t_walk) * 0.12f;
            _legL.localPosition = math.float3(0, math.max(0, -y), +z);
            _legR.localPosition = math.float3(0, math.max(0, +y), -z);
        }
    }
}
