using System;
using UnityEngine;

namespace TasiYokan.SpriteAnimation
{
    public class WaitForAnimEnd : CustomYieldInstruction
    {
        private Animator m_animator;
        private string m_stateName;

        public WaitForAnimEnd(Animator _animator, string _stateName)
        {
            m_animator = _animator;
            m_stateName = _stateName;
        }

        public override bool keepWaiting
        {
            get
            {
                AnimatorStateInfo currentInfo = m_animator.GetCurrentAnimatorStateInfo(0);

                //return currentInfo.IsName(m_stateName)
                //    && ((currentInfo.loop == false && currentInfo.normalizedTime < 1.0f)
                //        || currentInfo.loop == true);

                // If you want to change the animation even it's looping, uncomment the following,
                // this will jump out everytime a loop finished.
                // we need 2*delta cause
                // 1. when the check performing, the animation has not applied(that means, at the end of current frame, animation should at 
                // "currentInfo.normalizedTime + Time.smoothDeltaTime*currentInfo.speed")
                // 2. all the check will be applyed at next frame.(that means, at that time, the animation should at 
                // "currentInfo.normalizedTime + 2*Time.smoothDeltaTime*currentInfo.speed")
                return currentInfo.IsName(m_stateName)
                    && currentInfo.normalizedTime < 1f; // + 2*Time.smoothDeltaTime*currentInfo.speed < 1f;
            }
        }
    }
}