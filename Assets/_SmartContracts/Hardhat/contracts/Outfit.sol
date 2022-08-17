// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "hardhat/console.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
// This is NOT a production-ready contract as all functions are public
// Anyone could call this functions knowing the contract address 
// You should implement some type of access control
// More info about this: https://docs.openzeppelin.com/contracts/4.x/access-control
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
contract Outfit is ERC721URIStorage {

    constructor() ERC721("Outfit", "OTF") {}

    function buy(address origin, uint256 tokenId, string memory tokenURI) public
    {
        _mint(origin, tokenId);
        _setTokenURI(tokenId, tokenURI);

        console.log("Outfit with ID ", tokenId, " minted to: ", origin);
    }
}