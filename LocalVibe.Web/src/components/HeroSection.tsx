import { useState, useRef, useEffect } from 'react';
import { Search, Coffee, MapPin, Utensils, Hotel, Mountain, ChevronDown } from 'lucide-react';
import './HeroSection.css';

interface HeroSectionProps {
  activeCategory: string | null;
  onCategorySelect: (category: string | null) => void;
  location: string;
  onLocationSelect: (location: string) => void;
}

const CATEGORIES = [
  { tag: 'ScenicRoute', label: 'Cung đường đẹp', icon: Mountain },
  { tag: 'Coffee', label: 'Quán Cà phê', icon: Coffee },
  { tag: 'Food', label: 'Ẩm thực', icon: Utensils },
  { tag: 'Hotel', label: 'Nơi lưu trú', icon: Hotel },
  { tag: 'Attraction', label: 'Điểm tham quan', icon: MapPin },
];

const LOCATION_OPTIONS = [
  { value: '', label: 'Mọi địa điểm' },
  { value: 'Hà Nội', label: 'Hà Nội' },
  { value: 'Đà Nẵng', label: 'Đà Nẵng' },
  { value: 'TP. Hồ Chí Minh', label: 'TP. Hồ Chí Minh' },
  { value: 'Hội An', label: 'Hội An' },
  { value: 'Huế', label: 'Huế' },
  { value: 'Đà Lạt', label: 'Đà Lạt' },
];

const CATEGORY_OPTIONS = [
  { value: '', label: 'Tất cả danh mục' },
  ...CATEGORIES.map(c => ({ value: c.tag, label: c.label }))
];

interface DropdownProps {
  label: string;
  value: string;
  onChange: (value: string) => void;
  options: { value: string; label: string }[];
  placeholder: string;
}

function CustomDropdown({ label, value, onChange, options, placeholder }: DropdownProps) {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const selectedOption = options.find(opt => opt.value === value);

  return (
    <div className="search-field custom-dropdown-container" ref={dropdownRef}>
      <label>{label}</label>
      <div 
        className={`custom-select-trigger ${isOpen ? 'open' : ''}`}
        onClick={() => setIsOpen(!isOpen)}
      >
        <span className={selectedOption ? 'selected-text' : 'placeholder-text'}>
          {selectedOption && selectedOption.value !== '' ? selectedOption.label : placeholder}
        </span>
        <ChevronDown size={18} className={`dropdown-icon ${isOpen ? 'rotate' : ''}`} />
      </div>
      
      {isOpen && (
        <div className="custom-options-panel animate-fade-in-up fast">
          {options.map(opt => (
            <div 
              key={opt.value} 
              className={`custom-option ${value === opt.value ? 'selected' : ''}`}
              onClick={() => {
                onChange(opt.value);
                setIsOpen(false);
              }}
            >
              {opt.label}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default function HeroSection({ activeCategory, onCategorySelect, location, onLocationSelect }: HeroSectionProps) {

  return (
    <section className="hero">
      {/* Floating Background Cards for Parallax Effect */}
      <div className="floating-cards">
        <img src="https://images.unsplash.com/photo-1583417319070-4a69db38a482?auto=format&fit=crop&q=80&w=300" className="float-img float-1 animate-float-gentle delay-100" alt="Travel 1" />
        <img src="https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&q=80&w=300" className="float-img float-2 animate-float-reverse delay-300" alt="Food 1" />
        <img src="https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&q=80&w=300" className="float-img float-3 animate-float-gentle delay-500" alt="Travel 2" />
      </div>

      <div className="hero-content container">
        <h1 className="hero-title animate-fade-in-up">
          Khám phá <span className="highlight-text font-bold">Local Vibes</span> đích thực
        </h1>
        <p className="hero-subtitle animate-fade-in-up delay-100">
          Tìm những địa điểm bí mật, những quán cà phê nhỏ và những hương vị địa phương mà chỉ người bản địa mới biết.
        </p>

        {/* Central Pill Search Bar */}
        <div className="hero-search-wrapper animate-fade-in-up delay-200">
          <div className="search-pill shadow-md">
            <CustomDropdown
              label="Địa điểm"
              placeholder="Bạn muốn đi đâu?"
              value={location}
              onChange={onLocationSelect}
              options={LOCATION_OPTIONS}
            />
            <div className="search-divider"></div>
            <CustomDropdown
              label="Danh mục"
              placeholder="Cà phê, Ăn uống..."
              value={activeCategory || ''}
              onChange={(val) => onCategorySelect(val || null)}
              options={CATEGORY_OPTIONS}
            />
            <button className="search-btn" title="Tính năng tìm kiếm sắp ra mắt">
              <Search size={20} />
            </button>
          </div>
        </div>

        {/* Category Filter Pills */}
        <div className="category-filters animate-fade-in-up delay-300">
          {CATEGORIES.map(({ tag, label, icon: Icon }) => (
            <button
              key={tag}
              className={`category-pill ${activeCategory === tag ? 'active' : ''}`}
              onClick={() => onCategorySelect(activeCategory === tag ? null : tag)}
            >
              <Icon size={18} /> {label}
            </button>
          ))}
        </div>
      </div>
    </section>
  );
}
