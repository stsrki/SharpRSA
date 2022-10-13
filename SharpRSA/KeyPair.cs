using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace SharpRSA
{
    /// <summary>
    /// Wrapper KeyPair class, for the case when people generate keys locally.
    /// </summary>
    [DataContract]
    [Serializable]
    public sealed class KeyPair
    {
        [DataMember]
        public readonly Key PrivateKey;

        [DataMember]
        public readonly Key PublicKey;

        public KeyPair( Key privateKey, Key publicKey )
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        /// <summary>
        /// Returns a keypair based on the calculated n and d values from RSA.
        /// </summary>
        /// <param name="n">The "n" value from RSA calculations.</param>
        /// <param name="d">The "d" value from RSA calculations.</param>
        /// <returns></returns>
        public static KeyPair Generate( BigInteger n, BigInteger d )
        {
            Key publicKey = new Key( n, KeyType.Public );
            Key privateKey = new Key( n, KeyType.Private, d );

            return new KeyPair( privateKey, publicKey );
        }
    }
}
