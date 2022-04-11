//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Dream.Definition;
using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Dream.DataTable
{
    public static class DataTableExtension
    {
        private const string DataRowClassPrefixName = "StarForce.DR";
        internal static readonly char[] DataSplitSeparators = new char[] { '\t' };
        internal static readonly char[] DataTrimSeparators = new char[] { '\"' };

        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, string dataTableAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitedNames = dataTableName.Split('_');
            if (splitedNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataRowClassName = DataRowClassPrefixName + splitedNames[0];
            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            string name = splitedNames.Length > 1 ? splitedNames[1] : null;
            DataTableBase dataTable = dataTableComponent.CreateDataTable(dataRowType, name);
            dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset, userData);
        }

        public static Color32 ParseColor32(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color32(byte.Parse(splitedValue[0]), byte.Parse(splitedValue[1]), byte.Parse(splitedValue[2]), byte.Parse(splitedValue[3]));
        }

        public static Color ParseColor(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Color(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Quaternion ParseQuaternion(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Quaternion(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Rect ParseRect(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Rect(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Vector2 ParseVector2(string value)
        {
            value = value.Trim('(', ')');
            string[] splitedValue = value.Split(',');
            return new Vector2(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]));
        }

        public static Vector3 ParseVector3(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector3(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]));
        }

        public static Vector4 ParseVector4(string value)
        {
            string[] splitedValue = value.Split(',');
            return new Vector4(float.Parse(splitedValue[0]), float.Parse(splitedValue[1]), float.Parse(splitedValue[2]), float.Parse(splitedValue[3]));
        }

        public static Dictionary<int, int> ParseDictionary_2(string value)
        {
            string[] splitedValue = value.Split(',');
            Dictionary<int, int> result = new Dictionary<int, int>(splitedValue.Length);
            foreach (string itemValue in splitedValue)
            {
                string[] splitedItemValue = itemValue.Split('|');
                result[int.Parse(splitedItemValue[0])] = int.Parse(splitedItemValue[1]);
            }

            return result;
        }

        public static List<Dictionary<int, int>> ParseTileItemActions(string value)
        {
            string[] splitedValue = value.Split(',');
            List<Dictionary<int, int>> result = new List<Dictionary<int, int>>(splitedValue.Length);

            foreach (string _itemValue in splitedValue)
            {
                string itemValue = _itemValue.Trim('{', '}');
                string[] itemSplitValue = itemValue.Split(';');
                Dictionary<int, int> itemInst = new Dictionary<int, int>(itemSplitValue.Length);
                result.Add(itemInst);
                foreach (string item in itemSplitValue)
                {
                    string[] splitedItemValue = item.Split('|');
                    itemInst[int.Parse(splitedItemValue[0])] = int.Parse(splitedItemValue[1]);
                }
            }

            return result;
        }
    }
}
