using UnityEditor;
using UnityEngine;

public class AdUnitManagerWindow : EditorWindow
{
    private AdUnitSettings settings;
    private Texture2D headerIcon;

    [MenuItem("LocalSDK/SDK Manager")]
    public static void ShowWindow()
    {
        GetWindow<AdUnitManagerWindow>("SDK Manager");
    }

    private void OnEnable()
    {
        LoadSettings();
        headerIcon = Resources.Load<Texture2D>("LocalSDK/logo"); // load logo.png from Resources/LocalSDK/
    }

    private void LoadSettings()
    {
        settings = AssetDatabase.LoadAssetAtPath<AdUnitSettings>("Assets/Editor/AdUnitSettings.asset");

        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<AdUnitSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Editor/AdUnitSettings.asset");
            AssetDatabase.SaveAssets();
        }
    }

    private Vector2 scroll;

    private void OnGUI()
    {
        if (settings == null)
        {
            LoadSettings();
        }

        scroll = EditorGUILayout.BeginScrollView(scroll); // ⬅️ Start scroll view

        GUILayout.Space(10);

        if (headerIcon != null)
        {
            float iconSize = 256f;
            Rect iconRect = GUILayoutUtility.GetRect(iconSize, iconSize, GUILayout.ExpandWidth(false));
            iconRect.x = (position.width - iconSize) / 2;
            GUI.DrawTexture(iconRect, headerIcon, ScaleMode.ScaleToFit);
        }
        else
        {
            EditorGUILayout.HelpBox("Place logo.png in Resources/CodeGames/ to show a header icon.", MessageType.Info);
        }

        GUILayout.Space(15);

        // Android Ad IDs
        GUILayout.Label("Android Ad Unit IDs", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        settings.androidRewardedID = EditorGUILayout.TextField("Rewarded ID", settings.androidRewardedID);
        settings.androidInterstitialID = EditorGUILayout.TextField("Interstitial ID", settings.androidInterstitialID);
        settings.androidBannerID = EditorGUILayout.TextField("Banner ID", settings.androidBannerID);
        EditorGUILayout.EndVertical();

        GUILayout.Space(15);

        // iOS Ad IDs
        GUILayout.Label("iOS Ad Unit IDs", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        settings.iosRewardedID = EditorGUILayout.TextField("Rewarded ID", settings.iosRewardedID);
        settings.iosInterstitialID = EditorGUILayout.TextField("Interstitial ID", settings.iosInterstitialID);
        settings.iosBannerID = EditorGUILayout.TextField("Banner ID", settings.iosBannerID);
        EditorGUILayout.EndVertical();

        GUILayout.Space(15);

        // Applovin
        GUILayout.Label("Applovin Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        settings.applovinKey = EditorGUILayout.TextField("Applovin Key", settings.applovinKey);
        EditorGUILayout.EndVertical();

        GUILayout.Space(15);

        // Game Analytics
        GUILayout.Label("Game Analytics", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        settings.secretKey = EditorGUILayout.TextField("Secret Key", settings.secretKey);
        settings.gameKey = EditorGUILayout.TextField("Game Key", settings.gameKey);
        EditorGUILayout.EndVertical();

        GUILayout.Space(15);

        // Facebook Settings
        GUILayout.Label("Facebook Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        settings.appName = EditorGUILayout.TextField("App Name", settings.appName);
        settings.appKey = EditorGUILayout.TextField("App Key", settings.appKey);
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.EndScrollView(); // ⬅️ End scroll view

        if (GUI.changed)
        {
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }

}
