# LoggingOptions

## Mục đích
Cấu hình động cho Serilog thông qua `appsettings.json`

## Cấu trúc
```json
{
  "LoggingOptions": {
    "ServiceName": "MyService",
    "ElasticsearchUrl": "http://localhost:9200",
    "SeqUrl": "http://localhost:5341"
  }
}
```

> ⚠️ Hiện tại chưa được sử dụng trong `UseSharedSerilog`, nhưng có thể dùng để mở rộng.