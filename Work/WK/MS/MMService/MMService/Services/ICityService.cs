using MMService.Models.Settings;

namespace MMService.Services
{
    /// <summary>
    /// 城市相關服務
    /// </summary>
    public interface ICityService
    {
        /// <summary>
        /// 取得城市名稱
        /// </summary>
        /// <param name="areaCode">區域編號</param>
        /// <returns>城市名稱</returns>
        string Get(string areaCode);
        CitiesData[] GetProvinceInCityInfo(string areaCode);
    }
}