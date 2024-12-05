#nullable enable

namespace LilytechLab.ComponentIconInHierarchy.Editor {
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Hierarchyビューにコンポーネントのアイコンを表示する拡張機能
    /// </summary>
    /// <remarks>
    /// <para>Unity2020.3.5f1で動作確認。</para>
    /// <para>
    /// <list type="bullet">
    /// <item><description>Transform以外のコンポーネントのアイコン表示。</description></item>
    /// <item><description>スクリプトのアイコンは複数付与されていても1つのみ表示。</description></item>
    /// <item><description>コンポーネントが無効になっている場合はアイコン色が半透明になっている。</description></item>
    /// <item><description>コンポーネントがMissingとなっている場合は！のアイコンを表示し、エラーログも表示。</description></item>
    /// <item><description>
    /// コンポーネント内の参照項目にMissingとなっているものが存在する場合は！のアイコンを表示し、エラーログも表示。
    ///（ログはコンポーネント単位で最初に見つかったもののみ表示）
    /// </description></item>
    /// <item><description>ヒエラルキービューで右クリックで表示されるメニュー「Switch Icon Visibility」の選択で表示/非表示の切替可能。</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public static class ComponentIconDrawerInHierarchy {
        #region constants/readonly
        private const int IconSize = 16;

        private const string ParentMenuPath = "GameObject/Switch Icon Visibility/";

        private const string ComponentIconMenuPath = ParentMenuPath + "Component Icon";

        private const string MissingIndicatorMenuPath = ParentMenuPath + "Missing Indicator";

        private const string ScriptIconName = "cs Script Icon";

        private const string WarningIconName = "console.warnicon";

        private const string PropertyNameOfFieldId = "m_FileID";

        private static readonly Color colorWhenDisabled = new(1.0f, 1.0f, 1.0f, 0.5f);
        #endregion

        #region static fields
        private static Texture? scriptIcon;

        private static Texture? warningIcon;

        private static bool componentIconEnabled = true;

        private static bool missingIndicatorEnabled = true;
        #endregion

        #region private methods
        [InitializeOnLoadMethod]
        private static void Initialize() {
            // セーブしていた値をロード
            var componentIconEnabledSetting = EditorUserSettings.GetConfigValue(ComponentIconMenuPath);
            var missingIndicatorEnabledSetting = EditorUserSettings.GetConfigValue(MissingIndicatorMenuPath);

            componentIconEnabled = componentIconEnabledSetting == null || !string.Equals(componentIconEnabledSetting, false.ToString(), System.StringComparison.OrdinalIgnoreCase);
            missingIndicatorEnabled = missingIndicatorEnabledSetting == null || !string.Equals(missingIndicatorEnabledSetting, false.ToString(), System.StringComparison.OrdinalIgnoreCase);

            EditorApplication.delayCall += UpdateStatus;

            /*
             * ビルトインアイコンの呼び出し方は以下を参考にした
             * https://qiita.com/Rijicho_nl/items/88e71b5c5930fc7a2af1
             * https://unitylist.com/p/5c3/Unity-editor-icons
             */
#pragma warning disable UNT0023 // Coalescing assignment on Unity objects
            scriptIcon ??= EditorGUIUtility.IconContent(ScriptIconName).image;
            warningIcon ??= EditorGUIUtility.IconContent(WarningIconName).image;
#pragma warning restore UNT0023 // Coalescing assignment on Unity objects
        }


        [MenuItem(ComponentIconMenuPath, false, 10000)]
        private static void ToggleComponentIconEnabled() {
            componentIconEnabled = !componentIconEnabled;
            UpdateStatus();

            // 状態保持
            EditorUserSettings.SetConfigValue(ComponentIconMenuPath, componentIconEnabled.ToString());
        }

        [MenuItem(MissingIndicatorMenuPath, false, 10000)]
        private static void ToggleMissingIndicatorEnabled() {
            missingIndicatorEnabled = !missingIndicatorEnabled;
            UpdateStatus();

            // 状態保持
            EditorUserSettings.SetConfigValue(MissingIndicatorMenuPath, missingIndicatorEnabled.ToString());
        }

        private static void UpdateStatus() {
            EditorApplication.hierarchyWindowItemOnGUI -= DisplayIcons;
            if (componentIconEnabled || missingIndicatorEnabled)
                EditorApplication.hierarchyWindowItemOnGUI += DisplayIcons;

            Menu.SetChecked(ComponentIconMenuPath, componentIconEnabled);
            Menu.SetChecked(MissingIndicatorMenuPath, missingIndicatorEnabled);
        }

