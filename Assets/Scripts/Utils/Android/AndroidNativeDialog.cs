using System;
using UnityEngine;
using Utils.Android.Proxy;

namespace Utils.Android
{
    public class AndroidNativeDialog : BaseAndroidNative
    {
        public static void SimpleDialog(string title = null, string message = null, string okBtn = null,
            string cancelBtn = null, Action okAction = null, Action cancelAction = null)
        {
            var builder = CreateBaseDialog(title, message);

            if (!string.IsNullOrEmpty(okBtn) && okAction != null)
                builder.Call<AndroidJavaObject>("setPositiveButton", okBtn,
                    new DialogOnClickListener(okAction));

            if (!string.IsNullOrEmpty(cancelBtn) && cancelAction != null)
                builder.Call<AndroidJavaObject>("setNegativeButton", cancelBtn,
                    new DialogOnClickListener(cancelAction));

            builder.Call<AndroidJavaObject>("create").Call("show");
        }
    }
}