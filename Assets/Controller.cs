using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

sealed class Controller : MonoBehaviour
{
    [SerializeField] Transform _body = null;
    [SerializeField] Transform _handL = null;
    [SerializeField] Transform _handR = null;
    [SerializeField] Transform _legL = null;
    [SerializeField] Transform _legR = null;

    (RigidTransform body,
     float3 handL, float3 handR,
     float3 legL, float3 legR) _defaults;

    void Start()
    {
        var body = new RigidTransform(_body.rotation, _body.position);
        var inv = math.inverse(body);
        _defaults.body  = body;
        _defaults.handL = math.transform(inv, _handL.position);
        _defaults.handR = math.transform(inv, _handR.position);
        _defaults.legL  = math.transform(inv, _legL .position);
        _defaults.legR  = math.transform(inv, _legR .position);
    }

    void Update()
    {
        const uint seed = 0x123;
        var hash = new XXHash(seed);
        var t1 = 10 + hash.Float3(0.9f, 1.1f, 1) * Time.time * 0.5f;
        var t2 = 10 + hash.Float3(0.9f, 1.1f, 2) * Time.time * 0.5f;
        var t3 = 10 + hash.Float3(0.9f, 1.1f, 2) * Time.time * 0.5f;

        var bpos = _defaults.body.pos * math.float3(0, 0.95f, 0) + Noise.Float3(t1, seed + 1) * 0.03f;
        var brot = math.mul(Noise.Rotation(t1, math.float3(0.1f, 0.8f, 0.2f), seed + 2), _defaults.body.rot);
        var handL = _defaults.handL * math.float3(0.5f, 0.5f, 0.5f) + math.float3(0, 0.08f, -0.08f) + Noise.Float3(t2, seed + 3) * 0.05f;
        var handR = _defaults.handR * math.float3(0.5f, 0.5f, 0.5f) + math.float3(0, 0.08f, -0.08f) + Noise.Float3(t3, seed + 4) * 0.05f;

        var body = new RigidTransform(brot, bpos);

        _body.position = bpos;
        _body.rotation = brot;

        body = _defaults.body;
        _handL.position = math.transform(body, handL);
        _handR.position = math.transform(body, handR);
        //_legL .position = math.transform(body, _defaults.legL);
        //_legR .position = math.transform(body, _defaults.legR);
    }
}
