using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils.Android.Model {
    public class ReferrerDetails {
        private const string Source = "c99950998e1fb1c5e18cec0b2b70b42b";
        private const string Medium = "92247aa9766c9c6deebb28f078f75b83";

        public string InstallReferrer { get; set; }
        public long ReferrerClickTimestampSeconds { get; set; }
        public long InstallBeginTimestampSeconds { get; set; }
        public bool GooglePlayInstantParam { get; set; }

        public ReferrerDetails(AndroidJavaObject referrerBundle) {
            if (referrerBundle == null) return;
            InstallReferrer = referrerBundle.Call<string>("getInstallReferrer");
            ReferrerClickTimestampSeconds = referrerBundle.Call<long>("getReferrerClickTimestampSeconds");
            InstallBeginTimestampSeconds = referrerBundle.Call<long>("getInstallBeginTimestampSeconds");
            GooglePlayInstantParam = referrerBundle.Call<bool>("getGooglePlayInstantParam");
        }

        public bool IsValid {
            get {
                var valid = true;
                try {
                    var data = UnityWebRequest.UnEscapeURL(InstallReferrer).Split('&');
                    var dict = data.Select(t => t.Split('='))
                        .Where(val => val.Length == 2)
                        .ToDictionary<string[], string, object>
                            (val => val[0], val => Md5(val[1]));

                    if (dict.ContainsKey("utm_source") && dict["utm_source"].Equals(Source) &&
                        dict.ContainsKey("utm_medium") && dict["utm_medium"].Equals(Medium))
                    {
                        valid = false;
                    }
                } catch (Exception) {
                    Debug.Log("[ReferrerDetails] Problem with parse data");
                }
                return valid;
            }
        }

        public JObject Json {
            get {
                var json = new JObject { { "event", "track_referrer" }, { "save", true } };
                try {
                    var data = UnityWebRequest.UnEscapeURL(InstallReferrer).Split('&');
                    foreach (var t in data) {
                        var val = t.Split('=');
                        if (val.Length != 2)
                            continue;
                        json.Add(val[0], val[1]);
                    }
                } catch (Exception) {
                    Debug.Log("Problem with parse referrer data");
                } finally {
                    json.Add("full_data", new JObject {
                            { "InstallReferrer", InstallReferrer },
                            { "ReferrerClickTimestampSeconds", ReferrerClickTimestampSeconds },
                            { "InstallBeginTimestampSeconds", InstallBeginTimestampSeconds },
                            { "GooglePlayInstantParam", GooglePlayInstantParam }
                        }
                    );
                }

                return json;
            }
        }

        private static string Md5(string input) {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hashBytes) {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString().ToLower();
        }
    }
}