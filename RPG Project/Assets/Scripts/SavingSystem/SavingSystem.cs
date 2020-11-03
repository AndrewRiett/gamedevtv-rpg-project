using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

namespace RPG.SavingSystem
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");

            FileStream stream = File.Open(path, FileMode.Create);
            byte[] byteUTF = Encoding.UTF8.GetBytes("Hello World¡");
            stream.Write(byteUTF, 0, byteUTF.Length);
            stream.Close();
        }

        public void Load(string saveFile)
        {
            Debug.Log($"Loading from {GetPathFromSaveFile(saveFile)}");
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}