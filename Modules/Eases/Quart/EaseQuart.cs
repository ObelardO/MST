﻿namespace Obel.MSS
{
    public class EaseQuart
    {
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        #elif UNITY_STANDALONE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        #endif
        private static void ApplicationStart()
        {
            Ease.Add(QuartIn, "Quart/In");
            Ease.Add(QuartOut, "Quart/Out");
            Ease.Add(QuartInOut, "Quart/InOut");
        }

        #region Public methods

        public static float QuartIn(float t, float d)
        {
            return (t /= d) * t * t * t;
        }

        public static float QuartOut(float t, float d)
        {
            return (t /= d - 1) * t * t * t + 1;
        }

        public static float QuartInOut(float t, float d)
        {
            if ((t /= d / 2) < 1) return 0.5f * t * t * t * t;
            return 0.5f * ((t -= 2) * t * t * t + 2);
        }

        /*
        public static float QuartOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2) return QuartOut(t * 2, b, c / 2, d);
            return QuartIn((t * 2) - d, b + c / 2, c / 2, d);
        }
        */

        #endregion
    }
}