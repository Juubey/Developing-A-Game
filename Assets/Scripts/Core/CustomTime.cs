using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinfoes
{
   public static class CustomTime
    {
        public static float LocalTimeScale = 1f;
        public static float DeltaTime
        {
            get
            {
                return Time.deltaTime * LocalTimeScale;
            }
        }
        public static bool IsPaused
        {
            get
            {
                return LocalTimeScale == 0f;
            }
        }
        public static float TimeScale
        {
            get
            {
                return Time.timeScale * LocalTimeScale;
            }
        }
    }
}
