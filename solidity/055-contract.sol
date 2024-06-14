// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

// 合约A
contract ContractA {
    uint256 public data;
}

// 合约B
contract ContractB {
    //定义了一个ContractA的合约类型变量
    ContractA public contractA;
    ContractA public contractAA;

    constructor(address _contractA) {
        //将传入的合约地址实例化为ContractA合约，并将其赋值给contractA变量
        contractA = ContractA(_contractA);
        contractAA = new ContractA();
    }
}