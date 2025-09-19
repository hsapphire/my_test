1. 引依赖
<dependency>
  <groupId>org.springframework.cloud</groupId>
  <artifactId>spring-cloud-starter-loadbalancer</artifactId>
</dependency>


2. 
@Bean
@LoadBalanced
public RestTemplate restTemplate() {
    return new RestTemplate();
}


restTemplate.getForObject("http://user-service/api/user/1", User.class);

3. Spring Cloud LoadBalancer 负载均衡策略
默认策略：轮询（RoundRobin）
@Bean
public ReactorServiceInstanceLoadBalancer loadBalancer(Environment env,
                 LoadBalancerClientFactory factory) {
    String serviceId = env.getProperty(LoadBalancerClientFactory.PROPERTY_NAME);
    return new RandomLoadBalancer(
               factory.getLazyProvider(serviceId, ServiceInstanceListSupplier.class),
               serviceId);
}


4. 与nginx区别
客户端负载均衡

    客户端从服务注册中心（如 Eureka、Consul、Nacos）获取可用服务实例列表；

    客户端根据负载均衡策略（轮询、随机、权重等）选择一个实例；

    客户端直接发起请求到选定实例；

    请求路径：客户端 -> 目标服务实例，无中间转发。

Nginx 反向代理负载均衡

    Nginx 作为统一入口，监听请求；

    根据配置的负载均衡策略，将请求转发到后端服务器池中某一实例；

    请求路径：客户端 -> Nginx -> 目标服务实例，有中间代理。

