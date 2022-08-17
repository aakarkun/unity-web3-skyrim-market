const { expect } = require("chai");

describe("GameContract Test", function () {
  it("Testing GameContract", async function () {
    const [owner] = await ethers.getSigners();

    // Deploying Septim
    const Septim = await ethers.getContractFactory("Septim");
    const septim = await Septim.deploy();
    await septim.deployed();

    // Deploying Outfit
    const Outfit = await ethers.getContractFactory("Outfit");
    const outfit = await Outfit.deploy();
    await outfit.deployed();

    // Deploying GameContract and passing Septim and Outfit contract addresses as constructor parameters
    const GameContract = await ethers.getContractFactory("GameContract");
    const gameContract = await GameContract.deploy(septim.address, outfit.address);
    await gameContract.deployed();

    // Here you can call contract functions and test the result
    //const addedSeptim = await gameContract.addSeptim(2);
  });
});