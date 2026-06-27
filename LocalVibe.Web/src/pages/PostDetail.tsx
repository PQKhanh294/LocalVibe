import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  ArrowLeft, MapPin, Star, ThumbsUp, ThumbsDown, 
  Cloud, Wind, Droplets, Send, Clock, ChefHat,
  Coffee, Utensils, Hotel, Mountain, Camera, ExternalLink
} from 'lucide-react';
import { postApi } from '../api/postApi';
import { weatherApi } from '../api/weatherApi';
import { foodApi } from '../api/foodApi';
import { commentApi } from '../api/commentApi';
import { voteApi } from '../api/voteApi';
import type { PostDetailResponse } from '../types/post.types';
import type { WeatherResponse } from '../types/weather.types';
import type { FoodInfoResponse } from '../types/food.types';
import type { CommentResponse } from '../types/comment.types';
import './PostDetail.css';

const TAG_LABELS: Record<string, string> = {
  Food: 'Ẩm thực',
  Coffee: 'Cà phê',
  ScenicRoute: 'Cung đường đẹp',
  Hotel: 'Nơi lưu trú',
  Attraction: 'Điểm tham quan',
};

const getTagIcon = (tag: string) => {
  switch (tag) {
    case 'Coffee': return <Coffee size={18} />;
    case 'Food': return <Utensils size={18} />;
    case 'Hotel': return <Hotel size={18} />;
    case 'ScenicRoute': return <Mountain size={18} />;
    case 'Attraction': return <Camera size={18} />;
    default: return <MapPin size={18} />;
  }
};

function generateVoterToken(): string {
  let token = localStorage.getItem('voter_token');
  if (!token) {
    token = 'voter_' + Math.random().toString(36).substring(2, 15) + Date.now();
    localStorage.setItem('voter_token', token);
  }
  return token;
}

