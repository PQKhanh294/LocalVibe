import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import { authApi } from '../api/authApi';
import { useAuth } from '../contexts/AuthContext';
import './AuthModal.css';

interface AuthModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export default function AuthModal({ isOpen, onClose }: AuthModalProps) {
  const { login } = useAuth();
  const [isLoginView, setIsLoginView] = useState(true);
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Reset state when modal opens
  useEffect(() => {
    if (isOpen) {
      setUsername('');
      setPassword('');
      setError(null);
      setIsLoginView(true);
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!username || !password) {
      setError('Vui lòng nhập đầy đủ thông tin');
      return;
    }

    if (!isLoginView && password.length < 6) {
      setError('Mật khẩu phải có ít nhất 6 ký tự');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      if (isLoginView) {
        const response = await authApi.login({ username, password });
        login(response.token, { username: response.username, role: response.role });
        onClose();
      } else {
        const response = await authApi.register({ username, password });
        login(response.token, { username: response.username, role: response.role });
        onClose();
      }
    } catch (err: any) {
      if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError('Có lỗi xảy ra, vui lòng thử lại sau');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-modal-overlay" onClick={onClose}>
      <div className="auth-modal-container" onClick={(e) => e.stopPropagation()}>
        <button className="auth-modal-close" onClick={onClose}>
          <X size={20} />
        </button>

        <div className="auth-modal-header">
          <h2 className="auth-modal-title">
            {isLoginView ? 'Chào mừng trở lại' : 'Tạo tài khoản'}
          </h2>
          <p className="auth-modal-subtitle">
            {isLoginView ? 'Đăng nhập để khám phá LocalVibe' : 'Tham gia cộng đồng LocalVibe ngay hôm nay'}
          </p>
        </div>

        <form className="auth-form" onSubmit={handleSubmit}>
          {error && <div className="auth-error">{error}</div>}
          
          <div className="auth-input-group">
            <label htmlFor="username">Tên đăng nhập</label>
            <input
              type="text"
              id="username"
              className="auth-input"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Nhập tên đăng nhập"
              disabled={loading}
              autoFocus
            />
          </div>

          <div className="auth-input-group">
            <label htmlFor="password">Mật khẩu</label>
            <input
              type="password"
              id="password"
              className="auth-input"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Nhập mật khẩu"
              disabled={loading}
            />
          </div>

          <button type="submit" className="auth-btn" disabled={loading}>
            {loading ? <div className="spinner-small" /> : (isLoginView ? 'Đăng nhập' : 'Đăng ký')}
          </button>
        </form>

        <div className="auth-toggle">
          {isLoginView ? (
            <>
              Chưa có tài khoản?{' '}
              <button 
                className="auth-toggle-btn" 
                onClick={() => { setIsLoginView(false); setError(null); }}
              >
                Đăng ký ngay
              </button>
            </>
          ) : (
            <>
              Đã có tài khoản?{' '}
              <button 
                className="auth-toggle-btn" 
                onClick={() => { setIsLoginView(true); setError(null); }}
              >
                Đăng nhập
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
