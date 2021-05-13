package main

import (
	"time"
	"fmt"
	"math/rand"
	"strconv"
	"crypto/sha1"
)

// history/balance to be replaced later
type address struct {
	name string
	balance float64
	history string
}


type block struct {
	timestamp time.Time
	data string // format is sender/reciever/data/ data stream ends at lf
	difficulty int
	extData string;
}

func newTransaction(sender address, reciever address, amount float64) string {
	var data string;

	data = "SEND" + strconv.FormatFloat(amount, 'f', -1, 64) + "\n";

	return sender.name + reciever.name + data;
}

func mineBlock(blockChainMap map[string]block, blockNameToVerify string) string {
	currentDifficulty := 10
	var blockName string

	for blockName != blockNameToVerify {

		hashString := string(currentDifficulty) + blockChainMap["bufferblock"].data
		hash := sha1.New()
		hash.Write([]byte(hashString))
		blockName = string(hash.Sum(nil))

		currentDifficulty++
	}
	return blockName
}

func newBlock(blockChainMap map[string]block, data string, extData string) bool {
	blockChain["bufferBlock"] = block {
		timestamp: time.Now(),
		data: "0x0",
		difficulty: rand.Intn(99-10) + 10,
		extData: "template block for creating new blocks"}

	fmt.Println("Difficulty:", blockChainMap["bufferBlock"].difficulty)
	
	hashString := string(11) + blockChainMap["bufferblock"].data
	hash := sha1.New()
	hash.Write([]byte(hashString))
	blockName := string(hash.Sum(nil))
	
	if mineBlock(blockChainMap, blockName) == blockName {

		blockChainMap[blockName] = block {

			timestamp: blockChainMap["bufferblock"].timestamp,
			data: data,
			difficulty: blockChainMap["bufferblock"].difficulty,
			extData: ""}

		return true;
	} else {
		return false;
	}
}

var blockChain = make(map[string]block)

func main() {
	var wallets = make(map[string]address)
	wallets["0"] = address {
		name: "0",
		balance: 1000,
		history: "0",
	}

	wallets["1"] = address {
		name: "0",
		balance: 500.5,
		history: "0",
	}

	blockChain["bufferBlock"] = block {
		timestamp: time.Now(),
		data: "0x0",
		difficulty: rand.Intn(99-10) + 10,
		extData: "template block for creating new blocks"}

	fmt.Println(wallets["0"].balance)

	fmt.Println(newTransaction(wallets["0"], wallets["1"], 10))

}
