#!/bin/sh
DIR="$( cd "$( dirname "$0" )" && pwd )"
echo "Installing dependencies..."
npm install -g solc
dotnet tool install -g Nethereum.Generator.Console
echo "Compiling Poll.sol ..."
cd "${DIR}/Contracts/"
solcjs --optimize --bin --abi -o build Poll.sol
echo "Compiling Registry.sol ..."
solcjs --optimize --bin --abi -o build Registration.sol
echo "Fixing artifacts naming convention ..."
mv "${DIR}/Contracts/build/Poll_sol_Poll.abi" "${DIR}/Contracts/build/Poll.abi"
mv "${DIR}/Contracts/build/Poll_sol_Poll.bin" "${DIR}/Contracts/build/Poll.bin"
mv "${DIR}/Contracts/build/Registration_sol_Registration.abi" "${DIR}/Contracts/build/Registration.abi"
mv "${DIR}/Contracts/build/Registration_sol_Registration.bin" "${DIR}/Contracts/build/Registration.bin"
cd ..
echo "Generating C# library project ..."
Nethereum.Generator.Console generate from-project -a "VotingSystem.Contracts."