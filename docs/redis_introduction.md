
# 📌 Giới thiệu Redis

**Redis** (REmote DIctionary Server) là một **cơ sở dữ liệu NoSQL lưu trữ key-value trong bộ nhớ (in-memory)**, nổi bật với tốc độ xử lý cực nhanh, khả năng mở rộng linh hoạt và hỗ trợ nhiều kiểu dữ liệu phong phú. Redis thường được sử dụng như **bộ nhớ đệm (cache)**, **hàng đợi thông điệp**, **bộ đếm thời gian thực**, hoặc **lưu trữ session** trong các hệ thống hiện đại.

---

## ⚙️ Tính năng chính

| Tính năng | Mô tả |
|----------|------|
| In-memory Storage | Dữ liệu được lưu trữ hoàn toàn trong RAM nên cực kỳ nhanh (microseconds). |
| Key-Value Store | Cơ sở dữ liệu dạng key-value đơn giản. |
| Hỗ trợ nhiều kiểu dữ liệu | String, List, Set, Sorted Set, Hash, Stream, Bitmap, HyperLogLog... |
| Pub/Sub | Hỗ trợ publish/subscribe cho hệ thống thời gian thực. |
| Persistency | Có thể lưu xuống đĩa bằng RDB hoặc AOF. |
| Replication | Hỗ trợ replication (master/slave). |
| Redis Cluster | Phân mảnh dữ liệu tự động và mở rộng quy mô. |
| Lua scripting | Hỗ trợ script Lua để thực thi logic phức tạp. |

---

## 🚀 Các Use Case Phù Hợp

| Use Case | Mô tả |
|---------|------|
| **Caching** | Tăng tốc truy xuất dữ liệu như truy vấn DB, render HTML, token... |
| **Session Store** | Lưu session cho người dùng web/app. |
| **Message Queue** | Sử dụng với các thư viện như Celery, Bull, Sidekiq để quản lý task. |
| **Real-time pub/sub** | Chat, thông báo real-time, IoT. |
| **Leaderboard/Game Stats** | Dữ liệu game cần cập nhật nhanh và xếp hạng. |
| **Rate Limiting** | Giới hạn truy cập API, ngăn brute-force login. |
| **Data Expiration** | Lưu tạm thông tin OTP, token, mã xác thực có thời hạn. |

---

## 🛠️ Cài Đặt Redis

### 1. Cài Redis trên Linux

```bash
sudo apt update
sudo apt install redis-server
```

### 2. Chạy Redis bằng Docker

```bash
docker run -d --name redis -p 6379:6379 redis
```

### 3. Kiểm tra kết nối

```bash
redis-cli ping
# Kết quả: PONG
```

---

## 🔨 Cách sử dụng Redis cơ bản

### 1. Làm việc với String

```bash
SET name "Redis"
GET name
DEL name
```

### 2. Counter

```bash
INCR views
INCRBY views 5
GET views
```

### 3. List

```bash
LPUSH queue "task1"
LPUSH queue "task2"
LRANGE queue 0 -1
RPOP queue
```

### 4. Set

```bash
SADD users "alice"
SADD users "bob"
SMEMBERS users
SISMEMBER users "alice"
```

### 5. Hash

```bash
HSET user:1001 name "Alice" age 30
HGET user:1001 name
HGETALL user:1001
```

### 6. TTL và Expiry

```bash
SET otp "123456" EX 60  # Tự động hết hạn sau 60s
TTL otp
```

---

## 🧩 Tích hợp Redis trong ứng dụng

- **Node.js:** sử dụng `ioredis` hoặc `redis` package.
- **.NET:** sử dụng `StackExchange.Redis`.
- **Python:** dùng `redis-py`.
- **Laravel/PHP:** cấu hình `CACHE_DRIVER=redis`.

Ví dụ .NET:

```csharp
var redis = ConnectionMultiplexer.Connect("localhost");
var db = redis.GetDatabase();
db.StringSet("name", "Redis");
Console.WriteLine(db.StringGet("name")); // Redis
```

---

## 📚 Tài liệu tham khảo

- [Redis.io – Trang chủ chính thức](https://redis.io)
- [Awesome Redis (GitHub)](https://github.com/antirez/awesome-redis)

---

> Redis đơn giản, nhanh chóng, và mạnh mẽ – là lựa chọn hàng đầu cho các ứng dụng yêu cầu tốc độ và hiệu năng cao.
