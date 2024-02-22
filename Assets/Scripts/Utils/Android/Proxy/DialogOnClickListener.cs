using System;
using UnityEngine;

namespace Utils.Android.Proxy {
    public class DialogOnClickListener : AndroidJavaProxy {
        private readonly Action _action;

        public DialogOnClickListener(Action action = null) : base(
            "android.content.DialogInterface$OnClickListener") {
            _action = action;
        }

        public void onClick(AndroidJavaObject dialog, int which) {
            _action?.Invoke();
        }
    }
}