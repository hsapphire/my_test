// SPDX-License-Identifier: MIT
pragma solidity ^0.8.17;

contract MyNFT {
    
    // 定义 Token 结构体，用于存储 NFT 信息
    struct Token {
        string name;        // NFT 名称
        string description; // NFT 描述信息
        address owner;      // NFT 所有者地址
    }
    
    // 使用 mapping 存储每个 NFT 的信息
    mapping(uint256 => Token) private tokens;
    // 使用 mapping 存储每个地址所拥有的 NFT ID 列表
    mapping(address => uint256[]) private ownerTokens;

    // 记录下一个可用的 NFT ID
    uint256 nextTokenId = 1;
    
    // 创建 NFT 函数，用于创建一个新的 NFT，并将其分配给调用者
    function mint(string memory _name, string memory _description) public returns(uint256){ 
        Token memory newNFT = Token(_name, _description, msg.sender);
        tokens[nextTokenId] = newNFT;
        ownerTokens[msg.sender].push(nextTokenId);
        nextTokenId++;
        return nextTokenId-1;
    }

    // 获取指定 NFT 的信息
    function getNFT(uint256 _tokenId) public view returns (string memory name, string memory description, address owner) {
        require(_tokenId >= 1 && _tokenId < nextTokenId, "Invalid token ID");
        Token storage token = tokens[_tokenId];
        name = token.name;
        description = token.description;
        owner = token.owner;
    }

    // 获取指定地址所拥有的所有 NFT ID
    function getTokensByOwner(address _owner) public view returns (uint256[] memory) {
        return ownerTokens[_owner];
    }

    // 转移指定 NFT 的所有权给目标地址
    function transfer(address _to, uint256 _tokenId) public {
        require(_to != address(0), "Invalid recipien");
        require(_tokenId >= 1 && _tokenId < nextTokenId, "Invalid TokenID");
        Token storage token = tokens[_tokenId];
        require(token.owner == msg.sender, "You don't own this token");
        
        // 将 NFT 的所有权转移给目标地址
        token.owner = _to;
        
        deleteById(msg.sender, _tokenId);
        ownerTokens[_to].push(_tokenId);
    }

    function deleteById(address account, uint256 _tokenId) internal {
        uint256[] storage ownerTokenList = ownerTokens[account];
        for (uint256 i = 0; i < ownerTokenList.length; i++) {
            if (ownerTokenList[i] == _tokenId) {
                // 将该 NFT ID 与数组最后一个元素互换位置，然后删除数组最后一个元素
                ownerTokenList[i] = ownerTokenList[ownerTokenList.length - 1];
                ownerTokenList.pop();
                break;
            }
        }
    }

    function burn(uint256 _tokenId) public {
        require(_tokenId >= 1 && _tokenId < nextTokenId, "Invalid TokenID");
        Token storage token = tokens[_tokenId];
        require(token.owner == msg.sender, "You don't own this token");
        
        deleteById(msg.sender, _tokenId);
        delete tokens[_tokenId];
    }

}