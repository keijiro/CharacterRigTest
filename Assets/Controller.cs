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
    [SerializeField] uint _seed = 100;

    void Update()
    {
        // PRNG
        var seed = _seed;
        var hash = new XXHash(seed++);

        // Time parameters
        var t_walk = Time.time * 10;
        var t_noise = hash.Float3(0.9f, 1.1f, 1) * Time.time * 0.3f + 100;

        // Hip targets
        {
            var walk = math.sin(t_walk * 2 + math.PI * 0.25f) * 0.02f;
            var und = Noise.Float3(t_noise * 8, seed++) * 0.02f;
            _hip.localPosition = und + math.float3(0, walk, 0);
            _hip.localRotation = quaternion.RotateY(math.sin(t_walk) * 0.2f);
        }

        // Head targets
        _head.localPosition = Noise.Float3(t_noise * 1.4f, seed++) * 0.75f;

        // Hand targets
        {
            var phi = math.sin(t_walk) * 0.5f;
            _handL.parent.localRotation = quaternion.RotateY(-phi);
            _handR.parent.localRotation = quaternion.RotateY(+phi);
        }

        // Leg targets
        {
            var y = math.cos(t_walk) * -0.06f;
            var z = math.sin(t_walk) * 0.12f;
            _legL.localPosition = math.float3(0, math.min(0, -y), +z);
            _legR.localPosition = math.float3(0, math.min(0, +y), -z);
        }
    }
}
