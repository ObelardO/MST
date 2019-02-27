﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Obel.MSS;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
using System.Linq;

namespace Obel.MSS.Editor
{

    
    public static class MSSStateEditorWrapperExtantions
    {
        public static MSSStateEditorWrapper GetWrapperForState(this List<MSSStateEditorWrapper> wrappers, MSSState state)
        {
            return wrappers.Where(w => w.state == state).FirstOrDefault();
        }

        public static void OpenOne(this List<MSSStateEditorWrapper> wrappers)
        {
            wrappers.ForEach(sw => sw.stateFade.target = false);
            wrappers.Last().stateFade.target = true;
        }
    }

    public class MSSStateEditorWrapper
    {
        public MSSState state;

        public ReorderableList tweensList;
        public AnimBool stateFade;
        public AnimBool tweenFade;

        private int stateID;

        MSSStateEditorWrapper(int stateID, MSSState state, UnityAction onRepaint)
        {
            this.state = state;
            this.stateID = stateID;

            stateFade = new AnimBool(false);
            stateFade.valueChanged.AddListener(onRepaint);

            tweenFade = new AnimBool(false);
            tweenFade.valueChanged.AddListener(onRepaint);

            tweensList = new ReorderableList(state.tweens, typeof(IMSSTween), true, true, true, true);

            tweensList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
            {
                EditorGUI.LabelField(rect, state.tweens[index].name + "[" + index + "]");
            };
            tweensList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Tweens");
            };
            tweensList.elementHeightCallback = (int index) =>
            {
                return 60;
            };
            tweensList.onAddCallback += rect =>
            {
                Undo.RecordObject(state.gameObject, "[MSS] add tween");
                state.tweens.Add(new MSSTweenPosition(state));
            };
            tweensList.onRemoveCallback += rect =>
            {
                Undo.RecordObject(state.gameObject, "[MSS] remove tween");
                state.tweens.RemoveAt(tweensList.index);
            };
        }

        public static void Generate(SerializedObject serializedObject, ref List<MSSStateEditorWrapper> wrappers, List<MSSState> states, UnityAction onRepaint = null)
        {   
            wrappers = wrappers.Where(w => states.Contains(w.state)).ToList();

            List<MSSState> statesToAdd = new List<MSSState>();

            foreach (MSSState state in states)
            {
                bool stateExist = false;
                foreach (MSSStateEditorWrapper wrapper in wrappers) if (wrapper.state == state) { stateExist = true; break; }
                if (!stateExist) statesToAdd.Add(state);
            }

            for (int i = 0; i < statesToAdd.Count; i++) wrappers.Add(new MSSStateEditorWrapper(i, statesToAdd[i], onRepaint));
        }

        public static void DrawAll(ref List<MSSStateEditorWrapper> wrappers)
        {
            foreach (MSSStateEditorWrapper wrapper in wrappers) wrapper.Draw();
        }

        public static void DrawAllInState(ref List<MSSStateEditorWrapper> wrappers, MSSState state)
        {
            GetWrapper(ref wrappers, state).Draw();
        }

        public static MSSStateEditorWrapper GetWrapper(ref List<MSSStateEditorWrapper> wrappers, MSSState state)
        {
            foreach (MSSStateEditorWrapper wrapper in wrappers) if (wrapper.state == state) return wrapper;
            return null;
        }

        public void Draw()
        {
            tweensList.DoLayoutList();
        }
    }
    

    /*
    public class MSSStateWrapper
    {
        public MSSState state;

        public ReorderableList tweensList;
        public AnimBool stateFade;
        public AnimBool tweenFade;
    }
    */

    



    [CustomEditor(typeof(MSSItem))]
    public class MSSItemEditor : UnityEditor.Editor
    {
        #region Content

        private static readonly GUIContent statesHolderTitle = new GUIContent("States");

        #endregion

        #region Private

        private static MSSItem drawingItem;
  
        private static List<MSSStateEditorWrapper> statesWrappers = new List<MSSStateEditorWrapper>();
        private static MSSStateEditorWrapper drawingStateWrapper;

        private static MSSState drawingState;
        private static SerializedObject serializedItem;

        private static MSSState stateToRemove;

        #endregion

        public void OnDisable()
        {
            statesWrappers.Clear();
        }

        public void OnEnable()
        {
            drawingItem = (MSSItem)target;

            serializedItem = serializedObject;

            statesWrappers.Clear();
            ReGenerateWrappers();
        }


        private void ReGenerateWrappers()
        {
            if (statesWrappers.Count == drawingItem.count) return;
            MSSStateEditorWrapper.Generate(serializedItem, ref statesWrappers, drawingItem.states, Repaint);
        }


        #region InspectorGUI

        public override void OnInspectorGUI()
        {
            ReGenerateWrappers();

            serializedObject.Update();

            EditorGUILayout.Space();


            foreach(MSSState state in item.states)
                //MSSEditorUtils.DrawPanel(drawingStateWrapper.stateFade, state.name, OnDrawStatePanel, OnStateDrawHeader);



            {
                drawingStateWrapper = statesWrappers.GetWrapperForState(state);
                drawingState = state;

                MSSEditorUtils.DrawPanel(drawingStateWrapper.stateFade, state.name, OnDrawStatePanel, OnStateDrawHeader);
            }


            //EditorGUILayout.Space();

            if (GUILayout.Button("ADD NEW STATE")) OnAddState();

            //RemoveMarkedStates();
            CloseInspector();
        }

        private void RemoveMarkedStates()
        {
            if (stateToRemove == null) return;

            Undo.RecordObject(drawingItem, "[MSS] remove state");

            drawingItem.RemoveState(stateToRemove);
            stateToRemove = null;
        }

        private void CloseInspector()
        {
            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(target);

            DrawDefaultInspector();
        }

        public void OnDrawState(MSSState state)
        {
            drawingStateWrapper = statesWrappers.GetWrapperForState(state);
            drawingState = state;

            MSSEditorUtils.DrawPanel(drawingStateWrapper.stateFade, state.name, OnDrawStatePanel, OnStateDrawHeader);
        }

        public void OnStateDrawHeader()
        {
            if (GUILayout.Button("X"))
            {
                OnRemoveState(drawingState);
                ReGenerateWrappers();
                return;
            }
        }

        public void OnDrawStatePanel()
        {
            EditorGUILayout.Space();
            MSSStateEditorWrapper.DrawAllInState(ref statesWrappers, drawingState);
        }


        public void OnAddState()
        {
            Undo.RecordObject(drawingItem, "[MSS] new state");

            drawingItem.AddState();
            ReGenerateWrappers();



            statesWrappers.Open();
        }

        public void OnRemoveState(MSSState state)
        {
            stateToRemove = state;
        }

        public void OnUpState(MSSState state)
        {

        }

        public void OnDownState(MSSState state)
        {

        }

        #endregion
    }
}
