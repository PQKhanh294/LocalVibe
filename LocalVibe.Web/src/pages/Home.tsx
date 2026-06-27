import { useState, useEffect } from 'react';
import { AlertCircle, MapPinOff } from 'lucide-react';
import { postApi } from '../api/postApi';
import type { PostSummaryResponse } from '../types/post.types';
import HeroSection from '../components/HeroSection';
import PlaceCard from '../components/PlaceCard';
import './Home.css';

export default function Home() {
  const [posts, setPosts] = useState<PostSummaryResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeCategory, setActiveCategory] = useState<string | null>(null);
  const [location, setLocation] = useState<string>('');
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  // Reset page to 1 when filters change
  useEffect(() => {
    setPage(1);
  }, [activeCategory, location]);

  useEffect(() => {
    const fetchPosts = async () => {
      setLoading(true);
      setError(null);
      try {
        const response = await postApi.getPosts({ 
          tag: activeCategory || undefined,
          location: location || undefined,
          sort: 'top', 
          page: page,
          pageSize: 24 
        });
        setPosts(response.items);
        setTotalPages(response.totalPages);
      } catch (err) {
        console.error('Error fetching posts:', err);
        setError('Không thể tải dữ liệu. Vui lòng kiểm tra kết nối.');
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, [activeCategory, location, page]);

  return (
    <div className="home-page">
      <HeroSection 
        activeCategory={activeCategory} 
        onCategorySelect={setActiveCategory}
        location={location}
        onLocationSelect={setLocation}
      />
      
      <section className="trending-section container">
        <div className="section-header animate-fade-in-up">
          <h2 className="section-title">Điểm đến hàng đầu</h2>
          <p className="section-subtitle">Được cộng đồng LocalVibe bình chọn nhiều nhất tuần qua</p>
        </div>

        {loading ? (
          <div className="loading-state">
            <div className="spinner"></div>
            <p>Đang tìm kiếm Local Vibes...</p>
          </div>
        ) : error ? (
          <div className="error-state animate-fade-in-up">
            <AlertCircle size={48} className="error-icon" />
            <h3>Rất tiếc, không thể kết nối tới máy chủ</h3>
            <p>{error}</p>
            <button className="btn-retry" onClick={() => window.location.reload()}>Thử lại ngay</button>
          </div>
        ) : posts.length === 0 ? (
          <div className="empty-state animate-fade-in-up">
            <MapPinOff size={48} className="empty-icon" />
            <h3>Chưa có địa điểm nào</h3>
            <p>Hiện tại chưa có bài viết nào trong danh mục này. Hãy thử chọn danh mục khác nhé!</p>
            <button className="btn-outline" onClick={() => setActiveCategory(null)}>Xem tất cả địa điểm</button>
          </div>
        ) : (
           <div className="bento-grid">
            {posts.map((post, index) => {
              const apiBase = import.meta.env.VITE_API_BASE_URL?.replace('/api', '') || 'https://localhost:7011';
              const imgUrl = post.imageUrl?.startsWith('http') ? post.imageUrl : (post.imageUrl ? `${apiBase}${post.imageUrl}` : '');
              return (
                <div 
                  key={post.id} 
                  className={`bento-item animate-fade-in-up delay-${(index % 4) * 100 + 100}`}
                >
                  <PlaceCard
                    id={post.id}
                    title={post.title}
                    tag={post.tag || 'Khám phá'}
                    score={post.netScore}
                    commentsCount={post.commentCount}
                    imageUrl={imgUrl}
                  />
                </div>
              );
            })}
          </div>
        )}

        {!loading && !error && posts.length > 0 && totalPages > 1 && (
          <div className="pagination-container animate-fade-in-up">
            <button 
              className="btn-outline pagination-btn" 
              disabled={page === 1} 
              onClick={() => setPage(p => Math.max(1, p - 1))}
            >
              Trang trước
            </button>
            <span className="page-info">Trang {page} / {totalPages}</span>
            <button 
              className="btn-outline pagination-btn" 
              disabled={page === totalPages} 
              onClick={() => setPage(p => Math.min(totalPages, p + 1))}
            >
              Trang tiếp
            </button>
          </div>
        )}
      </section>
    </div>
  );
}
