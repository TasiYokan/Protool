using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TasiYokan.SpriteAnimation
{
    /// <summary>
    /// A custom Animator which can easily override different sets of animationClips in an animator.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PolyAnimator : MonoBehaviour
    {
        public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
        {
            public AnimationClipOverrides(int _capacity) : base(_capacity) { }

            public AnimationClip this[string _name]
            {
                get
                {
                    return this.Find(x => x.Key.name.Equals(_name)).Value;
                }
                set
                {
                    int index = this.FindIndex(x => x.Key.name.Equals(_name));
                    if (index != -1)
                        this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
                }
            }
        }

        /// <summary>
        /// Each includes a set of animations that needs to override.
        /// </summary>
        public class AnimSet : Dictionary<string, AnimationClip>
        {
            public AnimSet() : base() { }
            public AnimSet(int _capacity) : base(_capacity) { }
        }


        private Animator m_animator;
        private AnimatorOverrideController m_overrideController;
        /// <summary>
        /// A dictionary has several sets of animations
        /// </summary>
        private Dictionary<string, AnimSet> m_animDict;
        private AnimationClipOverrides m_clipOverrides;
        private string m_currentSetName;

        public AnimatorOverrideController OverrideController
        {
            get
            {
                return m_overrideController;
            }

            private set
            {
                m_overrideController = value;
            }
        }

        public string CurrentSetName
        {
            get
            {
                return m_currentSetName;
            }

            private set
            {
                m_currentSetName = value;
            }
        }

        public Animator RealAnimator
        {
            get
            {
                return m_animator ?? GetComponent<Animator>();
            }
        }

        public Dictionary<string, AnimSet> AnimDict
        {
            get
            {
                if (m_animDict == null)
                    m_animDict = new Dictionary<string, AnimSet>();
                return m_animDict;
            }

            private set
            {
                m_animDict = value;
            }
        }

        void Awake()
        {
            OverrideController = new AnimatorOverrideController(RealAnimator.runtimeAnimatorController);
            RealAnimator.runtimeAnimatorController = OverrideController;

            // By default, we will make a override for all state in the controller.
            m_clipOverrides = new AnimationClipOverrides(OverrideController.overridesCount);
            OverrideController.GetOverrides(m_clipOverrides);
        }

        public void InitAnimSets(Dictionary<string, AnimSet> _dict)
        {
            m_animDict = _dict;
        }

        /// <summary>
        /// <para>Using another set of animations to override current set.</para>
        /// _setName should be one in the AnimDict,
        /// otherwise we assume there's no available set of anims to switch
        /// </summary>
        /// <param name="_setName"></param>
        public void OverrideWith(string _setName, bool _isForce = false)
        {
            // Stop if clips haven't been initialized
            // or we didn't have the animSet in AnimDict
            if (m_clipOverrides == null
                || AnimDict.ContainsKey(_setName) == false)
                return;

            AnimatorStateInfo currentInfo = RealAnimator.GetCurrentAnimatorStateInfo(0);

            if (_isForce)
            {
                // LATER: Can we find a more elegant way or the native function to do this?
                foreach (KeyValuePair<AnimationClip, AnimationClip> pair in m_clipOverrides)
                {
                    m_clipOverrides[pair.Key.name] = null;
                }
            }
            AnimSet targetSet = AnimDict[_setName];
            foreach (string clipName in targetSet.Keys)
            {
                m_clipOverrides[clipName] = targetSet[clipName];
            }
            OverrideController.ApplyOverrides(m_clipOverrides);

            RealAnimator.Play(currentInfo.shortNameHash, 0, currentInfo.normalizedTime);
            CurrentSetName = _setName;
        }

        /// <summary>
        /// We can play a new set of anims to override animclips based on _animSet.Key
        /// </summary>
        /// <param name="_setName"></param>
        /// <param name="_animSet"></param>
        /// <param name="_isForce"></param>
        public void AddictivelyOverrideWith(string _setName, AnimSet _animSet, bool _isForce = false)
        {
            if (AnimDict.ContainsKey(_setName) == false)
                AnimDict.Add(_setName, _animSet);

            OverrideWith(_setName, _isForce);
        }
    }
}