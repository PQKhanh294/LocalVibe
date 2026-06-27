import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Compass, User, Menu, Globe, MapPin } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';
import AuthModal from './AuthModal';
import './Navbar.css';

// Simple weather code mapper
const getWeatherInfo = (code: number) => {
  if (code === 0) return { icon: '☀️', text: 'Nắng' };
  if (code === 1 || code === 2) return { icon: '🌤️', text: 'Có mây' };
  if (code === 3) return { icon: '☁️', text: 'Nhiều mây' };
  if (code === 45 || code === 48) return { icon: '🌫️', text: 'Sương mù' };
  if (code >= 51 && code <= 55) return { icon: '🌧️', text: 'Mưa phùn' };
  if (code === 61 || code === 63) return { icon: '🌧️', text: 'Mưa nhỏ' };
  if (code === 65 || code >= 80 && code <= 82) return { icon: '☔', text: 'Mưa rào' };
  if (code >= 71 && code <= 77) return { icon: '❄️', text: 'Tuyết' };
  if (code === 95) return { icon: '⛈️', text: 'Mưa dông' };
  return { icon: '⛈️', text: 'Dông lốc' };
};

export default function Navbar() {
  const { user, logout } = useAuth();
  const [isAuthModalOpen, setIsAuthModalOpen] = useState(false);
  const [weather, setWeather] = useState<{ temp: number; icon: string; city?: string } | null>(null);

  useEffect(() => {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(async (position) => {
        const { latitude, longitude } = position.coords;
        try {
          // Fetch weather including humidity
          const res = await fetch(`https://api.open-meteo.com/v1/forecast?latitude=${latitude}&longitude=${longitude}&current=temperature_2m,relative_humidity_2m,weather_code`);
          const data = await res.json();
          const wCode = data.current.weather_code;
          const temp = data.current.temperature_2m;
          const humidity = data.current.relative_humidity_2m;
          
          // Reverse geocoding for city name (using a free api or just displaying 'Vị trí của bạn')
          // To keep it simple and reliable without API keys, we just use a generic label or fetch from open-meteo geocoding if needed.
          // BigDataCloud offers a free client-side reverse geocoding API
          let city = 'Vị trí của bạn';
          try {
            const geoRes = await fetch(`https://api.bigdatacloud.net/data/reverse-geocode-client?latitude=${latitude}&longitude=${longitude}&localityLanguage=vi`);
            const geoData = await geoRes.json();
            if (geoData.city || geoData.locality) {
              city = geoData.city || geoData.locality;
            }
          } catch (e) {
            // fallback
          }

          setWeather({
            temp: Math.round(temp),
            humidity: humidity,
            icon: getWeatherInfo(wCode).icon,
            city
          });
        } catch (error) {
          console.error("Failed to fetch weather", error);
        }
      }, () => {
        console.log("Geolocation permission denied");
      });
    }
  }, []);

  return (
    <nav className="navbar">
      <div className="navbar-container container">
        {/* Left side: Logo + Weather */}
        <div className="navbar-left" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <Link to="/" className="navbar-logo" title="Trang chủ LocalVibe">
            <div className="app-icon-logo">
              <Compass size={24} color="white" />
            </div>
          </Link>

          {weather && (
            <div className="nav-weather" title={`Thời tiết tại ${weather.city}`}>
              <MapPin size={14} className="weather-pin" />
              <span className="weather-city">{weather.city}</span>
              <span className="weather-divider">|</span>
              <span className="weather-icon">{weather.icon}</span>
              <span className="weather-temp">{weather.temp}°C</span>
            </div>
          )}
        </div>
        
        <div className="navbar-center hidden-mobile">
          <button className="nav-link">Nơi ở</button>
          <button className="nav-link active">Trải nghiệm</button>
          <button className="nav-link">Trực tuyến</button>
        </div>

        <div className="navbar-actions">
          <button className="btn-host hidden-mobile" title="Tính năng sắp ra mắt">Cho thuê chỗ ở</button>
          <button className="btn-icon" title="Tính năng đổi ngôn ngữ sắp ra mắt">
            <Globe size={20} />
          </button>
          
          {user ? (
            <button className="user-menu" onClick={logout} title="Đăng xuất">
              <span style={{ fontWeight: 'bold', fontSize: '0.9rem', padding: '0 4px', color: 'var(--text-primary)' }}>
                {user.username}
              </span>
              <div className="user-avatar" style={{ background: 'var(--accent-primary)' }}>
                {user.username.charAt(0).toUpperCase()}
              </div>
            </button>
          ) : (
            <button className="user-menu" onClick={() => setIsAuthModalOpen(true)} title="Đăng nhập / Đăng ký">
              <Menu size={18} className="text-secondary" />
              <div className="user-avatar">
                <User size={18} />
              </div>
            </button>
          )}
        </div>
      </div>

      <AuthModal isOpen={isAuthModalOpen} onClose={() => setIsAuthModalOpen(false)} />
    </nav>
  );
}
