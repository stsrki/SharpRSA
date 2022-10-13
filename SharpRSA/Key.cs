using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace SharpRSA
{
    /// <summary>
    /// Class to contain RSA key values for public and private keys. All values readonly and protected
    /// after construction, type set on construction.
    /// </summary>
    [DataContract( Name = "Key", Namespace = "SharpRSA" )]
    [Serializable]
    public class Key
    {
        //Hidden key constants, n and e are public key variables.
        [DataMember( Name = "n" )]
        public BigInteger N { get; set; }

        [DataMember( Name = "e" )]
        public int E = Constants.e;

        //Optional null variable D.
        //This should never be shared as a DataMember, by principle this should not be passed over a network.
        public readonly BigInteger D;

        //Variable for key type.
        [DataMember( Name = "type" )]
        public KeyType Type { get; set; }

        //Constructor that sets values once, values then permanently unwriteable.
        public Key( BigInteger n, KeyType type, BigInteger d )
        {
            //Catching edge cases for invalid input.
            if ( type == KeyType.Private && d < 2 )
            {
                throw new Exception( "Constructed as private, but invalid d value provided." );
            }

            N = n;
            Type = type;
            D = d;
        }

        //Overload constructor for key with no d value.
        public Key( BigInteger n, KeyType type )
        {
            //Catching edge cases for invalid input.
            if ( type == KeyType.Private )
            {
                throw new Exception( "Constructed as private, but no d value provided." );
            }

            //Setting values.
            N = n;
            Type = type;
        }
    }
}
