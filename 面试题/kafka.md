Kafka 是一个分布式系统，由服务器和客户端 组成，它们通过高性能TCP 网络协议进行通信
Kafka 由一台或多台服务器组成集群运行，这些服务器可以跨越多个数据中心或云区域。其中一些服务器构成存储层，称为代理 (broker)
其他服务器运行 Kafka Connect，以事件流的形式持续导入和导出数据，从而将 Kafka 与您现有的系统（例如关系数据库）以及其他 Kafka 集群集成

## 如何防止重复消费
1.关闭自动提交  props.put("enable.auto.commit", "false");
2.处理成功以后 再提交 consumer.commitSync(); // 或 commitAsync()
 消息处理幂等化（推荐）
 数据写数据库	建立主键约束，使用 UPSERT 或 INSERT IGNORE
 订单系统	消息中携带业务唯一标识 ID，幂等判断
 Redis 缓存	用 SETNX + 业务ID 过滤

 使用业务唯一 ID + 去重表
 SELECT COUNT(*) FROM consume_log WHERE msg_id = 'abc123';
 Kafka 幂等 Producer（防止重复发送）
 props.put("enable.idempotence", "true");
保证同一条消息只写入一次 partition
Kafka 从 0.11 起支持事务写入 + 幂等组合，配合 Sink 端实现精准一次：
producer.initTransactions();
producer.beginTransaction();
// send records
producer.commitTransaction();
 最推荐方案组合（实际可落地）
手动提交 offset；
消费逻辑实现幂等性（用 Redis/DB 去重）；
数据最终写入采用 UPSERT / INSERT IGNORE；
Kafka 设置合理的重试机制 + 死信队列（DLQ）；
若高一致性要求，用事务 Producer + 幂等消费；