1. 服务提供者配置
```yml 
spring:
  application:
    name: product-service
  cloud:
    nacos:
      discovery:
        server-addr: localhost:8848

```

2. 消费者
``` yml
spring:
  application:
    name: order-service
  cloud:
    nacos:
      discovery:
        server-addr: localhost:8848

```

3. 使用 @LoadBalanced + RestTemplate 调用服务名
``` java
@Autowired
@LoadBalanced
private RestTemplate restTemplate;
restTemplate.getForObject("http://product-service/api/products", String.class);

```
