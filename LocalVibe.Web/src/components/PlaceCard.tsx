import { Heart, Star, Coffee, Utensils, MapPin, Hotel, Camera, Mountain } from 'lucide-react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './PlaceCard.css';

interface PlaceCardProps {
  id: number;
  title: string;
  tag: string;
  score: number;
  commentsCount: number;
  imageUrl: string;
}

const TAG_LABELS: Record<string, string> = {
  Food: 'Ẩm thực',
  Coffee: 'Cà phê',
  ScenicRoute: 'Cung đường đẹp',
  Hotel: 'Nơi lưu trú',
  Attraction: 'Điểm tham quan',
};

const getCategoryIcon = (tag: string) => {
  const lowerTag = tag.toLowerCase();
  if (lowerTag.includes('coffee') || lowerTag.includes('cà phê')) return <Coffee size={40} strokeWidth={1.5} />;
  if (lowerTag.includes('food') || lowerTag.includes('ẩm thực')) return <Utensils size={40} strokeWidth={1.5} />;
  if (lowerTag.includes('hotel') || lowerTag.includes('lưu trú')) return <Hotel size={40} strokeWidth={1.5} />;
  if (lowerTag.includes('scenicroute') || lowerTag.includes('cung đường')) return <Mountain size={40} strokeWidth={1.5} />;
  if (lowerTag.includes('attraction') || lowerTag.includes('tham quan')) return <Camera size={40} strokeWidth={1.5} />;
  return <MapPin size={40} strokeWidth={1.5} />;
};

export default function PlaceCard({ id, title, tag, score, commentsCount, imageUrl }: PlaceCardProps) {
  const [isSaved, setIsSaved] = useState(false);
  const [imgError, setImgError] = useState(false);
  const navigate = useNavigate();

  const showPlaceholder = !imageUrl || imgError;
  const displayTag = TAG_LABELS[tag] || tag;

  return (
    <div className="place-card bento-card" onClick={() => navigate(`/posts/${id}`)}>
      <div className="place-card-image">
        {showPlaceholder ? (
          <div className="placeholder-image">
            <div className="placeholder-icon-wrapper">
              {getCategoryIcon(tag)}
            </div>
          </div>
        ) : (
          <img 
            src={imageUrl} 
            alt={title} 
            onError={() => setImgError(true)} 
          />
        )}
        
        {/* Floating Heart Icon (Airbnb style) */}
        <button 
          className="save-btn" 
          onClick={(e) => {
            e.stopPropagation();
            setIsSaved(!isSaved);
          }}
        >
          <Heart 
            size={24} 
            className={isSaved ? "heart-filled text-teal" : "heart-outline"} 
            fill={isSaved ? "currentColor" : "rgba(0,0,0,0.5)"} 
          />
        </button>

        {/* Floating Tag */}
        <span className="place-card-tag pill-tag">{displayTag}</span>
      </div>
      
      <div className="place-card-content">
        <div className="place-card-header">
          <h3 className="place-card-title">{title}</h3>
          <div className="place-card-rating">
            <Star size={14} className="star-icon" fill="currentColor" />
            <span>{(score > 0 ? (score / 10).toFixed(1) : "Mới")}</span>
          </div>
        </div>
        
        <p className="place-card-subtitle text-muted">
          {commentsCount} đánh giá
        </p>
      </div>
    </div>
  );
}
