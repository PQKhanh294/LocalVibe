# Build React App bằng Node.js
FROM node:20-alpine as build
WORKDIR /app

# Khai báo biến môi trường (Lát nữa sẽ trỏ vào IP của EC2)
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL

COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

# Dùng Nginx để chạy giao diện
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]