using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace RExt.Extension {
    public static class ArrayExtension {
        public static T Find<T>(this T[] array, Predicate<T> match) {
            return Array.Find(array, match);
        }
    }
    
    public static class CollectionExtension {
        public static bool IsNotEmpty<T>(this List<T> list) {
            return list != null && list.Count > 0;
        }

        public static bool IsNotEmpty<T>(this Stack<T> stack) {
            return stack != null && stack.Count > 0;
        }
        
        public static bool IsNotEmpty<T>(this Queue<T> queue) {
            return queue != null && queue.Count > 0;
        }
    }
    
    public static class EnumExtension {
        [Flags]
        public enum FlagEnumExample {
            None = 0,
            First = 0x01,
            Second = 0x02,
            Third = 0x04,
            Fourth = 0x08,
            Fifth = 0x10,
            Sixth = 0x20,
            FirstAndSecond = First | Second,
            ExceptFirst = Second | Third | Fourth | Fifth | Sixth,
            // ...
            All = First | Second | Third | Fourth | Fifth | Sixth
        }
        
        public static string ToDescriptionString(this Enum value) {
            var attributes = (DescriptionAttribute[]) value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionLowerString(this Enum value) {
            var attributes = (DescriptionAttribute[]) value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description.ToLower() : string.Empty;
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum {
            foreach (var field in typeof(T).GetFields()) {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) {
                    if (attribute.Description == description) return (T) field.GetValue(null);
                } else {
                    if (field.Name == description) return (T) field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        public static int IntValue(this Enum value) {
            return Convert.ToInt32(value);
        }

        public static T Next<T>(this T source) where T : struct {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            var arr = (T[]) Enum.GetValues(source.GetType());
            var j = Array.IndexOf(arr, source) + 1;
            return arr.Length == j ? arr[0] : arr[j];
        }

        /// <summary>
        /// Check if an enum (combination) contains another enum
        /// </summary>
        public static bool Has(this Enum combination, Enum value) {
            if (combination.GetType() != value.GetType()) throw new ArgumentException("Type mismatch");
            return (combination.IntValue() & value.IntValue()) == value.IntValue();
        }
    }
    
    public static class LayerExtension {
        public static bool Contains(this LayerMask layerMask, int layer) {
            return (layerMask & (1 << layer)) != 0;
        }
    }
    
    public static class RectTransformExtension {
        public static void SetRect(this RectTransform rt, float left, float right, float top, float bottom) {
            rt.SetLeft(left);
            rt.SetRight(right);
            rt.SetTop(top);
            rt.SetBottom(bottom);
        }

        public static void SetLeft(this RectTransform rt, float left) {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right) {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top) {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom) {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
        
        private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
            Vector3[] objectCorners = new Vector3[4];
            rectTransform.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            for (var i = 0; i < objectCorners.Length; i++) {
                var tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);
                if (screenBounds.Contains(tempScreenSpaceCorner))
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }
        
        /// <summary>
        /// Check if a RectTransform is FULLY visible in screen
        /// </summary>
        /// <param name="rectTransform">target</param>
        /// <param name="camera">seen by which camera</param>
        public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) == 4;
        }
        
        /// <summary>
        /// Check if a RectTransform is visible in screen
        /// </summary>
        /// <param name="rectTransform">target</param>
        /// <param name="camera">seen by which camera</param>
        public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
        {
            return CountCornersVisibleFrom(rectTransform, camera) > 0;
        }
        
        /// <summary>
        /// Check if a RectTransform is visible in another RectTransform (e.g. a ScrollRect)
        /// </summary>
        /// <param name="element">target</param>
        /// <param name="container">container</param>
        public static bool IsVisibleInside(this RectTransform element, RectTransform container)
        {
            var elementCorners = new Vector3[4];
            var containerCorners = new Vector3[4];
        
            element.GetWorldCorners(elementCorners);
            container.GetWorldCorners(containerCorners);

            var isVisible = false;
            foreach (Vector3 corner in elementCorners)
            {
                if (corner.x >= containerCorners[0].x && corner.x <= containerCorners[2].x &&
                    corner.y >= containerCorners[0].y && corner.y <= containerCorners[2].y)
                {
                    isVisible = true;
                    break;
                }
            }

            if (!isVisible)
            {
                var rect1 = new Rect(elementCorners[0], element.sizeDelta);
                var rect2 = new Rect(containerCorners[0], container.sizeDelta);
                isVisible = rect1.Overlaps(rect2);
            }

            return isVisible;
        }
    }
    
    public static class ScrollRectExtension {
		public static Vector2 CalculateFocusedScrollPosition( this ScrollRect scrollView, Vector2 focusPoint )
		{
			var contentSize = scrollView.content.rect.size;
			var viewportSize = ( (RectTransform) scrollView.content.parent ).rect.size;
			var contentScale = scrollView.content.localScale;

			contentSize.Scale( contentScale );
			focusPoint.Scale( contentScale );

			var scrollPosition = scrollView.normalizedPosition;
			if( scrollView.horizontal && contentSize.x > viewportSize.x )
				scrollPosition.x = Mathf.Clamp01( ( focusPoint.x - viewportSize.x * 0.5f ) / ( contentSize.x - viewportSize.x ) );
			if( scrollView.vertical && contentSize.y > viewportSize.y )
				scrollPosition.y = Mathf.Clamp01( ( focusPoint.y - viewportSize.y * 0.5f ) / ( contentSize.y - viewportSize.y ) );

			return scrollPosition;
		}

		public static Vector2 CalculateFocusedScrollPosition( this ScrollRect scrollView, RectTransform item )
		{
			var itemCenterPoint = (Vector2)scrollView.content.InverseTransformPoint( item.transform.TransformPoint( item.rect.center ) );

			var contentSizeOffset = scrollView.content.rect.size;
			contentSizeOffset.Scale( scrollView.content.pivot );

			return scrollView.CalculateFocusedScrollPosition( itemCenterPoint + contentSizeOffset );
		}

		public static void FocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint )
		{
			scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition( focusPoint );
		}

		public static void FocusOnItem(this ScrollRect scrollView, RectTransform item )
		{
			scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition( item );
		}

		private static IEnumerator LerpToScrollPositionCoroutine( this ScrollRect scrollView, Vector2 targetNormalizedPos, float speed )
		{
			var initialNormalizedPos = scrollView.normalizedPosition;

			var t = 0f;
			while( t < 1f )
			{
				scrollView.normalizedPosition = Vector2.LerpUnclamped( initialNormalizedPos, targetNormalizedPos, 1f - ( 1f - t ) * ( 1f - t ) );

				yield return null;
				t += speed * Time.unscaledDeltaTime;
			}

			scrollView.normalizedPosition = targetNormalizedPos;
		}

		public static IEnumerator FocusAtPointCoroutine(this ScrollRect scrollView, Vector2 focusPoint, float speed )
		{
			yield return scrollView.LerpToScrollPositionCoroutine( scrollView.CalculateFocusedScrollPosition( focusPoint ), speed );
		}

		public static IEnumerator FocusOnItemCoroutine(this ScrollRect scrollView, RectTransform item, float speed )
		{
			yield return scrollView.LerpToScrollPositionCoroutine( scrollView.CalculateFocusedScrollPosition( item ), speed );
		}
	}

    public static class FloatExtension {
        public static float ToMilliseconds(this float s) {
            return s * 1000;
        }
        
        public static float ToSeconds(this float ms) {
            return ms / 1000;
        }
    }
    
    public static class StringExtension {
        public static bool IsValid(this string str) {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
    
    public static class TransformExtension {
        public static void IterateChildren<T>(this Transform t, Action<T> task) {
            foreach (Transform child in t) {
                if (child.TryGetComponent(out T component)) {
                    task?.Invoke(component);
                }
            }
        }
        
        public static void DestroyAllChildren(this Transform t) {
#if UNITY_EDITOR
            if (Application.isPlaying) {
                foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
            }
            else {
                while (t.childCount > 0) {
                    UnityEngine.Object.DestroyImmediate(t.GetChild(0));
                }
            }
#else
            foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
#endif
        }
    }

    public static class ColorExtension {
        public static string ToHex(this Color color) {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
    }

    public static class Texture2DExtension {
        public static Sprite ToSprite(this Texture2D t) {
            if (t == null) return null;
            return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
        }
    }
}