using UnityEngine;

sealed class Scroller : MonoBehaviour
{
    [SerializeField] float _speed = 1;

    Material _material;

    void Start()
      => _material = GetComponent<MeshRenderer>().material;

    void Update()
      => _material.mainTextureOffset =
          new Vector2(0, Time.time * _speed);
}
