pragma solidity ^0.8.17;

contract Example{
		// 定义 A 结构体，用于存储 Apple 的信息
    struct A {
        string name;        //A 名称
        string description; //A 描述信息
        address owner;      //A 所有者地址
    } 
    // 这里给出了 mint 的标准定义
// 请注意 string 等变长类型在作为参数输入时必须指定他的存储位置
// 例如这里我们只需要读取参数内容，所以使用 memory
function mint(string memory _name, string memory _description) public {}

Token memory token = Token(_name, _desc, msg.sender);

}

