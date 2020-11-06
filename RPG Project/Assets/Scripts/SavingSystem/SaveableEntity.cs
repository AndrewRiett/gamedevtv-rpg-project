using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Collections;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookUp = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();

                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // for making sure that we don't change a prefab's GUID

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !isUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookUp[property.stringValue] = this;
        }
#endif
        private bool isUnique(string candidate)
        {
            if (!globalLookUp.ContainsKey(candidate)) { return true; }
            if (globalLookUp[candidate] == this) { return true; }

            // means that it was destroyed in unity
            if (globalLookUp[candidate] == null)
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            // if for some reason the uuid and the uuid in dictionary are different (manual changing)
            if (globalLookUp[candidate].GetUniqueIdentifier() != candidate) {
                return true;
            }

            return false;
        }
    }
}