        private static void DisplayIcons(int instanceID, Rect selectionRect) {
            // instanceIDをオブジェクト参照に変換
            if (!(EditorUtility.InstanceIDToObject(instanceID) is GameObject gameObject)) return;

            var pos = selectionRect;
            pos.x = pos.xMax - IconSize;
            pos.width = IconSize;
            pos.height = IconSize;

            // オブジェクトが所持しているコンポーネント一覧を取得
            var components
                = gameObject
                    .GetComponents<Component>()
                    .Where(x => !(x is Transform || x is ParticleSystemRenderer))
                    .Reverse()
                    .ToList();

            // Missingなコンポーネントが存在する場合はWarningアイコン表示
            var existsMissing = components.RemoveAll(x => x == null) > 0;
            if (missingIndicatorEnabled && existsMissing) {
                UnityEngine.Debug.LogError($"{gameObject.name} has MISSING component(s).");
                DrawIcon(ref pos, warningIcon!);
            }

            var existsScriptIcon = false;
            foreach (var component in components) {
                if (missingIndicatorEnabled) {
                    // SerializeFieldsにMissingなものが存在する場合はWarningアイコン表示
                    var existsMissingField = ExistsMissingField(component);
                    if (existsMissingField)
                        DrawIcon(ref pos, warningIcon!);
                }

                if (componentIconEnabled) {
                    Texture image = AssetPreview.GetMiniThumbnail(component);
                    if (image == null) continue;

                    // Scriptのアイコンは1つのみ表示
                    if (image == scriptIcon) {
                        if (existsScriptIcon) continue;
                        existsScriptIcon = true;
                    }

                    // アイコン描画
                    DrawIcon(ref pos, image, component.IsEnabled() ? Color.white : colorWhenDisabled);
                }
            }
        }

        /// <summary>
        /// コンポーネントの設定値にMissingなものが存在するかどうかを確認する
        /// </summary>
        /// <param name="component">確認対象のコンポーネント</param>
        /// <returns>MissingなSerializedFieldが存在するかどうか</returns>
        /// <remarks>
        /// 以下の条件を満たす場合はMissingと見なす。Unityのバージョンが変わると変更になる可能性有。
        /// <list type="bullet">
        /// <item><description><see cref="SerializedProperty.propertyType"/>が<see cref="SerializedPropertyType.ObjectReference"/></description></item>
        /// <item><description><see cref="SerializedProperty.objectReferenceInstanceIDValue"/>がnull</description></item>
        /// <item><description>fileIDが0ではない</description></item>
        /// </list>
        /// </remarks>
        private static bool ExistsMissingField(Component component) {
            var ret = false;
            var serializedProp = new SerializedObject(component).GetIterator();

            while (!ret && serializedProp.NextVisible(true)) {
                if (serializedProp.propertyType != SerializedPropertyType.ObjectReference) continue;
                if (serializedProp.objectReferenceValue != null) continue;

                var fileId = serializedProp.FindPropertyRelative(PropertyNameOfFieldId);
                if (fileId == null || fileId.intValue == 0) continue;

                UnityEngine.Debug.LogError(
                    $"The field {serializedProp.propertyPath} in {component.GetType().Name} on {component.name} is MISSING.");
                ret = true;
            }

            return ret;
        }

        private static void DrawIcon(ref Rect pos, Texture image, Color? color = null) {
            Color? defaultColor = null;
            if (color.HasValue) {
                defaultColor = GUI.color;
                GUI.color = color.Value;
            }

            GUI.DrawTexture(pos, image, ScaleMode.ScaleToFit);
            pos.x -= pos.width;

            if (defaultColor.HasValue)
                GUI.color = defaultColor.Value;
        }

        /// <summary>
        /// コンポーネントが有効かどうかを確認する拡張メソッド
        /// </summary>
        /// <param name="this">拡張対象</param>
        /// <returns>コンポーネントが有効となっているかどうか</returns>
        private static bool IsEnabled(this Component @this) {
            var property = @this.GetType().GetProperty("enabled", typeof(bool));
            return (bool)(property?.GetValue(@this, null) ?? true);
        }
        #endregion

    }

}

