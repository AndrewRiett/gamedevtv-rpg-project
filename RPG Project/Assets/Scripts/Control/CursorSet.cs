using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "Cursor Set", menuName = "Cursors/New Cursor Set", order = 0)]
    public class CursorSet : ScriptableObject
    {
        [SerializeField] private CursorMapping[] cursorMappings = null;

#pragma warning disable 0649 // to avoid unity never assigned default value error
        
        [System.Serializable]
        private struct CursorMapping
        {
            // assign in inspector:
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }
#pragma warning restore 0649

        private Dictionary<CursorType, CursorMapping> cursorDict = null;

        private void OnEnable() 
        {
            BuildDictionary();
        }

        public void SetCursor(CursorType cursorType)
        {
            CursorMapping cursor = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto); // Unity Cursor API
        }

        private void BuildDictionary()
        {
            if (cursorDict != null) return;

            if (cursorMappings != null)
            {
                cursorDict = new Dictionary<CursorType, CursorMapping>();

                foreach (CursorMapping mapping in cursorMappings)
                {
                    cursorDict[mapping.cursorType] = mapping;
                }
            }
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            if (cursorDict.ContainsKey(type))
            {
                return cursorDict[type];
            }

            Debug.LogError("Cursor type is not found, returning the default variant");
            return cursorMappings[0]; // TODO: might throw null exeptions if cursorMappings == null; 
        }
    }
}

