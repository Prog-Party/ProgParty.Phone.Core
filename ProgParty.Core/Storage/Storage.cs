using System;
using System.Collections.Generic;

namespace ProgParty.Core.Storage
{
    public class StorageKeys
    {
        public const string Review = "review";
        public const string ReviewDone = "reviewdone";

        //public const string LoggedInName = "loggedInName";
        //public const string LoggedInPassword = "loggedInPassword";

        //public const string ProfileName = "profileName";
        //public const string ProfileZipcode = "profileZipCode";
        //public const string ProfilePhoneNumber = "profilePhoneNumber";

        //public const string AppMessage = "AppMessage";
    }

    public class Storage
    {
        private Windows.Storage.ApplicationDataContainer _roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
        private Windows.Storage.ApplicationDataContainer _localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public Storage()
        {
        }

        public void StoreInRoaming(string key, object value)
        {
            //Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            //if(roamingSettings.Containers.ContainsKey(key))
            _roamingSettings.Values[key] = value;
            //else
            //    roamingSettings.Values[key] = 0;
        }

        public void StoreInRoaming(string key, Dictionary<object, object> values)
        {
            Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;

            Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();

            foreach (KeyValuePair<object, object> pair in values)
                composite[pair.Key.ToString()] = pair.Value;

            //if (roamingSettings.Values.ContainsKey(key))
            _roamingSettings.Values[key] = composite;
        }

        private object LoadFrom(Windows.Storage.ApplicationDataContainer container, string key)
        {
            if (!container.Values.ContainsKey(key))
                return null;

            return container.Values[key];
        }

        internal string LoadFromLocal(object profileName)
        {
            throw new NotImplementedException();
        }

        private int LoadIntFrom(Windows.Storage.ApplicationDataContainer container, string key)
        {
            var result = LoadFrom(container, key);
            if (result == null)
                return 0;
            int i;
            if (!int.TryParse(result.ToString(), out i))
                return 0;
            return i;
        }

        private bool LoadBoolFrom(Windows.Storage.ApplicationDataContainer container, string key)
        {
            var result = LoadFrom(container, key);
            if (result == null)
                return false;
            bool i;
            if (!bool.TryParse(result.ToString(), out i))
                return false;
            return i;
        }


        public object LoadFromRoaming(string key) => LoadFrom(_roamingSettings, key);
        public int LoadIntFromRoaming(string key) => LoadIntFrom(_roamingSettings, key);
        internal bool LoadBoolFromRoaming(string key) => LoadBoolFrom(_roamingSettings, key);
        public object LoadFromLocal(string key) => LoadFrom(_localSettings, key);
        public int LoadIntFromLocal(string key) => LoadIntFrom(_localSettings, key);
        internal bool LoadBoolFromLocal(string key) => LoadBoolFrom(_localSettings, key);

        public void StoreInLocal(string key, object value)
        {
            //Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            //if(roamingSettings.Containers.ContainsKey(key))
            _localSettings.Values[key] = value;
            //else
            //    roamingSettings.Values[key] = 0;
        }
    }
}
