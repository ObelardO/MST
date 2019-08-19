﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obel.MSS
{
    public class EaseQuad
    {
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        #elif UNITY_STANDALONE
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        #endif
        private static void ApplicationStart()
        {
            Ease.Add(QuadIn, "Quad/In");
            Ease.Add(QuadOut, "Quad/Out");
            Ease.Add(QuadInOut, "Quad/InOut");
        }

        #region Public methods

        public static float QuadIn(float t, float d)
        {
            return (t /= d) * t;
        }

        public static float QuadOut(float t, float d)
        {
            return -(t /= d) * (t - 2);
        }

        public static float QuadInOut(float t, float d)
        {
            if ((t /= d / 2) < 1) return 0.5f * t * t;
            return -0.5f * ((--t) * (t - 2) - 1);
        }

        #endregion

        /*
        public static float QuadOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2) return QuadOut(t * 2, b, c / 2, d);
            return QuadIn((t * 2) - d, b + c / 2, c / 2, d);
        }
        */
    }
}


