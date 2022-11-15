namespace TestBlockChain {
    public class Transaction {      //list of transactions must be a private variable  //public, cause this way we can demontrate change detection and chain validity/invalidity
        public string From {        //identifies the sender of the money
            get;
        }     
        public string To {          //identifies the receiver of the money
            get;
        }       
        public double Amount {      //represents the amount of money sent 
            get;
        }         
        public Transaction(string from, string to, double amount) {
            From = from;        
            To = to;
            Amount = amount;
        }
    }
}