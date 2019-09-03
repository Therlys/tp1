using System;
using System.Data.Common;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

namespace Harmony
{
    public class SqLiteConnectionFactory : MonoBehaviour
    {
        //#Dirty Fix : Unity player (not Editor) does not load DLLs that depends on other DLLs
#if !UNITY_EDITOR
        static void SqLiteRepository()
        {
            string path = System.Environment.GetEnvironmentVariable("PATH", System.EnvironmentVariableTarget.Process);
            string pluginPath = UnityEngine.Application.dataPath + System.IO.Path.DirectorySeparatorChar + "Plugins";
            if (path.Contains(pluginPath) == false)
            {
                System.Environment.SetEnvironmentVariable("PATH", path + System.IO.Path.PathSeparator + pluginPath,
                                                          System.EnvironmentVariableTarget.Process);
            }
        }

#endif
        
        private const string SQLITE_CONNECTION_TEMPLATE = "URI=file:{0}";

        [SerializeField] private string databaseFileName = "Database.db";

        private string connexionString;

        private void Awake()
        {
            connexionString = GetConnexionString();
        }
        
        public DbConnection GetConnection()
        {
            CreateDatabaseIfDoesntExits();
            
            return new SqliteConnection(connexionString);
        }

        public string GetCurrentDatabaseFilePath()
        {
            return Path.Combine(ApplicationExtensions.PersistentDataPath, databaseFileName);
        }

        public string GetSourceDatabaseFilePath()
        {
            return Path.Combine(ApplicationExtensions.ApplicationDataPath, databaseFileName);
        }

        public void CreateDatabaseIfDoesntExits()
        {
            if (IsCurrentDatabaseDoesntExists())
            {
                File.Copy(GetSourceDatabaseFilePath(), GetCurrentDatabaseFilePath(), true);
            }
        }

        public void ResetDatabase()
        {
            if (IsCurrentDatabaseExists())
            {
                File.Delete(GetCurrentDatabaseFilePath());
            }
        }

        public bool IsCurrentDatabaseExists()
        {
            return File.Exists(GetCurrentDatabaseFilePath());
        }

        public bool IsCurrentDatabaseDoesntExists()
        {
            return !IsCurrentDatabaseExists();
        }

        public bool IsSourceDatabaseExists()
        {
            return File.Exists(GetSourceDatabaseFilePath());
        }

        public bool IsSourceDatabaseDoesntExists()
        {
            return !IsSourceDatabaseExists();
        }

        private string GetConnexionString()
        {
            return String.Format(SQLITE_CONNECTION_TEMPLATE, GetCurrentDatabaseFilePath());
        }
    }
}
