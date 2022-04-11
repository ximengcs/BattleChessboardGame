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
        private sealed class DictionaryInt2Processor : GenericDataProcessor<Dictionary<int, int>>
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
                    return "Dictionary<int,int>";
                }
            }

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "dictionary<int,int>"
                };
            }

            public override Dictionary<int, int> Parse(string value)
            {
                return DataTableExtension.ParseDictionary_2(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(value);
            }
        }
    }
}
