using System;
using UnityEngine;

namespace Utils.Android.Proxy {
    public class RefListener : AndroidJavaProxy {
        private readonly Action<int> _callback;

        public RefListener(Action<int> action = null) : base(
            "com.android.installreferrer.api.InstallReferrerStateListener") {
            _callback = action;
        }

        public void onInstallReferrerSetupFinished(int responseCode) {
            _callback?.Invoke(responseCode);
        }

        public void onInstallReferrerServiceDisconnected() { }
    }
}