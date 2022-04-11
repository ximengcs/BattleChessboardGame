using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace MeaponUnity.Editor
{
    public class GenerateAssetsBundle
    {
        [MenuItem("Tools/Generate Ab", false, 1)]
        public static void GenerateAB()
        {
            BuildPipeline.BuildAssetBundles("Assets/Resources", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        }
    }
}
