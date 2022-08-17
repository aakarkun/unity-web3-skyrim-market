require("@nomicfoundation/hardhat-toolbox");
require("@nomiclabs/hardhat-etherscan");
require("@cronos-labs/hardhat-cronoscan");

///////////////////////////////////////////////////////////
// WALLET AND CRONOS VALUES
///////////////////////////////////////////////////////////
const PRIVATE_KEY = "";
const CRONOS_TESTNET_URL = "https://evm-t3.cronos.org/";
const CRONOSCAN_API_KEY = "";

///////////////////////////////////////////////////////////
// MODULE EXPORTS
///////////////////////////////////////////////////////////
module.exports = {
  solidity: "0.8.9",
  networks: {
    cronosTestnet: {
      url: CRONOS_TESTNET_URL,
      chainId: 338,
      accounts: [PRIVATE_KEY],
      gasPrice: 5000000000000
    }
  },
  etherscan: {
    apiKey: {
      cronosTestnet: CRONOSCAN_API_KEY,
    }
  }
};

///////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////
task("cct", "Clean, Compile, & Test the Greeter.sol").setAction(async () => {

  // Works!
  await hre.run("clean");
  await hre.run("compile");
  await hre.run("test");
});

