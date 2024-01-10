using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


//Note: A lot of repeated code that could be combined into private functions.
public class Blackboard {

    private List<IDictionary> DataLibraries = new List<IDictionary>();
    private Dictionary<string, int> Addresses = new Dictionary<string, int>();

    static private int INVALID_LIBRARY_ADDRESS = -1;



    public void AddEntry<T>(string key, T value) {
        int Index = INVALID_LIBRARY_ADDRESS;
        if (DataLibraries.Count == 0) {
            CreateLibraryAndAddEntry(key, value);
            return;
        }

        Index = GetLibraryIndexByType<T>();
        if (Index == INVALID_LIBRARY_ADDRESS) {
            CreateLibraryAndAddEntry(key, value);
            return;
        }

        var DataLibrary = (Dictionary<string, T>)DataLibraries[Index];
        if (DataLibrary.ContainsKey(key)) {
            Debug.LogWarning("Attempted to add an already existing entry to blackboard!");
            return;
        }
        else
            DataLibrary.Add(key, value);
    }
    public void RemoveEntry<T>(string key) {
        if (DataLibraries.Count == 0)
            return;

        int Index = GetLibraryIndexByType<T>();
        if (Index == INVALID_LIBRARY_ADDRESS) {
            Debug.LogWarning("Attempted to delete entry that doesnt exist from blackboard!");
            return;
        }

        var DataLibrary = (Dictionary<string, T>)DataLibraries[Index];
        if (!DataLibrary.ContainsKey(key)) {
            Debug.LogWarning("Attempted to delete entry that doesnt exist from blackboard!");
            return;
        }

        DataLibrary.Remove(key);
        //Note: Data libraries are not deallocated if empty in case of future reuse.
    }
    public bool GetEntry<T>(string key, out T value) {
        value = default(T);
        if (DataLibraries.Count == 0)
            return false;

        int Index = GetLibraryIndexByType<T>();
        if (Index == INVALID_LIBRARY_ADDRESS) {
            Debug.LogWarning("Attempted to get entry that does not exist in blackboard");
            return false;
        }

        var DataLibrary = (Dictionary<string, T>)DataLibraries[Index];
        if (!DataLibrary.ContainsKey(key)) {
            Debug.LogWarning("Attempted to get entry that does not exist in blackboard");
            return false;
        }

        value = DataLibrary[key];
        return true;
    }
    public void UpdateEntry<T>(string key, T value) {
        if (DataLibraries.Count == 0)
            return;

        int Index = GetLibraryIndexByType<T>();
        if (Index == INVALID_LIBRARY_ADDRESS) {
            Debug.LogWarning("Attempted to update entry that does not exist in blackboard");
            return;
        }

        var DataLibrary = (Dictionary<string, T>)DataLibraries[Index];
        if (!DataLibrary.ContainsKey(key)) {
            Debug.LogWarning("Attempted to update entry that does not exist in blackboard");
            return;
        }

        DataLibrary[key] = value;
    }


    private int GetLibraryIndexByType<T>() {
        if (DataLibraries.Count == 0)
            return INVALID_LIBRARY_ADDRESS;

        var Type = typeof(T);
        int Index;
        if (!Addresses.TryGetValue(Type.ToString(), out Index))
            return INVALID_LIBRARY_ADDRESS;

        return Index;
    }
    private int CreateNewLibrary<T>() {
        Dictionary<string, T> NewDataLibrary = new Dictionary<string, T>();
        DataLibraries.Add(NewDataLibrary);
        Addresses.Add(typeof(T).ToString(), DataLibraries.Count - 1);
        return DataLibraries.Count - 1;
    }
    private void CreateLibraryAndAddEntry<T>(string key, T value) {
        int Index = CreateNewLibrary<T>();
        DataLibraries[Index].Add(key, value);
    }
}
