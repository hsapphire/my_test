package com.example.springioctest;


public class MyBean {
    public String userName;
    public String userPwd;

    public String getUserName() {
        System.out.println("username");
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getUserPwd() {
        return userPwd;
    }

    public void setUserPwd(String userPwd) {
        this.userPwd = userPwd;
    }
}
