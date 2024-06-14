// enum State { Waiting, Ready, Active }
// State s = State.Active;

pragma solidity ^0.8.0;

contract Example {
  enum City {
    BeiJing,
    HangZhou,
    ChengDu
  }

  City public selectedCity;

  constructor() {
    selectedCity = City.BeiJing;
  }
  
  //赋值，其实是通过数字传递的
  function changeCity1(City _newCity) public {
    selectedCity = _newCity;
  }

  //显式类型转换
  function changeCity2(uint8 _newCity) public {
    selectedCity = City(_newCity);
  }

  //我们使用type(枚举名).max的语法获取到了Color这个枚举的最大值。
City a = type(City).max;
City b = type(City).min;
}