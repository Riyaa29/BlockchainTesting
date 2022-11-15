namespace TestBlockChain {
    public class BlockChain {
        private readonly int _powDifficulty;                    //the difficulty of mining a new block
        private readonly double _miningReward;                  //a reward, which will get to the miner who creates the new block
        private List<Transaction> _pendingTransactions;         //transactions have to be processed by the miners (by mining new valid blocks), hence a list of pending transactions
        public List<Block> Chain {                              //represents the blockchain
            get;
            set;
        }

        public BlockChain(int powDifficulty, int miningReward) {
            _powDifficulty = powDifficulty;
            _miningReward = miningReward;
            _pendingTransactions = new List<Transaction>();
            Chain = new List<Block> {                           //This creates a block with the previous hash set to zero and with empty transaction list
                CreateGenesisBlock()
            };       
        }

        public void CreateTransaction(Transaction transaction) {
            _pendingTransactions.Add(transaction);
        }

        public void MineBlock(string minerAddress) {            //used by the miners to mine new blocks
            Transaction minerRewardTransaction = new Transaction(String.Empty, minerAddress, _miningReward);        //reward for the miner 
            _pendingTransactions.Add(minerRewardTransaction);   //Adding a new transaction 
            Block block = new Block(DateTime.Now, _pendingTransactions);
            block.MineBlock(_powDifficulty);                    //calling mine method of the block, to calculate valid hash for that block 
            block.PreviousHash = Chain.Last().Hash;             //we set the valid block's previous hash to the last block’s hash, this way creating a valid chain
            Chain.Add(block);
            _pendingTransactions = new List<Transaction>();     //Clearing the pending transactions 
        }

        public bool IsValidChain() {                            //used to check the validity of the chain, to be sure that the chain was not hacked, was not tampered with        
            for (int i = 1; i < Chain.Count; i++) {
                Block previousBlock = Chain[i - 1];         
                Block currentBlock = Chain[i];
                if (currentBlock.Hash != currentBlock.CreateHash())    //if the currently calculated hash is different from the hash added to the block at creation, the block was modified
                    return false;
                if (currentBlock.PreviousHash != previousBlock.Hash)   //if  the current block’s previous hash property with the previous block’s hash property, the chain was tampered with
                    return false;
            }
            return true;
        }

        public double GetBalance(string address) {                      //to calculate the balance of a user of the blockchain using his/her address   
            double balance = 0;
            foreach (Block block in Chain) {                            //Iterating through all the blocks
                foreach (Transaction transaction in block.Transactions) {   //Iterating through all transactions
                    if (transaction.From == address) {                  //if calculating balance for sender, subtract amount from balance
                        balance -= transaction.Amount;
                    }
                    if (transaction.To == address) {                    //if calculating balance for receiver, add amount to balance
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }

        private Block CreateGenesisBlock() {                        //creates a block with the previous hash set to zero and with empty transaction list
            List<Transaction> transactions = new List<Transaction>() { new Transaction("user1", "user2", 0) };
            return new Block(DateTime.Now, transactions, "0");
        }
    }
}