using System;
using System.Collections.Generic;
using Assassin.Extension;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assassin.Utils {
    public static class Common {
        #region Transform/GameObject

        public static void DestroyAllChildren(this Transform transform) {
            foreach (Transform child in transform) UnityEngine.Object.Destroy(child.gameObject);
        }

        #endregion

        #region Camera

        private static Camera _camera;

        public static Camera GetCamera() {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }

        #endregion

        #region Primitive data types

        public static string GetFormatedNumber(float num) {
            return num switch {
                >= 1000000000 => (num / 1000000000).ToString("0.##B"),
                >= 1000000 => (num / 1000000).ToString("0.##M"),
                >= 1000 => (num / 1000).ToString("0.##K"),
                _ => num.ToString("0.##"),
            };
        }

        public static int CompareFloat(float a, float b, float error = 0.001f) {
            float delta = Mathf.Abs(a - b);
            if (delta >= error) return 1;
            if (delta <= -error) return -1;
            return 0;
        }

        #endregion

        #region Gizmos

        public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f) {
            var direction = to - from;
            Gizmos.DrawRay(from, direction);
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
            var up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
            var down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
            Gizmos.DrawRay(to, right * arrowHeadLength);
            Gizmos.DrawRay(to, left * arrowHeadLength);
            Gizmos.DrawRay(to, up * arrowHeadLength);
            Gizmos.DrawRay(to, down * arrowHeadLength);
        }

        #endregion

        #region Coroutine

        private static Dictionary<float, WaitForSeconds> _waitDictionary = new();

        public static WaitForSeconds GetWait(float time) {
            if (_waitDictionary.TryGetValue(time, out var wait)) return wait;
            _waitDictionary[time] = new WaitForSeconds(time);
            return _waitDictionary[time];
        }

        private static Dictionary<float, WaitForSecondsRealtime> _waitRealTimeDictionary = new();

        public static WaitForSecondsRealtime GetWaitRealTime(float time) {
            if (_waitRealTimeDictionary.TryGetValue(time, out var wait)) return wait;
            _waitRealTimeDictionary[time] = new WaitForSecondsRealtime(time);
            return _waitRealTimeDictionary[time];
        }

        #endregion

        #region UI

        private static PointerEventData _currentPositionEventData;
        private static List<RaycastResult> _raycastResults;

        public static bool IsOverUI() {
            _currentPositionEventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_currentPositionEventData, _raycastResults);
            return _raycastResults.IsNotEmpty();
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, GetCamera(), out var position);
            return position;
        }

        public static Vector3 GetCanvasPosition(GameObject gameObject, Canvas canvas) {
            if (gameObject == null || canvas == null) return default;
            var worldPosition = gameObject.transform.position;
            var viewportPosition = GetCamera().WorldToViewportPoint(worldPosition);
            var rect = canvas.GetComponent<RectTransform>();
            var canvasSize = rect.sizeDelta;
            var canvasPosition = new Vector2(
                viewportPosition.x * canvasSize.x - canvasSize.x * 0.5f,
                viewportPosition.y * canvasSize.y - canvasSize.y * 0.5f
            );

            return canvasPosition;
        }

        public static void SetCanvasPosition(RectTransform target, Vector2 canvasPosition, Canvas canvas) {
            if (CompareFloat(target.anchorMin.x, target.anchorMax.x) != 0
                || CompareFloat(target.anchorMin.y, target.anchorMax.y) != 0) {
                ALog.Log("Only rect transform with 1 anchor (anchorMin = anchorMax) is supported");
            }

            var canvasSizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            target.anchoredPosition = new Vector2(
                canvasPosition.x + canvasSizeDelta.x * (0.5f - target.anchorMin.x),
                canvasPosition.y + canvasSizeDelta.y * (0.5f - target.anchorMin.y)
            );
        }

        #endregion
    }
}