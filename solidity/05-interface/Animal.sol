pragma solidity ^0.8.0;

// 抽象合约
abstract contract Animal {
    //抽象合约可以有变量的定义
    string public name;
    bool public hasEaten;

    event EatEvent(string name);
    //也可以有构造函数
    constructor(string memory _name) {
        name = _name;
    }
    
    function speak() public virtual returns (string memory);
    
    function eat() public virtual {
        // 抽象合约可以包含实现
        // 具体的逻辑可以在子合约中重写
        hasEaten = true;
    }
    
}

// 实现抽象合约的合约
contract Cat is Animal {
    constructor(string memory _name) Animal(_name) {}
    
    function speak() public override returns (string memory) {
        return "Meow";
    }
    
    function eat() public override {
        // 重写抽象合约中的 eat 函数
        // 提供猫的具体进食逻辑
        super.eat();
        emit EatEvent(name);
        // 进行其他猫的进食操作
    }
}