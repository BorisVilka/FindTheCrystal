using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using Unity.Android.Gradle;
using Unity.Android.Gradle.Manifest;
public class ModifyProjectScript : AndroidProjectFilesModifier
{
    public override void OnModifyAndroidProjectFiles(AndroidProjectFiles projectFiles)
    {
        var usesPermissionM0 = new UsesPermission();
        projectFiles.UnityLibraryManifest.Manifest.UsesPermissionList.AddElement(usesPermissionM0);
        usesPermissionM0.Attributes.Name.Set("android.permission.INTERNET");
        
        
        var usesPermissionM1 = new UsesPermission();
        projectFiles.UnityLibraryManifest.Manifest.UsesPermissionList.AddElement(usesPermissionM1);
        usesPermissionM1.Attributes.Name.Set("android.permission.ACCESS_NETWORK_STATE");
        
        
        var usesPermissionM2 = new UsesPermission();
        projectFiles.UnityLibraryManifest.Manifest.UsesPermissionList.AddElement(usesPermissionM2);
        usesPermissionM2.Attributes.Name.Set("com.google.android.gms.permission.AD_ID");

    }
}
