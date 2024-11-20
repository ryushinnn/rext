using System;
using System.Reflection;
using UnityEngine;

namespace RExt.Utils {
    public static class UniversalClipboard {
        interface IClipboard {
            void SetText(string text);
            string GetText();
        }

        static IClipboard Clipboard;
        
        public static void SetText(string text) {
            GetClipboard().SetText(text);
        }

        public static string GetText() {
            return GetClipboard().GetText();
        }

        static IClipboard GetClipboard() {
            if (Clipboard == null) {
#if UNITY_ANDROID && !UNITY_EDITOR
                Clipboard = new AndroidClipboard();
#elif UNITY_IOS && !UNITY_TVOS && !UNITY_EDITOR
                Clipboard = new IOSClipboard();
#else
                Clipboard = new StandardClipboard();
#endif
            }

            return Clipboard;
        }

        class StandardClipboard : IClipboard {
            PropertyInfo systemCopyBufferProperty;

            PropertyInfo GetSystemCopyBufferProperty() {
                if (systemCopyBufferProperty == null) {
                    var t = typeof(GUIUtility);
                    systemCopyBufferProperty = t.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.Public);
                    if (systemCopyBufferProperty == null) {
                        systemCopyBufferProperty = t.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
                    }

                    if (systemCopyBufferProperty == null) {
                        throw new Exception("Can't access internal member 'GUIUtility.systemCopyBuffer' it may have been removed / renamed");
                    }
                }

                return systemCopyBufferProperty;
            }

            public void SetText(string text) {
                GetSystemCopyBufferProperty().SetValue(null, text, null);
            }

            public string GetText() {
                return (string)GetSystemCopyBufferProperty().GetValue(null, null);
            }
        }
        
#if UNITY_IOS && !UNITY_TVOS
        class IOSClipboard : IClipboard {
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
        class AndroidClipboard : IClipboard {
            AndroidJavaObject GetClipboardManager() {
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