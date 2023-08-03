using System.Collections;
using UnityEngine;


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

        public void CallShockwave()
        {
            _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        }

        private IEnumerator ShockWaveAction(float startPos, float endPos)
        {
            _material.SetFloat(_waveDistFromCenter, startPos);
            float elaspedTime = 0f;

            while(elaspedTime < _shockWaveTime)
            {
                elaspedTime += Time.deltaTime;

                float lerpedAmount = Mathf.Lerp(startPos, endPos, (elaspedTime / _shockWaveTime));
                _material.SetFloat(_waveDistFromCenter, lerpedAmount);

                yield return null;
            }

        }
    }
}
