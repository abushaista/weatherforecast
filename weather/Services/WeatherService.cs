using System;
using System.Text.Json;
using weather.Models;

namespace weather.Services;

public class WeatherService: IWeatherService
{
	private HttpClient _client;


    public WeatherService(HttpClient client)
	{
		this._client = client;
	}

    public async Task<CoordinateResponse> GetCoordinate(string CityName)
    {
        var url = "http://api.openweathermap.org/geo/1.0/direct?q={0}&limit=5&appid=173264351cf6a4698333f3b6cd00f96f";
        var result = await _client.GetAsync(string.Format(url, CityName));
        if (result.IsSuccessStatusCode)
        {
            var data = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<List<CoordinateResponse>>(data);
            if (response.Count > 0)
            {
                return response[0];
            }
        }
        return null;
    }

    public async Task<WeatherForecastData> GetCurrentWeather(double lat, double lon, string city)
    {
        var url = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid=173264351cf6a4698333f3b6cd00f96f&units=metric";
        var result = await _client.GetAsync(string.Format(url, lat,lon));
        var weatherData = new WeatherForecastData();
        weatherData.City = city;
        if(result.IsSuccessStatusCode)
        {
            var data = await result.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<WeatherResponse>(data);
            if(response != null)
            {
                weatherData.Weather = string.Format("{0}C/{1}%", response.main.temp, response.main.humidity);
            }
        }
        return weatherData;
    }
}

