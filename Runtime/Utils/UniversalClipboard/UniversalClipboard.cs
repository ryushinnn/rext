using System;
using System.Reflection;
using UnityEngine;

namespace Ryushin.Utils {
    public static class UniversalClipboard {
        private interface IClipboard {
            void SetText(string text);
            string GetText();
        }
        
        private static IClipboard _clipboard;
        
        public static void SetText(string text) {
            GetClipboard().SetText(text);
        }

        public static string GetText() {
            return GetClipboard().GetText();
        }

        private static IClipboard GetClipboard() {
            if (_clipboard == null) {
#if UNITY_ANDROID && !UNITY_EDITOR
                _clipboard = new AndroidClipboard();
#elif UNITY_IOS && !UNITY_TVOS && !UNITY_EDITOR
                _clipboard = new IOSClipboard();
#else
                _clipboard = new StandardClipboard();
#endif
            }

            return _clipboard;
        }

        private class StandardClipboard : IClipboard {
            private PropertyInfo _systemCopyBufferProperty;

            private PropertyInfo GetSystemCopyBufferProperty() {
                if (_systemCopyBufferProperty == null) {
                    var t = typeof(GUIUtility);
                    _systemCopyBufferProperty = t.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.Public);
                    if (_systemCopyBufferProperty == null) {
                        _systemCopyBufferProperty = t.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
                    }

                    if (_systemCopyBufferProperty == null) {
                        throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
                    }
                }

                return _systemCopyBufferProperty;
            }

            public void SetText(string text) {
                GetSystemCopyBufferProperty().SetValue(null, text, null);
            }

            public string GetText() {
                return (string)GetSystemCopyBufferProperty().GetValue(null, null);
            }
        }
        
#if UNITY_IOS && !UNITY_TVOS
        private class IOSClipboard : IClipboard {
            [DllImport("__Internal")]
            static extern void SetText_ (string str);
            [DllImport("__Internal")]
            static extern string GetText_();
            
            public void SetText(string text){
                if (Application.platform != RuntimePlatform.OSXEditor) {
                    SetText_ (text);
                }
            }

            public string GetText(){
                return GetText_();
            }
        }
#endif

#if UNITY_ANDROID
        private class AndroidClipboard : IClipboard {
            private AndroidJavaObject GetClipboardManager() {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                var staticContext = new AndroidJavaClass("android.content.Context");
                var service = staticContext.GetStatic<AndroidJavaObject>("CLIPBOARD_SERVICE");
                return activity.Call<AndroidJavaObject>("getSystemService", service);
            }

            public void SetText(string text) {
                GetClipboardManager().Call("setText", text);
            }

            public string GetText() {
                return GetClipboardManager().Call<string>("getText");
            }
        }
#endif
    }
}