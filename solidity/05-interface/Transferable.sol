pragma solidity ^0.8.0;

// 定义接口：可转账接口
interface Transferable {
    function transfer(address recipient, uint256 amount) external returns (bool);
    function getBalance() external view returns (uint256);
}

// 合约：银行账户
//继承了Transferable接口，这意味着我们合约中必须包含transfer和getBalance函数。
contract BankAccount is Transferable {
    mapping(address => uint256) private balances;
    
    constructor(uint256 amount){
        balances[msg.sender] = amount;
    }

		//实现transfer函数
    function transfer(address recipient, uint256 amount) external override returns (bool) {
        require(balances[msg.sender] >= amount, "Insufficient balance");
        balances[msg.sender] -= amount;
        balances[recipient] += amount;
        return true;
    }
		//实现getBalance函数
    function getBalance() external view override returns (uint256) {
        return balances[msg.sender];
    }
}