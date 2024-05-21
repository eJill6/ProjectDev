using FreeRedis;
using Microsoft.Extensions.Options;
using MMService.Models.Settings;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MMService.Services
{
    /// <inheritdoc cref="ICityService"/>
    public class CityService : ICityService
    {
        /// <inheritdoc cref="Dictionary{string, CityInfo}"/>
        private ConcurrentDictionary<string, CityInfo> _citiesDic = new ConcurrentDictionary<string, CityInfo>();


        private IOptionsMonitor<IEnumerable<CitiesData>> _optionsCityInfo; 

        private readonly ILogger<CityService> _logger = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="config">設定檔</param>
        public CityService(IConfiguration config, IOptionsMonitor<IEnumerable<CitiesData>> optionsCityInfo,
            ILogger<CityService> logger)
        {
            _logger = logger;
            _optionsCityInfo = optionsCityInfo;
            _ = Task.Run(() =>
            {
                try
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    _logger.LogInformation("CityService Init Start");
                    OnChange(config);
                    watch.Stop();
                    _logger.LogInformation($"CityService Init End ElapsedMilliseconds: {watch.ElapsedMilliseconds}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CityService Init fail");
                }
            });
        }

        /// <inheritdoc/>
        public string Get(string areaCode)
        {
            if (_citiesDic.ContainsKey(areaCode))
            {
                return _citiesDic[areaCode].Name;
            }
            return areaCode;
        }

        public CitiesData[] GetProvinceInCityInfo(string areaCode)
        {

            var provinceCode = _optionsCityInfo.CurrentValue.Where(c => c.Code == areaCode).First().Province;
            var cityInfos = _optionsCityInfo.CurrentValue.Where(c => c.Province == provinceCode).ToArray();
            return cityInfos;


        }

        private void OnChange(IConfiguration config)
        {
            var raw = config.GetSection("CitiesData");

            var cityList = raw.Get<CityInfo[]>();
            if (cityList?.Any() == true)
            {
                var rawResult = cityList.ToDictionary(p => p.Code, p => p);
                //var rawResult = raw.Get<Dictionary<string, CityInfo>>();
                //_citiesDic.Clear();
                _citiesDic = new ConcurrentDictionary<string, CityInfo>(rawResult.Select(x => x));
                //_citiesDic.TryAdd("00", new CityInfo()
                //{
                //    C = "00",
                //    N = "全国",
                //});

                //var valu = JsonConvert.SerializeObject(_citiesDic);
                //int i = 0;
            }
        }
    }
}