export default function PostDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const [post, setPost] = useState<PostDetailResponse | null>(null);
  const [weather, setWeather] = useState<WeatherResponse | null>(null);
  const [foodInfo, setFoodInfo] = useState<FoodInfoResponse | null>(null);
  const [comments, setComments] = useState<CommentResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Vote state
  const [upVotes, setUpVotes] = useState(0);
  const [downVotes, setDownVotes] = useState(0);
  const [isVoting, setIsVoting] = useState(false);

  // Extended Weather state
  const [extendedWeather, setExtendedWeather] = useState<{
    current: { temp: number; icon: string; condition: string; wind: number; precip: number; city: string };
    forecast: Array<{ date: string; maxTemp: number; minTemp: number; icon: string; condition: string }>;
  } | null>(null);

  // Comment form
  const [authorName, setAuthorName] = useState('');
  const [commentContent, setCommentContent] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (!id) return;
    const postId = parseInt(id);

    const fetchData = async () => {
      setLoading(true);
      try {
        // Fetch post detail
        const postData = await postApi.getPostById(postId);
        setPost(postData);
        setComments(postData.comments || []);
        setUpVotes(postData.upVotes);
        setDownVotes(postData.downVotes);

        // Fetch weather (directly from open-meteo for 3-day forecast)
        const fetchWeather = async () => {
          try {
            const res = await fetch(`https://api.open-meteo.com/v1/forecast?latitude=${postData.latitude}&longitude=${postData.longitude}&current=temperature_2m,weather_code,wind_speed_10m,precipitation&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=auto&forecast_days=4`);
            const data = await res.json();
            
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

            const cityName = postData.address ? postData.address.split(',').pop()?.trim() || 'địa điểm này' : 'địa điểm này';
            const currentInfo = getWeatherInfo(data.current.weather_code);

            // Bỏ qua ngày hôm nay (index 0) để tránh gây nhầm lẫn với thời tiết hiện tại
            const forecast = data.daily.time.slice(1).map((timeStr: string, sliceIdx: number) => {
              const idx = sliceIdx + 1;
              const info = getWeatherInfo(data.daily.weather_code[idx]);
              const dateObj = new Date(timeStr);
              return {
                date: idx === 1 ? 'Ngày mai' : dateObj.toLocaleDateString('vi-VN', { weekday: 'short', day: 'numeric', month: 'numeric' }),
                maxTemp: Math.round(data.daily.temperature_2m_max[idx]),
                minTemp: Math.round(data.daily.temperature_2m_min[idx]),
                icon: info.icon,
                condition: info.text
              };
            });

            setExtendedWeather({
              current: {
                temp: Math.round(data.current.temperature_2m),
                icon: currentInfo.icon,
                condition: currentInfo.text,
                wind: data.current.wind_speed_10m,
                precip: data.current.precipitation,
                city: cityName
              },
              forecast
            });
          } catch (e) {
            console.error("Failed to fetch extended weather", e);
          }
        };
        fetchWeather();

        // Fetch food info if tag is Food
        if (postData.tag === 'Food') {
          foodApi.getByPostId(postId)
            .then(f => setFoodInfo(f))
            .catch(() => {});
        }
      } catch (err) {
        console.error(err);
        setError('Không thể tải thông tin địa điểm.');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleVote = async (type: 'Up' | 'Down') => {
    if (!id || isVoting) return;
    setIsVoting(true);
    try {
      const result = await voteApi.vote(parseInt(id), {
        voteType: type,
        voterToken: generateVoterToken(),
      });
      setUpVotes(result.upVotes);
      setDownVotes(result.downVotes);
    } catch (err) {
      console.error('Vote error:', err);
    } finally {
      setIsVoting(false);
    }
  };

  const handleComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id || !authorName.trim() || !commentContent.trim() || isSubmitting) return;
    setIsSubmitting(true);
    try {
      const newComment = await commentApi.addComment(parseInt(id), {
        authorName: authorName.trim(),
        content: commentContent.trim(),
      });
      setComments(prev => [newComment, ...prev]);
      setCommentContent('');
    } catch (err) {
      console.error('Comment error:', err);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="post-detail-page">
        <div className="container">
          <div className="loading-state">
            <div className="spinner"></div>
            <p>Đang tải thông tin...</p>
          </div>
        </div>
      </div>
    );
  }

  if (error || !post) {
    return (
      <div className="post-detail-page">
        <div className="container">
          <div className="error-state">
            <p>{error || 'Không tìm thấy bài viết.'}</p>
            <button className="btn-primary" onClick={() => navigate('/')}>Về trang chủ</button>
          </div>
        </div>
      </div>
    );
  }

  const displayTag = TAG_LABELS[post.tag] || post.tag;
  const netScore = upVotes - downVotes;
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL?.replace('/api', '') || 'https://localhost:7011';
  const imageUrl = post.imageUrl?.startsWith('http') ? post.imageUrl : (post.imageUrl ? `${apiBaseUrl}${post.imageUrl}` : '');
  
  // Deterministic Calorie Estimator (300 - 800 kcal)
  const getCalories = (mealName: string) => {
    let hash = 0;
    for (let i = 0; i < mealName.length; i++) {
      hash = mealName.charCodeAt(i) + ((hash << 5) - hash);
    }
    return 300 + (Math.abs(hash) % 500);
  };

  return (
    <div className="post-detail-page">
      <div className="container">
        {/* Back button */}
        <button className="back-btn animate-fade-in-up" onClick={() => navigate('/')}>
          <ArrowLeft size={20} /> Quay lại
        </button>

        <div className="detail-layout animate-fade-in-up delay-100">
          {/* Main Content */}
          <div className="detail-main">
            {/* Hero Image / Image Grid */}
            {imageUrl || (post.additionalImages && post.additionalImages.length > 0) ? (
              <div className="detail-image-gallery">
                <div className="detail-image-main">
                  <img src={imageUrl || post.additionalImages![0]} alt={post.title} className="detail-image" />
                </div>
                {post.additionalImages && post.additionalImages.length > 0 && (
                  <div className="detail-image-side">
                    {post.additionalImages.slice(0, 2).map((img, idx) => (
                      <div key={idx} className="detail-image-small">
                        <img src={img} alt={`${post.title} - ảnh ${idx + 1}`} className="detail-image" />
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ) : (
              <div className="detail-placeholder">
                {getTagIcon(post.tag)}
                <span>{displayTag}</span>
              </div>
            )}

            {/* Title & Meta */}
            <div className="detail-header">
              <div className="detail-tag-row">
                <span className="detail-tag">{getTagIcon(post.tag)} {displayTag}</span>
                <span className="detail-date">
                  <Clock size={14} /> {new Date(post.createdAt).toLocaleDateString('vi-VN')}
                </span>
              </div>
              <h1 className="detail-title">{post.title}</h1>
              {post.description && (
                <p className="detail-description">{post.description}</p>
              )}
            </div>

            {/* Vote Section */}
            <div className="vote-section">
              <button 
                className="vote-btn vote-up" 
                onClick={() => handleVote('Up')}
                disabled={isVoting}
              >
                <ThumbsUp size={20} /> {upVotes}
              </button>
              <div className={`vote-score ${netScore > 0 ? 'positive' : netScore < 0 ? 'negative' : ''}`}>
                <Star size={18} fill="currentColor" /> {netScore > 0 ? `+${netScore}` : netScore}
              </div>
              <button 
                className="vote-btn vote-down" 
                onClick={() => handleVote('Down')}
                disabled={isVoting}
              >
                <ThumbsDown size={20} /> {downVotes}
              </button>
            </div>

            {/* Food Info Widget (only for Food tag) */}
            {foodInfo && foodInfo.relatedMeals.length > 0 && (
              <div className="widget food-widget animate-fade-in-up">
                <h3 className="widget-title"><ChefHat size={20} /> Món ăn liên quan</h3>
                <div className="meal-grid">
                  {foodInfo.relatedMeals.map((meal, index) => (
                    <div key={index} className="meal-card">
                      {meal.thumbnailUrl && (
                        <img src={meal.thumbnailUrl} alt={meal.name} className="meal-img" />
                      )}
                      <div className="meal-info">
                        <h4>{meal.name}</h4>
                        <div className="meal-meta-group">
                          {meal.category && <span className="meal-meta">{meal.category}</span>}
                          {meal.area && <span className="meal-meta">{meal.area}</span>}
                          <span className="meal-meta meal-calories">🔥 ~{getCalories(meal.name)} kcal</span>
                        </div>
                        {meal.youtubeUrl && (
                          <a href={meal.youtubeUrl} target="_blank" rel="noopener noreferrer" className="meal-link">
                            <ExternalLink size={14} /> Xem video
                          </a>
                        )}
                      </div>
                    </div>
                  ))}
                </div>
                <p className="widget-provider">Nguồn: {foodInfo.provider}</p>
              </div>
            )}

            {/* Comments Section */}
            <div className="comments-section animate-fade-in-up">
              <h3 className="widget-title">💬 Bình luận ({comments.length})</h3>
              
              <form className="comment-form" onSubmit={handleComment}>
                <input
                  type="text"
                  placeholder="Tên của bạn"
                  value={authorName}
                  onChange={e => setAuthorName(e.target.value)}
                  className="comment-input"
                  required
                />
                <div className="comment-input-row">
                  <input
                    type="text"
                    placeholder="Viết bình luận..."
                    value={commentContent}
                    onChange={e => setCommentContent(e.target.value)}
                    className="comment-input comment-content-input"
                    required
                  />
                  <button type="submit" className="comment-submit" disabled={isSubmitting}>
                    <Send size={18} />
                  </button>
                </div>
              </form>

              <div className="comments-list">
                {comments.length === 0 ? (
                  <p className="no-comments">Chưa có bình luận nào. Hãy là người đầu tiên!</p>
                ) : (
                  comments.map(c => (
                    <div key={c.id} className="comment-item">
                      <div className="comment-avatar">{c.authorName.charAt(0).toUpperCase()}</div>
                      <div className="comment-body">
                        <div className="comment-header">
                          <span className="comment-author">{c.authorName}</span>
                          <span className="comment-time">{new Date(c.createdAt).toLocaleDateString('vi-VN')}</span>
                        </div>
                        <p className="comment-text">{c.content}</p>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </div>
          </div>

          {/* Sidebar */}
          <aside className="detail-sidebar">
            {/* Weather Widget */}
            {extendedWeather && (
              <div className="widget weather-widget animate-fade-in-up delay-200" style={{ padding: '1.25rem' }}>
                <h3 className="widget-title" style={{ marginBottom: '0.5rem' }}><Cloud size={18} /> Thời tiết {extendedWeather.current.city}</h3>
                
                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '0.75rem' }}>
                  <div className="weather-main" style={{ gap: '0.5rem', marginBottom: 0 }}>
                    <span className="weather-icon" style={{ fontSize: '2rem' }}>{extendedWeather.current.icon}</span>
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                      <span className="weather-temp" style={{ fontSize: '1.5rem', lineHeight: '1' }}>{extendedWeather.current.temp}°C</span>
                      <span style={{ fontSize: '0.8rem', color: 'var(--text-secondary)', fontWeight: 500, marginTop: '2px' }}>{extendedWeather.current.condition}</span>
                    </div>
                  </div>
                  
                  <div className="weather-details" style={{ display: 'flex', flexDirection: 'column', gap: '0.2rem', marginTop: 0 }}>
                    <div className="weather-detail-item" style={{ fontSize: '0.75rem' }}>
                      <Wind size={14} /> {extendedWeather.current.wind} km/h
                    </div>
                    <div className="weather-detail-item" style={{ fontSize: '0.75rem' }}>
                      <Droplets size={14} /> {extendedWeather.current.precip} mm
                    </div>
                  </div>
                </div>
                
                {/* 3-Day Forecast */}
                <div className="weather-forecast" style={{ paddingTop: '0.5rem', borderTop: '1px solid rgba(0,0,0,0.05)' }}>
                  <div className="forecast-list" style={{ display: 'flex', flexDirection: 'column', gap: '0.25rem' }}>
                    {extendedWeather.forecast.map((day, idx) => (
                      <div key={idx} className="forecast-item" style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', fontSize: '0.8rem', padding: '0.2rem 0' }}>
                        <span style={{ width: '60px', fontWeight: 500, color: 'var(--text-primary)' }}>{idx === 0 ? 'Hôm nay' : day.date}</span>
                        <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', width: '70px' }}>
                          <span style={{ fontSize: '1.1rem' }}>{day.icon}</span>
                          <span style={{ color: 'var(--text-secondary)' }}>{day.condition}</span>
                        </div>
                        <div style={{ fontWeight: 600, color: 'var(--text-primary)', textAlign: 'right', flex: 1 }}>
                          {day.minTemp}° <span style={{ color: 'var(--text-secondary)', fontWeight: 400, margin: '0 2px' }}>-</span> {day.maxTemp}°
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            )}

            {/* Location Info */}
            <div className="widget location-widget animate-fade-in-up delay-300">
              <h3 className="widget-title"><MapPin size={20} /> Địa chỉ</h3>
              <p className="location-address" style={{ marginBottom: '1rem', fontWeight: 600, color: 'var(--text-primary)' }}>
                {post.address || 'Đang cập nhật địa chỉ...'}
              </p>
              <a 
                href={`https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(post.address || `${post.latitude},${post.longitude}`)}`}
                target="_blank"
                rel="noopener noreferrer"
                className="btn-map"
              >
                <MapPin size={16} /> Mở Google Maps
              </a>
            </div>
          </aside>
        </div>
      </div>
    </div>
  );
}
