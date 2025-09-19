1. 限流 从 Hoxton 版本起内置了限流支持，依赖 Redis 作为令牌桶的计数器
1.1  限流核心原理：
使用 Redis + Lua 脚本 实现令牌桶限流。
每个请求根据某种 key（如 IP、用户、接口）被计算。
Redis 存储请求次数，Lua 脚本判断是否超限。
1.2 限流算法原理（RedisRateLimiter）
实现了令牌桶算法（Token Bucket）
每秒向桶里加入一定数量的令牌
请求过来，先尝试拿令牌，没有就拒绝
Redis + Lua 的组合，天然具有原子性、分布式可用性。



1. 添加依赖
<!-- Spring Cloud Gateway -->
<dependency>
    <groupId>org.springframework.cloud</groupId>
    <artifactId>spring-cloud-starter-gateway</artifactId>
</dependency>

<!-- Redis 支持 -->
<dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-data-redis-reactive</artifactId>
</dependency>

2. 配置 Redis 限流过滤器（application.yml）
``` yml
spring:
  cloud:
    gateway:
      routes:
        - id: product-service
          uri: http://localhost:8081
          predicates:
            - Path=/product/**
          filters:
            - name: RequestRateLimiter
              args:
                redis-rate-limiter.replenishRate: 5     # 每秒放入桶中的令牌数
                redis-rate-limiter.burstCapacity: 10    # 桶的最大容量
                key-resolver: "#{@ipKeyResolver}"        # 限流 key 的解析方式

```
3. 配置 KeyResolver（按 IP 限流）
``` java
@Bean
public KeyResolver ipKeyResolver() {
    return exchange -> Mono.just(
        exchange.getRequest().getRemoteAddress().getAddress().getHostAddress()
    );
}
```
4. 