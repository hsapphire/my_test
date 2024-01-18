package com.example.springioctest;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

/**
 * 描述创建spring ioc是怎么创建的
 */
@SpringBootApplication
public class SpringiocTestApplication {

    public static void main(String[] args) {
//        SpringApplication.run(SpringiocTestApplication.class, args);

        ApplicationContext context = new ClassPathXmlApplicationContext("applicationContext.xml");

        // 从容器中获取 Bean
        MyBean myBean = (MyBean) context.getBean("myBean");

        // 使用 Bean
        myBean.getUserName();
    }

}
