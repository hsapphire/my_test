1. 添加依赖
<!-- Sentinel 核心依赖 -->
<dependency>
    <groupId>com.alibaba.cloud</groupId>
    <artifactId>spring-cloud-starter-alibaba-sentinel</artifactId>
</dependency>

2. 基本配置（application.yml）
spring:
  cloud:
    sentinel:
      transport:
        dashboard: localhost:8080  # Sentinel 控制台地址
        port: 8719                 # 控制台和客户端通信端口

启动 Sentinel 控制台：java -Dserver.port=8080 -jar sentinel-dashboard.jar

3. 注解限流（@SentinelResource）
``` java
@SentinelResource(value = "getProduct", blockHandler = "handleBlock")
@GetMapping("/product/{id}")
public String getProduct(@PathVariable String id) {
    return "商品信息：" + id;
}

public String handleBlock(String id, BlockException ex) {
    return "限流啦，稍后再试～";
}

```

