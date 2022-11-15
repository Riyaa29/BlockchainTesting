using System.Text;  //Contains classes that represent ASCII and Unicode character encodings; abstract base classes for converting blocks of characters to and from blocks of bytes; and a 
//helper class that manipulates and formats String objects without creating intermediate instances of String.
using System.Security.Cryptography;      //Represents the abstract base class from which all implementations of the Advanced Encryption Standard (AES) must inherit.    

namespace TestBlockChain {

    public class Block {

        private readonly DateTime _timeStamp;       //time of block creation //readonly, so that the time cannot be altered even inside the class
        private long _nonce;                        //number only used once 
        public string PreviousHash {                //contains the hash of the previous block in the chain
            get;
            set;
        }
        public List<Transaction> Transactions {     //this is the data stored in the blocks    
            get;
            set;
        }     
        public string Hash {                        //hash of the block calculated based on all the properties of the block 
            get;
            private set;
        }

        public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash = "") { //Constructor of the class
            _timeStamp = timeStamp;
            _nonce = 0;
            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = CreateHash();                    //to calculate the hash of the block based on all properties of the block
        }

        public void MineBlock(int powDifficulty) {  //used by the miners to create new valid blocks
            string hashValidation = new String('0', powDifficulty);     
            while (Hash.Substring(0, powDifficulty) != hashValidation) {
                _nonce++;
                Hash = CreateHash();
            }
            Console.WriteLine("Block with HASH = {0} successfully mined!", Hash);
        }
        public string CreateHash() {                //we use SHA256 to create a hash based on all the properties of the block.
        
            using (SHA256 sha256 = SHA256.Create()) {
                string rawData = PreviousHash + _timeStamp + Transactions + _nonce;        //_timeStamp  new Random().Next(1000)
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Encoding.Default.GetString(bytes);
            }
        }
    }
}
