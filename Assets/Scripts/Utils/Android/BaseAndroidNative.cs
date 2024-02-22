using UnityEngine;

namespace Utils.Android
{
    public abstract class BaseAndroidNative {
        protected static AndroidJavaClass UnityActivity;
        protected static AndroidJavaObject CurrentActivity;
        protected static AndroidJavaObject CurrentContext;

        protected static readonly AndroidJavaClass ContextClass;
        protected static readonly AndroidJavaClass IntentClass;
        protected static readonly AndroidJavaClass BuildClass;

        static BaseAndroidNative() {
            IntentClass = new AndroidJavaClass("android.content.Intent");
            ContextClass = new AndroidJavaClass("android.content.Context");
            BuildClass = new AndroidJavaClass("android.os.Build");
        }

        protected static AndroidJavaClass GetUnityPlayer() {
            return UnityActivity ??= new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }

        protected static AndroidJavaObject GetActivity() {
            return CurrentActivity ??= GetUnityPlayer().GetStatic<AndroidJavaObject>("currentActivity");
        }

        protected static AndroidJavaObject GetContext() {
            return CurrentContext ??= GetActivity().Call<AndroidJavaObject>("getApplicationContext");
        }

        protected static string GetIntentParam(string name) {
            return IntentClass.GetStatic<string>(name);
        }

        protected static string GetBuildParam(string name) {
            return BuildClass.GetStatic<string>(name);
        }

        protected static AndroidJavaObject CreateBaseDialog(string title = null, string message = null) {
            var builder = new AndroidJavaObject("android.app.AlertDialog$Builder", CurrentActivity);
            if (title != null) builder.Call<AndroidJavaObject>("setTitle", title);
            if (message != null) builder.Call<AndroidJavaObject>("setMessage", message);

            builder.Call<AndroidJavaObject>("setCancelable", false);
            return builder;
        }
    }
}