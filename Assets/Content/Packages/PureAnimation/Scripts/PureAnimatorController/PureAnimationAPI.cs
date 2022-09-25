using System;
using Common;
using UnityEngine;

namespace PureAnimator
{
    public static class PureAnimationAPI
    {
        public static void PlayCurveJump(
            Transform transform,
            JumpAnimationData data,
            Action exitCommand)
        {
            var startPosition = transform.position;
            var pureAnimatorJump = Services<PureAnimatorController>.Get().GetPureAnimator();
            pureAnimatorJump.Play(data.duration, progress =>
            {
                if (progress > 1) progress = 1;
                Vector3 position =
                    Vector3.Scale(new Vector3(0, data.heightJump * data.curve.Evaluate(progress), 0),
                        data.directionJump);
                return new TransformChanges(position);
            }, () => { });
            
            var pureAnimatorMove = Services<PureAnimatorController>.Get().GetPureAnimator();
            pureAnimatorMove.Play(data.duration,
                progress =>
                {
                    transform.position = Vector3.Lerp(startPosition,
                        data.endMove,
                        progress) + pureAnimatorJump.LastChanges.Value;
                    return default;
                },
                exitCommand
            );
        }
        
        public static void PlayCurveScale(Transform transform,
            ScaleAnimationData data,
            Action exitCommand)
        {
            var pureAnimatorMove = Services<PureAnimatorController>.Get().GetPureAnimator();
            pureAnimatorMove.Play(data.duration,
                progress =>
                {
                    transform.localScale = data.offsetScale + data.axisScale * data.curve.Evaluate(progress);
                    return default;
                },
                exitCommand
            );
        }
    }
}
