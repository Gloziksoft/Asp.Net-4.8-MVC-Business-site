using System;
using System.Collections;
using System.Collections.Generic;

namespace dufeksoft.lib.UI
{
    public class CmpDropDown : List<CmpDropDownItem>
    {
        Hashtable htKeyItems = new Hashtable();
        Hashtable htNameItems = new Hashtable();
        Hashtable htIdItems = new Hashtable();

        public CmpDropDownItem AddItem(string name, string dataKey, object data)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            CmpDropDownItem item = new CmpDropDownItem(this.Count + 1, name, dataKey, data);
            this.Add(item);
            htIdItems.Add(item.Id, item);
            htKeyItems.Add(item.DataKey, item);
            if (!htNameItems.ContainsKey(item.Name))
            {
                htNameItems.Add(item.Name, item);
            }

            return item;
        }

        public CmpDropDownItem GetItemForId(int id)
        {
            return htIdItems.ContainsKey(id) ? (CmpDropDownItem)htIdItems[id] : null;
        }

        public CmpDropDownItem GetItemForKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return htKeyItems.ContainsKey(key) ? (CmpDropDownItem)htKeyItems[key] : null;
        }

        public CmpDropDownItem GetItemForName(string name, bool createIfNotFound = false)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (!htNameItems.ContainsKey(name) && createIfNotFound)
            {
                this.AddItem(name, Guid.NewGuid().ToString(), null);
            }
            return htNameItems.ContainsKey(name) ? (CmpDropDownItem)htNameItems[name] : null;
        }

        public string GetItemNameForKey(string key)
        {
            CmpDropDownItem ddi = this.GetItemForKey(key);
            if (ddi == null)
            {
                return string.Empty;
            }

            return ddi.Name;
        }
    }

    public class CmpDropDownItem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string DataKey { get; private set; }
        public object Data { get; private set; }

        public CmpDropDownItem(int id, string name, string dataKey, object data)
        {
            this.Id = id;
            this.Name = name;
            this.DataKey = dataKey;
            this.Data = data;
        }
    }
}
