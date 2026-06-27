# Authentication Design Specification

**Date**: 2026-06-27  
**Topic**: Authentication (Login & Register)

## 1. Overview
Triển khai tính năng Đăng nhập và Đăng ký cho người dùng trên LocalVibe. Backend đã hỗ trợ mã hóa BCrypt và tạo JWT. Frontend sẽ quản lý trạng thái đăng nhập toàn cục và thiết kế giao diện Popup mượt mà.

## 2. Architecture & Components

### 2.1. Backend Updates
- **`AuthController`**: Bổ sung API `POST /api/auth/register` để tạo tài khoản mới.
- **`AuthService`**: Thêm logic kiểm tra trùng Username và khởi tạo User mới với BCrypt password hash.

### 2.2. Frontend Context (React)
- **`AuthContext.tsx`**: 
  - Lưu trạng thái: `user` (id, username, role) và `token`.
  - Cung cấp hàm `login(token, user)` và `logout()`.
  - Đồng bộ JWT xuống `localStorage`.

### 2.3. Frontend UI Components
- **`AuthModal.tsx`**:
  - Modal dạng Glassmorphism, hiển thị chính giữa màn hình (z-index cao).
  - Kết hợp cả 2 Form: Đăng nhập & Đăng ký.
  - Sử dụng state `isLoginView` (boolean) để chuyển đổi qua lại giữa 2 Form (có hiệu ứng trượt hoặc lật).
- **`Navbar.tsx`**:
  - Tích hợp AuthContext.
  - Trạng thái chưa đăng nhập: Nút Avatar sẽ hiển thị chữ "Đăng nhập". Bấm vào mở `AuthModal`.
  - Trạng thái đã đăng nhập: Hiển thị Avatar và Dropdown menu (Đăng xuất).

### 2.4. API Interceptor
- **`axiosClient.ts`**:
  - Interceptor tự động đọc JWT từ `localStorage` và đính kèm vào header `Authorization: Bearer <token>` của mọi API request sau này (ví dụ: Tạo bài viết, Lưu địa điểm).

## 3. Data Flow
1. Người dùng bấm "Đăng nhập" trên Navbar.
2. `AuthModal` mở lên ở chế độ Login. (Người dùng có thể chuyển sang Register).
3. Người dùng điền thông tin và Submit.
4. Axios gọi `POST /api/auth/login` (hoặc `register`).
5. Nếu API trả về 200 OK + JWT:
   - Frontend lưu JWT vào `localStorage`.
   - `AuthContext` cập nhật state `user`.
   - `AuthModal` đóng lại.
   - Giao diện Navbar cập nhật sang trạng thái đã đăng nhập.
6. Nếu lỗi (401 Unauthorized, 400 Bad Request):
   - Hiển thị thông báo lỗi ngay trên Form.

## 4. Error Handling
- Lỗi trùng lặp Username khi đăng ký: Báo lỗi "Tài khoản đã tồn tại".
- Lỗi sai mật khẩu khi đăng nhập: Báo lỗi "Sai tài khoản hoặc mật khẩu".
- Xác thực Frontend: Yêu cầu Username và Password không được để trống, Password tối thiểu 6 ký tự.

## 5. Testing
- Thử nghiệm tạo tài khoản mới thành công.
- Thử nghiệm đăng nhập thành công và JWT được lưu đúng cách.
- Thử nghiệm đăng nhập thất bại và kiểm tra hiển thị lỗi.
- Tải lại trang (F5) và xác nhận tài khoản vẫn được duy trì đăng nhập (do Token lưu ở `localStorage`).
