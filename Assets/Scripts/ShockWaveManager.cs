using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoulSpliter
{
    public class ShockWaveManager : MonoBehaviour
    {
        [SerializeField] private float _shockWaveTime = 0.75f;

        private Coroutine _shockWaveCoroutine;
        private Material _material;

        private static int _waveDistFromCenter = Shader.PropertyToID("_WaveDistFromCenter");

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        private void Update()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CallShockwave();
            }
        }

        public void CallShockwave()
        {
            _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        }

        private IEnumerator ShockWaveAction(float startPos, float endPos)
        {
            _material.SetFloat(_waveDistFromCenter, startPos);
            float lerpedAmount = 0f;
            float elaspedTime = 0f;

            while(elaspedTime < _shockWaveTime)
            {
                elaspedTime += Time.deltaTime;

                lerpedAmount = Mathf.Lerp(startPos, endPos, (elaspedTime / _shockWaveTime));
                _material.SetFloat(_waveDistFromCenter, lerpedAmount);

                yield return null;
            }

        }
    }
}
