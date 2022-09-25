using UnityEngine;

namespace PureAnimator
{
    public class JumpAnimationData
    {
        public float duration = 1;
        public AnimationCurve curve = default;
        public Vector3 directionJump = Vector3.up;
        public float heightJump = 1;
        public Vector3 endMove = Vector3.zero;
    }
}