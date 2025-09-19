1. Redisson 分布式锁的用法
1.1  基础锁（可重入锁）
``` java
@Autowired
private RedissonClient redissonClient;

public void doSomething() {
    RLock lock = redissonClient.getLock("my-lock-key");

    try {
        // 加锁，默认 30s 自动过期
        lock.lock();

        // 业务逻辑
        System.out.println("正在执行业务代码...");

    } finally {
        lock.unlock();
    }
}

```

1.2 加锁带过期时间（防止死锁）
``` java
// 加锁 10 秒后自动释放
lock.lock(10, TimeUnit.SECONDS);

```

1.3 尝试加锁（tryLock）
``` java
boolean isLocked = lock.tryLock(5, 10, TimeUnit.SECONDS);
if (isLocked) {
    try {
        // 加锁成功，执行业务逻辑
    } finally {
        lock.unlock();
    }
} else {
    // 加锁失败
}

```

Redisson 的“看门狗机制”

默认加锁时间是 30s，但实际业务处理时间可能更长，Redisson 会：

自动启动一个看门狗（WatchDog）线程

每隔 10 秒续期，确保锁不会过期

看门狗默认在 lock.lock() 下生效

# 高并发秒杀
``` java
public String kill(String productId) {
    String lockKey = "lock:product:" + productId;
    RLock lock = redissonClient.getLock(lockKey);

    try {
        if (lock.tryLock(5, 10, TimeUnit.SECONDS)) {
            // 查询库存
            Integer stock = redisTemplate.opsForValue().get("stock:" + productId);
            if (stock != null && stock > 0) {
                // 扣减库存
                redisTemplate.opsForValue().decrement("stock:" + productId);
                return "秒杀成功";
            } else {
                return "库存不足";
            }
        } else {
            return "系统繁忙，请稍后重试";
        }
    } catch (Exception e) {
        return "系统异常";
    } finally {
        if (lock.isHeldByCurrentThread()) {
            lock.unlock();
        }
    }
}

```