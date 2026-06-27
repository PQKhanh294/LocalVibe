export interface WeatherResponse {
  temperature: number;
  condition: string;
  conditionIcon: string;
  windSpeed: number;
  precipitation: number;
  unit: string;
  fetchedAt: string;
}
