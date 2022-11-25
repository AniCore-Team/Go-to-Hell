#pragma warning disable 649
using System;
using UnityEngine;

namespace Sources
{
    [Serializable]
    public class AudioData
    {
        [SerializeField] private AudioClip _clip;
        [Range(0f, 1f)] [SerializeField] private float _volume = 1f;

        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }
}