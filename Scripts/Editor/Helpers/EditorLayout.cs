﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

namespace Obel.MSS.Editor
{
    internal static class EditorLayout
    {
        #region Properties

        private static Vector2 startPosition;
        private static Rect rect;
        private static float fixedWidth = 30;
        private static float fixedHeight = 16;
        public static readonly float offset = 4;

        private static Color storedColor;

        #endregion

        #region Layout methods

        public static void SetPosition(float x, float y)
        {
            SetPosition(new Vector2(x, y));
        }

        public static void SetPosition(Vector2 position)
        {
            rect = new Rect(position.x + offset, position.y, fixedWidth, fixedHeight);
            startPosition = position;
        }

        public static void SetWidth(float width)
        {
            fixedWidth = width;
        }

        public static void Control(Action<Rect> drawCallback)
        {
            Control(new Vector2(fixedWidth, fixedHeight), drawCallback);
        }

        public static void Control(float width, Action<Rect> drawCallback)
        {
            Control(new Vector2(width, fixedHeight), drawCallback);
        }

        public static void Control(float width, float height, Action<Rect> drawCallback)
        {
            Control(new Vector2(width, height), drawCallback);
        }

        public static void Control(Vector2 size, Action<Rect> drawCallback)
        {
            rect.width = size.x;
            rect.height = size.y;
            drawCallback(rect);
            rect.x += offset + rect.width;
        }

        public static void Space()
        {
            Space(offset);
        }

        public static void Space(float height)
        {
            rect.x = startPosition.x + offset;
            rect.y += height + rect.height;
        }

        public static void PushColor()
        {
            storedColor = GUI.color;
        }

        public static void PullColor()
        {
            GUI.color = storedColor;
        }

        #endregion
    }
}

