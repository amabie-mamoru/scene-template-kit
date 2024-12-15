using UnityEditor;
using UnityEngine;
using System.IO;

namespace com.amabie.SceneTemplateKit
{
    [InitializeOnLoad]
    public static class PostImportHandler
    {
        static PostImportHandler()
        {
            // パッケージがインポートされたタイミングでダイアログを表示
            EditorApplication.delayCall += OnPackageImported;
        }

        private static void OnPackageImported()
        {
            if (EditorPrefs.GetBool("SceneTemplateKitPackage_Imported", false))
                return;

            EditorPrefs.SetBool("SceneTemplateKitPackage_Imported", true);

            if (EditorUtility.DisplayDialog(
                "サンプルを生成しますか？",
                "パッケージにサンプルデータがあります。生成しますか？",
                "生成する",
                "キャンセル"))
            {
                CreateSample();
            }
        }

        private static void CreateSample()
        {
            // サンプルのコピー元パスを取得
            string packagePath = "Packages/com.amabie.SceneTemplateKit/Samples~/SceneTemplateSample";
            string destinationPath = Path.Combine(Application.dataPath, "SceneTemplateSample");

            // サンプルデータをプロジェクトにコピー
            if (Directory.Exists(packagePath))
            {
                Directory.CreateDirectory(destinationPath);
                foreach (string dirPath in Directory.GetDirectories(packagePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(packagePath, destinationPath));
                }

                foreach (string filePath in Directory.GetFiles(packagePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(filePath, filePath.Replace(packagePath, destinationPath), true);
                }

                AssetDatabase.Refresh();
                Debug.Log("サンプルが生成されました: " + destinationPath);
            }
            else
            {
                Debug.LogError("サンプルパスが見つかりません: " + packagePath);
            }
        }
    }
}