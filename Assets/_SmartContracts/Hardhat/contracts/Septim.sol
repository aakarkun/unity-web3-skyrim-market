// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "hardhat/console.sol";
import "@openzeppelin/contracts/token/ERC20/ERC20.sol";

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
// This is NOT a production-ready contract as all functions are public
// Anyone could call this functions knowing the contract address 
// You should implement some type of access control
// More info about this: https://docs.openzeppelin.com/contracts/4.x/access-control
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! //
contract Septim is ERC20 
{
    address _owner;

    constructor() ERC20 ("Septim", "ST") 
    {
        _owner = msg.sender;
    }

    function getBalance(address origin) public view returns (uint256 balance)
    {
        balance = balanceOf(origin);
        console.log("Septim balance: ", balance);
    }

    function addToBalance(address origin, uint256 amount) public 
    {
        _mint(origin, amount);
        console.log(amount, " ST added to: ", origin);
    }

    function removeFromBalance(address origin, uint256 amount) public 
    {
        // We could check if amount we want to remove is bigger than balance
        _burn(origin, amount);
        console.log(amount, " ST removed from: ", origin);
    }
}