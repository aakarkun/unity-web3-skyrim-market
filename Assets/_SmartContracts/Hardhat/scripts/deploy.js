// We require the Hardhat Runtime Environment explicitly here. This is optional
// but useful for running the script in a standalone fashion through `node <script>`.
//
// You can also run a script with `npx hardhat run <script>`. If you do that, Hardhat
// will compile your contracts, add the Hardhat Runtime Environment's members to the
// global scope, and execute the script.
const hre = require("hardhat");
const fs = require('fs'); // Required to get ABI

async function main() {

  ///////////////////////////////////////////////////////////
  // DEPLOYMENT
  ///////////////////////////////////////////////////////////

  // Deploying Septim contract
  const Septim = await hre.ethers.getContractFactory("Septim");
  const septim = await Septim.deploy();
  await septim.deployed();

  console.log("\n");
  console.log("Septim Address: ", septim.address);
  console.log("\n");


  // Deploying Outfit contract
  const Outfit = await hre.ethers.getContractFactory("Outfit");
  const outfit = await Outfit.deploy();
  await outfit.deployed();

  console.log("Outfit Address: ", outfit.address);
  console.log("\n");


  // Deploying GameContract and passing Septim and Outfit contract addresses as constructor parameters
  const GameContract = await hre.ethers.getContractFactory("GameContract");
  const gameContract = await GameContract.deploy(septim.address, outfit.address);
  await gameContract.deployed();

  console.log("GameContract Address: ", gameContract.address);
  console.log("\n");


  // Get GameContract ABI
  const abiFile = JSON.parse(fs.readFileSync('./artifacts/contracts/GameContract.sol/GameContract.json', 'utf8'));
  const abi = JSON.stringify(abiFile.abi);

  console.log("GameContract ABI:");
  console.log("\n");
  console.log(abi);
  console.log("\n");


  ///////////////////////////////////////////////////////////
  // WAITING
  ///////////////////////////////////////////////////////////
  await gameContract.deployTransaction.wait(7);

  ///////////////////////////////////////////////////////////
  // VERIFICATION - We verify the contract on Cronoscan - More info here: https://www.npmjs.com/package/@cronos-labs/hardhat-cronoscan
  ///////////////////////////////////////////////////////////
  await hre.run("verify:verify", {
      address: gameContract.address,
      constructorArguments: [
        septim.address,
        outfit.address
      ],
  });
}

// We recommend this pattern to be able to use async/await everywhere
// and properly handle errors.
main().catch((error) => {
  console.error(error);
  process.exitCode = 1;
});
