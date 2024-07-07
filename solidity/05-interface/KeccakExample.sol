pragma solidity ^0.8.0;

contract KeccakExample {
		//接收一个字符串参数 _message，并返回一个32字节的哈希值（bytes32类型）。
		//在函数内部，我们使用keccak256函数来对输入字符串进行哈希运算，并将结果返回。
    function hash(string memory _message) public pure returns (bytes32) {
        return keccak256(bytes(_message));
    }
}