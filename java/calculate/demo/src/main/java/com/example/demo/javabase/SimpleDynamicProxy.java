//package com.example.demo.javabase;
//
//import java.lang.reflect.Proxy;
//
//import java.lang.reflect.*;
//
//
//class SimpleDynamicProxy {
//    public static void consumer(Interface iface) {
//        iface.doSomething();
//        iface.somethingElse("bonobo");
//    }
//
//    public static void main(String[] args) {
//        RealObject real = new RealObject();
//        consumer(real);
//        // Insert a proxy and call again:
//        Interface proxy = (Interface) Proxy.newProxyInstance(
//                Interface.class.getClassLoader(),
//                new Class[]{Interface.class},
//                new DynamicProxyHandler(real));
//        consumer(proxy);
//    }
//} /* Output: (95% match)
//doSomething
//somethingElse bonobo
//**** proxy: class $Proxy0, method: public abstract void Interface.doSomething(), args: null
//doSomething
//**** proxy: class $Proxy0, method: public abstract void Interface.somethingElse(java.lang.String),
//args: [Ljava.lang.Object;@42e816
//    bonobo
//somethingElse bonobo
//*///:~
