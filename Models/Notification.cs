using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWallet.Models
{
	public class GoogleId
	{
		public int Id { get; set; } //required for BSON storage
        public string googleId { get; set; }
        public string publicKey { get; set; }
        
	}

    public class TxHash
    {
    	public int Id { get; set; } //required for BSON storage
        public string hash { get; set; }
        public string amount { get; set; }
        public string publicKey { get; set; }
        
    }

    public class FirebaseMessage
    {
        public string multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public FirebaseResult[] results { get; set; }
    }

    public class FirebaseResult
    {
        public string message_id { get; set; }
    }
}
