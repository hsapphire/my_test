

contract Variables {
    mapping(address => uint256) private _balances;  //引用类型

    uint256 private _totalSupply; //值类型

    uint num=66;
	function doSomething() public {
        num = 88;
    }
//声明一个状态变量，名为 userAddress ，类型为 address ，存储在 storage 中。
    address storage userAddress;

    //在 memory 中声明一个名为 tempString 的 string 变量，其值为 "Hello, Solidity"。
    function example() public {
        string memory tempString="Hello,Solidity";
    }
//将两个字符串连接起来，并返回 string 类型的 memory 变量，变量名是 result 。
    string a = "Hello";
    string b = "world";
    string memory result=string.concat(a,b);
    //创建一个名为 length 的 uint 变量，并将字符串转换为 bytes，以获取字符串的长度。
    string planet = "neptune";
    uint length=bytes(planet).length;
}