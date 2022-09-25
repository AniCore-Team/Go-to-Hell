using UnityEngine;

namespace PureAnimator
{
    public class ScaleAnimationData
    {
        public float duration = 1;
        public AnimationCurve curve = default;
        public Vector3 axisScale = Vector3.one;
        public Vector3 offsetScale = Vector3.one;
    }
}