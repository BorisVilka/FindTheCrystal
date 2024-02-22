using System;
using UnityEngine;
using Utils.Android;
using Utils.Android.Model;
using Utils.Android.Proxy;

namespace Core.CommonScripts.Utils.Native.Android {
    public class AndroidNative : BaseAndroidNative {
        private static string _packageName;

        public static string GetPackageName() {
            return _packageName ??= GetActivity().Call<string>("getPackageName");
        }

        public static bool IsNetworkAvailable() {
            var manager = GetActivity().Call<AndroidJavaObject>("getSystemService",
                ContextClass.GetStatic<string>("CONNECTIVITY_SERVICE"));

            var networkInfo = manager.Call<AndroidJavaObject>("getActiveNetworkInfo");
            return networkInfo != null && networkInfo.Call<bool>("isConnectedOrConnecting");
        }

        public static void GetAdvertisingIdentifier(Action<AdvertisingData> callback) {
            var data = GmsAdvertisingIdentifier();
            if (data == null || data.Id == null) {
                data = HmsAdvertisingIdentifier();
            }

            callback?.Invoke(data);
        }


        public static AdvertisingData GmsAdvertisingIdentifier() {
            try {
                var client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
                return GetAdvertisingData(client);
            } catch (Exception e) {
                Debug.Log($"GooglePlayServices not available: {e.StackTrace}");
                return null;
            }
        }

        public static AdvertisingData HmsAdvertisingIdentifier() {
            try {
                var client = new AndroidJavaClass("com.huawei.hms.ads.identifier.AdvertisingIdClient");
                return GetAdvertisingData(client);
            } catch (Exception e) {
                Debug.Log($"HuaweiPlayServices not available: {e.StackTrace}");
                return null;
            }
        }

        private static AdvertisingData GetAdvertisingData(AndroidJavaClass client) {
            try {
                var adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo",
                    GetActivity());
                var data = new AdvertisingData {
                    Id = adInfo.Call<string>("getId"),
                    IsLimitAdTrackingEnabled = adInfo.Call<bool>("isLimitAdTrackingEnabled")
                };
                return data;
            } catch (Exception e) {
                Debug.Log(e);
                return null;
            }
        }

        public static void GetReferrerData(Action<ReferrerDetails> callback) {
            var referrerClient = new AndroidJavaObject("com.android.installreferrer.api.InstallReferrerClientImpl",
                GetContext());
            referrerClient.Call("startConnection",
                new RefListener((statusCode) => {
                    if (statusCode == 0) {
                        var bundle = referrerClient.Call<AndroidJavaObject>("getInstallReferrer");
                        if (bundle != null) {
                            var refData = new ReferrerDetails(bundle);
                            callback?.Invoke(refData);
                            referrerClient.Call("endConnection");
                        } else {
                            callback?.Invoke(null);
                        }
                    } else {
                        callback?.Invoke(null);
                    }
                })
            );
        }
    }
}