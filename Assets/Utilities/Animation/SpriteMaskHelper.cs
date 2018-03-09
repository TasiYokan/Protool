using UnityEngine;
using System.Collections;

namespace TasiYokan.SpriteAnimation
{
    public class SpriteMaskHelper : MonoBehaviour
    {
        public Sprite sprite;
        private SpriteMask m_mask;

        // Use this for initialization
        void Start()
        {
            m_mask = GetComponent<SpriteMask>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_mask.sprite != sprite)
                m_mask.sprite = sprite;
        }
    }
}