flink cdc课程介绍
https://www.bilibili.com/video/BV16x4y1Y7fA/?spm_id_from=333.337.search-card.all.click&vd_source=786d1910f4436a8796d3d7c9315c60ac

# 相应的版本号
稳定生产	JDK 11	Flink 1.17.2	Flink CDC 2.4.2	Spring Boot 2.7.18	✅ 推荐	最稳定，企业级首选
兼容旧项目	JDK 8	Flink 1.13.6	Flink CDC 2.2.1	Spring Boot 2.5.14	⚠️ 维护	JDK 8 兼容
最新特性	JDK 17	Flink 1.18.1	Flink CDC 3.0.0	Spring Boot 3.2.0	🆕 最新	最新功能

# 稳定
jdk 11下载地址：
Windows x64: https://github.com/adoptium/temurin11-binaries/releases
https://adoptium.net/temurin/releases/
# Windows 多版本管理
手动设置 JAVA_HOME 环境变量切换
环境变量中只改java_home的指向就可以了。 


使用第三方工具如 JEnv 或手动批处理脚本
系统变量：JAVA_HOME 
    jdk8： D:\Program Files\Java\jdk1.8.0_172
    jdk11: D:\codeEnv\jdk11
## spring initializr中的java version和jdk version区辊，
Java Version：项目的目标语言级别，限制你能用哪些语法/特性。
JDK：你机器上装的 Java 工具链，负责实际编译和运行。
