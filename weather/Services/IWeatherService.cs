using System;
using weather.Models;

namespace weather.Services;

public interface IWeatherService
{
    Task<CoordinateResponse> GetCoordinate(string CityName);
    Task<WeatherForecastData> GetCurrentWeather(double lat, double lon, string city);

}

