
using System.Collections;
using SuperFramework.Interfaces;
using UnityEngine.Networking;

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
            using (var request = new UnityWebRequest())
            {
                request.url = Url;
                request.method = UnityWebRequest.kHttpVerbGET;
                request.timeout = 10;


                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    _logger?.LogError($"Probe {Url} reports offline status!", Id);
                    Online = false;
                }
                else
                {
                    Online = true;
                }
            }
        }

        #endregion
    }

}