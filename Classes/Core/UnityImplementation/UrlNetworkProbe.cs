using System.Collections;
using SuperFramework.Interfaces;
using UnityEngine.Networking;
using ILogger = SuperFramework.Interfaces.ILogger;

namespace SuperFramework.Classes.Core
{
    public class UrlNetworkProbe : INetworkProbe
    {

        #region Fields


        private float _passedTime, _repeatDelay;

        private ILogger _logger;

        #endregion

        #region Properties

        public string Id => nameof(UrlNetworkProbe);

        public string Url { get; protected set; }

        public bool Online { get; protected set; }

        #endregion

        #region Constructor

        public UrlNetworkProbe(string url, float repeatDelay, ILogger logger)
        {
            Url = url;
            _passedTime = 0f;
            _repeatDelay = repeatDelay;
            _logger = logger;
        }

        #endregion

        #region API

        public void Check()
        {
            _logger?.Log($"Probe check {Url}");

            GameContext.Instance.StartCoroutine(CheckIE());
        }

        public void Update(float deltaTime)
        {
            _passedTime += deltaTime;
            if(_passedTime >= _repeatDelay)
            {
                _passedTime = 0f;
                Check();
            }
        }

        #endregion

        #region Private

        private IEnumerator CheckIE()
        {
            UnityWebRequest www = UnityWebRequest.Get(Url);

            // Send the request
            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.result != UnityWebRequest.Result.ConnectionError)
            {
                _logger?.LogError($"Probe {Url} reports online status!", Id);
                Online = true;
            }
            else
            {
                _logger?.LogError($"Probe {Url} reports offline status!", Id);
                Online = false;
            }
        }

        #endregion
    }

}