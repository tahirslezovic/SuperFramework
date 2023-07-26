using SuperFramework.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SuperFramework.Core
{
    public interface ISaveData
    {

    };

    /// <summary>
    /// System that handles save game data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SaveGameSubsystem<T> : ISubsystem where T : ISaveData, new()
    {
        public string Name => nameof(SaveGameSubsystem<T>);

        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// Last loaded data.
        /// </summary>
        public ISaveData Data { get; protected set; }

        /// <summary>
        /// Check if save file exists
        /// </summary>
        public bool DoesSaveFileExists => File.Exists(Path.Combine(_path, _filename + _extension));


        private readonly string _path;
        private readonly string _filename;
        private readonly string _extension;
        private readonly ILogger _logger;


        public SaveGameSubsystem(string path, string filename, string extension, ILogger logger)
        {
            _path = path;
            _filename = filename;
            _extension = extension;
            _logger = logger;
        }

        public virtual void InitializationFailed(ILogger logger, Exception e)
        {

        }

        public virtual Task InitializeAsync(ILogger logger)
        {
            if (IsInitialized)
                return Task.CompletedTask;

            LoadData();


            IsInitialized = true;
            return Task.CompletedTask;
        }

        public virtual Task PauseSystemAsync()
        {
            SaveData((T)Data);

            return Task.CompletedTask;
        }

        public virtual Task ResumeSystemAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task ShutdownAsync()
        {
            SaveData((T)Data);
            return Task.CompletedTask;
        }

        public virtual void SaveData(T data)
        {
            if (string.IsNullOrEmpty(_path) || string.IsNullOrWhiteSpace(_path))
            {
                _logger.LogWarning("Save path is null or empty!", Name);
                return;
            }

            using (Stream stream = File.Open(Path.Combine(_path, _filename + _extension), FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                try
                {
                    binaryFormatter.Serialize(stream, data);
                    _logger.Log("Data successfully saved!", Name);
                }
                catch (Exception e)
                {
                    _logger.LogException("There was a problem saving data, exception",e, Name);
                }
            }
            Data = data;
        }

        public virtual void SaveData()
        {
            SaveData((T)Data);
        }

        public virtual T LoadData()
        {
            if (string.IsNullOrEmpty(_path) || string.IsNullOrWhiteSpace(_path))
            {
                _logger.LogWarning("Path is null or empty!", Name);
                return default(T);
            }

            if (!File.Exists(Path.Combine(_path, _filename + _extension)))
            {
                _logger.Log("Saved data doesn't exists, creating a new one.", Name);
                Data = new T();
                return (T)Data;
            }

            using (Stream stream = File.Open(Path.Combine(_path, _filename + _extension), FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                try
                {
                    Data = (T)binaryFormatter.Deserialize(stream);
                    _logger.Log("Saved data loaded successfully!", Name);
                }
                catch (Exception e)
                {
                    _logger.Log("There was a problem loading saved data, exception: " + e.ToString(), Name);
                }
            }

            return (T)Data;

        }

    }
}
