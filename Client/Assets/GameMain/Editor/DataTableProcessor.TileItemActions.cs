//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Dream.DataTable;
using System.Collections.Generic;
using System.IO;

namespace MeaponUnity.Editor
{
    public sealed partial class DataTableProcessor
    {
        private sealed class TileItemActionsProcessor : GenericDataProcessor<List<Dictionary<int, int>>>
        {
            public override bool IsSystem
            {
                get
                {
                    return false;
                }
            }

            public override string LanguageKeyword
            {
                get
                {
                    return "List<Dictionary<int, int>>";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "actions"
                };
            }

            public override List<Dictionary<int, int>> Parse(string value)
            {
                return DataTableExtension.ParseTileItemActions(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(value);
            }
        }
    }
}
