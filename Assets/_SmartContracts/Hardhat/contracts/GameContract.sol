// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "hardhat/console.sol";
import "contracts/Septim.sol";
import "contracts/Outfit.sol";

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
// This is NOT a production-ready contract as all functions are public
// Anyone could call this functions knowing the contract address 
// You should implement some type of access control
// More info about this: https://docs.openzeppelin.com/contracts/4.x/access-control
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
contract GameContract
{
    ///////////////////////////////////////////////////////////
    // Storing addresses of Nested Contracts (Septim and Outfit)
    ///////////////////////////////////////////////////////////
    address _septimContractAddr;
    address _outfitContractAddr;

    ///////////////////////////////////////////////////////////
    // CONSTRUCTOR
    ///////////////////////////////////////////////////////////
    constructor(address septimContractAddr, address outfitContractAddr) 
    {
        _septimContractAddr = septimContractAddr;
        _outfitContractAddr = outfitContractAddr;
    }

    ///////////////////////////////////////////////////////////
    // Calling Septim contract functions
    ///////////////////////////////////////////////////////////
    function getSeptimBalance() public view returns (uint256 balance)
    {
        balance = Septim(_septimContractAddr).getBalance(msg.sender);
    }

    function addSeptim(uint256 amount) public 
    {
        Septim(_septimContractAddr).addToBalance(msg.sender, amount);
    }

    function removeSeptim(uint256 amount) public 
    {
        Septim(_septimContractAddr).removeFromBalance(msg.sender, amount);
    }

    ///////////////////////////////////////////////////////////
    // Calling Outfit contract functions
    ///////////////////////////////////////////////////////////
    function buyOutfit(uint256 tokenId, uint256 price, string memory metadataUrl) public
    {
        uint256 septimBalance = getSeptimBalance();

        // If we don't have enough Septim, we can't mint the outfit
        require(septimBalance >= price, "Insufficient Septim Balance");

        Outfit(_outfitContractAddr).buy(msg.sender, tokenId, metadataUrl);

        // We should check if outfit minting has succeed
        removeSeptim(price);
    }
}