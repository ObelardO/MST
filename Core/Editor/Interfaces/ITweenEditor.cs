﻿using System;
using UnityEngine;

namespace Obel.MSS.Editor
{
    public interface ITweenEditor
    {
        #region Properties

        string Name { get; }

        Type TweenType { set; get; }

        Action AddAction { set; get; }

        float Height { set; get; }

        #endregion

        #region Inspector

        void OnGUI(Rect rect, Tween tween);

        #endregion
    }
}