using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Krivodeling.UI.Effects
{
    public class UIBlur : MonoBehaviour
    {
        private Material material;

        public Color color = Color.white;

        public float intensity
        {
            get => _intensity;
            set
            {
                _intensity = Mathf.Clamp01(value);
                material?.SetFloat("_Intensity", _intensity);
            }
        }
        [SerializeField] [Range(0f, 1f)] private float _intensity;
        [Range(0f, 1f)] public float multiplier = 0.15f;

        private void OnValidate()
        {
            SetBlurInEditor();
        }

        private void SetBlurInEditor()
        {
            Material m = GetComponent<Image>().material;

            m.SetColor("_Color", color);
            m.SetFloat("_Intensity", intensity);
            m.SetFloat("_Multiplier", multiplier);
        }

        private void Start()
        {
            SetComponents();
            SetBlur(color, intensity, multiplier);
        }

        private void SetComponents()
        {
            material = GetComponent<Image>().materialForRendering;
        }

        public void SetBlur(Color color, float intensity, float multiplier)
        {
            material.SetColor("_Color", color);
            material.SetFloat("_Intensity", intensity);
            material.SetFloat("_Multiplier", multiplier);
        }
    }
}
