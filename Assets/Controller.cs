using UnityEngine;
using Unity.Mathematics;
using Klak.Math;

sealed class Controller : MonoBehaviour
{
    [SerializeField] Animator _animator = null;
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

        var lshoulder = _animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        Debug.Log(lshoulder);
    }

    void Update()
    {
        const uint seed = 0x123;
        var hash = new XXHash(seed);
        var t1 = 10 + hash.Float3(0.9f, 1.1f, 1) * Time.time;
        var t2 = 10 + hash.Float3(0.9f, 1.1f, 2) * Time.time;
        var t3 = 10 + hash.Float3(0.9f, 1.1f, 2) * Time.time;

        var t = Time.time * 14;

        var bpos = _defaults.body.pos * math.float3(0, 0.95f, 0) + Noise.Float3(t1, seed + 1) * 0.03f;
        var brot = math.mul(Noise.Rotation(t1, math.float3(0.1f, 0.8f, 0.2f), seed + 2), _defaults.body.rot);
        var handL = _defaults.handL * math.float3(0.5f, 0.5f, 0.5f) + math.float3(0, 0.08f, -0.08f) + Noise.Float3(t2, seed + 3) * 0.05f;
        var handR = _defaults.handR * math.float3(0.5f, 0.5f, 0.5f) + math.float3(0, 0.08f, -0.08f) + Noise.Float3(t3, seed + 4) * 0.05f;

        bpos.y += math.sin(t * 2 + math.PI * 0.25f) * 0.02f - 0.00f;

        var body = new RigidTransform(brot, bpos);

        _body.position = bpos;
        _body.rotation = brot;

        body = _defaults.body;
        //_handL.position = math.transform(body, handL);
        //_handR.position = math.transform(body, handR);
        //_legL .position = math.transform(body, _defaults.legL);
        //_legR .position = math.transform(body, _defaults.legR);

        _handL.parent.localRotation = quaternion.AxisAngle(math.float3(0, 1, 0), -math.sin(t) * 0.5f);
        _handR.parent.localRotation = quaternion.AxisAngle(math.float3(0, 1, 0),  math.sin(t) * 0.5f);
        _legL.localPosition = math.float3(0, math.max(0, -math.cos(t)) * -0.06f, math.sin(t) *  0.12f);
        _legR.localPosition = math.float3(0, math.max(0, +math.cos(t)) * -0.06f, math.sin(t) * -0.12f);
    }
}
