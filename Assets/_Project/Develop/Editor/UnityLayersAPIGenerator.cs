using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Assets._Project.Develop.Editor
{
    public class UnityLayersAPIGenerator
    {
        private static string OutputPath
            => Path.Combine(Application.dataPath, "_Project/Develop/Runtime/Gameplay/EntitiesCore/Generated/UnityLayersAPI.cs");

        [InitializeOnLoadMethod]
        [MenuItem("Tools/GenerateUnityLayersAPI")]
        private static void Generate()
        {
            string[] layers = InternalEditorUtility.layers;

            StringBuilder sb = new();

            sb.AppendLine("using UnityEngine;");
            sb.AppendLine();

            sb.AppendLine("public static class UnityLayersAPI");
            sb.AppendLine("{");

            foreach (string layer in layers)
            {
                string safeName = ToSafeName(layer);

                sb.AppendLine($"\tpublic static readonly int Layer{safeName} = LayerMask.NameToLayer(\"{layer}\");");
            }

            sb.AppendLine();

            foreach(string layer in layers)
            {
                string safeName = ToSafeName(layer);

                sb.AppendLine($"\tpublic static readonly int LayerMask{safeName} = 1 << Layer{safeName};");
            }

            sb.AppendLine("}");

            File.WriteAllText(OutputPath, sb.ToString());

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static string ToSafeName(string layerName)
        {
            StringBuilder sb = new();

            foreach (char c in layerName)
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);

            return sb.ToString();
        }
    }
}