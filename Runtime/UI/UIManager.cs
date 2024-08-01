using System.Collections.Generic;
using System.Linq;
using Assassin.Core;
using Assassin.Extension;
using Assassin.Utils;
using UnityEngine;

namespace Assassin.UI {
    public class UIManager : Singleton<UIManager> {
        [SerializeField] private List<UI> _uiList = new();

        protected override void OnAwake() {
            transform.IterateChildren<UI>(ui => _uiList.Add(ui));
        }
        
        public static T GetUI<T>() where T : UI {
            return Instance()._uiList.OfType<T>().FirstOrDefault();
        }
        
        public static void OpenUI<T>(params object[] prs) where T : UI {
            var ui = GetUI<T>();
            if (!ui) {
                ALog.Log($"UI with type {typeof(T).Name} is missing!!!");
                return;
            }
        
            ui.Open(prs);
        }

        public static void CloseUI<T>() where T : UI {
            var ui = GetUI<T>();
            if (!ui) {
                ALog.Log($"UI with type {typeof(T).Name} is missing!!!");
                return;
            }

            ui.Close();
        }
    }
}