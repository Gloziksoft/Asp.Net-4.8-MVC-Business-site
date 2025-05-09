using System;
using System.Globalization;
using System.Xml;

namespace dufeksoft.lib.ParamSet
{
    public class XmlParamSet
    {
        /// <summary>
        /// Saves a <b>string</b> value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveItem(XmlDocument doc, XmlElement parent, string itemName, string itemValue)
        {
            XmlElement item = doc.CreateElement(itemName);
            item.InnerText = itemValue;
            parent.AppendChild(item);
        }

        /// <summary>
        /// Saves a <b>bool</b> value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveBoolItem(XmlDocument doc, XmlElement parent, string itemName, bool itemValue)
        {
            SaveItem(doc, parent, itemName, itemValue ? "T" : "F");
        }

        /// <summary>
        /// Saves an <b>int</b> value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveIntItem(XmlDocument doc, XmlElement parent, string itemName, int itemValue)
        {
            SaveItem(doc, parent, itemName, itemValue.ToString());
        }

        /// <summary>
        /// Saves an <b>decimal</b> value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveDecimalItem(XmlDocument doc, XmlElement parent, string itemName, decimal itemValue)
        {
            SaveItem(doc, parent, itemName, itemValue.ToString(NumberFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Saves an <b>decimal</b> or <b>null</b>value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveDecimalItem(XmlDocument doc, XmlElement parent, string itemName, decimal? itemValue)
        {
            SaveItem(doc, parent, itemName, itemValue == null ? string.Empty : ((decimal)itemValue).ToString(NumberFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Saves a <b>DateTime</b> value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveDateItem(XmlDocument doc, XmlElement parent, string itemName, DateTime itemValue)
        {
            SaveItem(doc, parent, itemName, itemValue.ToString(DateTimeFormatInfo.InvariantInfo));
        }

        /// <summary>
        /// Saves an <b>DateTime</b> or <b>null</b>value of param set to a XML document.
        /// </summary>
        /// <param name="doc">A XML document to save data to.</param>
        /// <param name="parent">A parent node to add new item to.</param>
        /// <param name="itemName">An item name to save.</param>
        /// <param name="itemValue">An item value to save.</param>
        public static void SaveDateItem(XmlDocument doc, XmlElement parent, string itemName, DateTime? itemValue)
        {
            if (itemValue == null)
            {
                SaveItem(doc, parent, itemName, string.Empty);
            }
            else
            {
                SaveDateItem(doc, parent, itemName, itemValue.Value);
            }
        }
        /// <summary>
        /// Loads a <see cref="XmlNode"/> from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <returns>Returns a <see cref="XmlNode"/> or <b>null</b> if a specified node was not found.</returns>
        public static XmlNode LoadNode(XmlDocument doc, string parentName, string itemName)
        {
            return doc.SelectSingleNode(string.Format("//{0}/{1}", parentName, itemName));
        }

        /// <summary>
        /// Loads a <b>string</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns a <b>string</b> value of a specified item.</returns>
        public static string LoadItem(XmlDocument doc, string parentName, string itemName, string defaultValue)
        {
            string itemValue = defaultValue;

            XmlNode node = LoadNode(doc, parentName, itemName);
            if (node != null)
            {
                itemValue = node.InnerText;
            }

            return itemValue;
        }

        /// <summary>
        /// Loads a <b>bool</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns a <b>bool</b> value of a specified item.</returns>
        public static bool LoadBoolItem(XmlDocument doc, string parentName, string itemName, bool defaultValue)
        {
            return LoadItem(doc, parentName, itemName, defaultValue ? "T" : "F") == "T";
        }

        /// <summary>
        /// Loads an <b>int</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns an <b>int</b> value of a specified item.</returns>
        public static int LoadIntItem(XmlDocument doc, string parentName, string itemName, int defaultValue)
        {
            return int.Parse(LoadItem(doc, parentName, itemName, defaultValue.ToString()));
        }

        /// <summary>
        /// Loads an <b>decimal</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns an <b>int</b> value of a specified item.</returns>
        public static decimal LoadDecimalItem(XmlDocument doc, string parentName, string itemName, decimal defaultValue)
        {
            string str = LoadItem(doc, parentName, itemName, defaultValue.ToString(NumberFormatInfo.InvariantInfo));

            // Replace SK decimal separator ',' with invariant decimal separator '.'
            // Until 9.3.2016 wasn't used in SaveDecimalItem invariant decimal separator
            return decimal.Parse(str.Replace(',', '.'), NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Loads an <b>decimal</b> or a <b>null</b>value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns an <b>int</b> value of a specified item.</returns>
        public static decimal? LoadDecimalItem(XmlDocument doc, string parentName, string itemName, decimal? defaultValue)
        {
            string str = LoadItem(doc, parentName, itemName, defaultValue == null ? string.Empty : ((decimal)defaultValue).ToString(NumberFormatInfo.InvariantInfo));

            // Replace SK decimal separator ',' with invariant decimal separator '.'
            // Until 9.3.2016 wasn't used in SaveDecimalItem invariant decimal separator
            return string.IsNullOrEmpty(str) ? null : (decimal?)decimal.Parse(str.Replace(',', '.'), NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Loads a <b>DateTime</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns an <b>int</b> value of a specified item.</returns>
        public static DateTime LoadDateItem(XmlDocument doc, string parentName, string itemName, DateTime defaultValue)
        {
            return DateTime.Parse(LoadItem(doc, parentName, itemName, defaultValue.ToString(DateTimeFormatInfo.InvariantInfo)), DateTimeFormatInfo.InvariantInfo);
        }


        /// <summary>
        /// Loads a <b>DateTime</b> value of param set from a XML document.
        /// </summary>
        /// <param name="doc">A XML document to load data from.</param>
        /// <param name="parentName">A parent node name to load item from.</param>
        /// <param name="itemName">An item name to load.</param>
        /// <param name="defaultValue">A default value to be used if item doesn't exist.</param>
        /// <returns>Returns an <b>int</b> value of a specified item.</returns>
        public static DateTime? LoadDateItem(XmlDocument doc, string parentName, string itemName, DateTime? defaultValue)
        {
            string itemValue = LoadItem(doc, parentName, itemName, null);
            if (string.IsNullOrEmpty(itemValue))
            {
                return defaultValue;
            }

            return DateTime.Parse(itemValue, DateTimeFormatInfo.InvariantInfo);
        }
    }
}
