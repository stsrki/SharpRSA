using System;
using System.Numerics;

namespace SharpRSA
{
    internal class Utils
    {
        //Finds a prime of the given bit length, to be used as n and p in RSA key calculations.
        public static BigInteger FindPrime( int bitlength )
        {
            //Generating a random number of bit length.
            if ( bitlength % 8 != 0 )
            {
                throw new Exception( "Invalid bit length for key given, cannot generate primes." );
            }

            //Filling bytes with pseudorandom.
            byte[] randomBytes = new byte[( bitlength / 8 ) + 1];
            Maths.rand.NextBytes( randomBytes );
            //Making the extra byte 0x0 so the BigInts are unsigned (little endian).
            randomBytes[randomBytes.Length - 1] = 0x0;

            //Setting the bottom bit and top two bits of the number.
            //This ensures the number is odd, and ensures the high bit of N is set when generating keys.
            SetBitInByte( 0, ref randomBytes[0] );
            SetBitInByte( 7, ref randomBytes[randomBytes.Length - 2] );
            SetBitInByte( 6, ref randomBytes[randomBytes.Length - 2] );

            while ( true )
            {
                //Performing a Rabin-Miller primality test.
                bool isPrime = Maths.RabinMillerTest( randomBytes, 40 );
                if ( isPrime )
                {
                    break;
                }
                else
                {
                    IncrementByteArrayLE( ref randomBytes, 2 );
                    var upper_limit = new byte[randomBytes.Length];

                    //Clearing upper bit for unsigned, creating upper and lower bounds.
                    upper_limit[randomBytes.Length - 1] = 0x0;
                    BigInteger upper_limit_bi = new BigInteger( upper_limit );
                    BigInteger lower_limit = upper_limit_bi - 20;
                    BigInteger current = new BigInteger( randomBytes );

                    if ( lower_limit < current && current < upper_limit_bi )
                    {
                        //Failed to find a prime, returning -1.
                        //Reached limit with no solutions.
                        return new BigInteger( -1 );
                    }
                }
            }

            //Returning working BigInt.
            return new BigInteger( randomBytes );
        }

        /// <summary>
        /// Sets a bit in a given ref byte, using an index from 0-7 from the right.
        /// </summary>
        /// <param name="bitNumFromRight">The index of the bit number from the lesser side of the byte.</param>
        /// <param name="toSet">The referenced byte to set.</param>
        public static void SetBitInByte( int bitNumFromRight, ref byte toSet )
        {
            byte mask = (byte)( 1 << bitNumFromRight );
            toSet |= mask;
        }

        /// <summary>
        /// Increments the byte array as a whole, by a given amount. Assumes little endian.
        /// Assumes unsigned randomBytes.
        /// </summary>
        public static void IncrementByteArrayLE( ref byte[] randomBytes, int amt )
        {
            BigInteger n = new BigInteger( randomBytes );
            n += amt;
            randomBytes = n.ToByteArray();
        }

        /// <summary>
        /// Decrements the byte array as a whole, by a given amount. Assumes little endian.
        /// Assumes unsigned randomBytes.
        /// </summary>
        public static void DecrementByteArrayLE( ref byte[] randomBytes, int amt )
        {
            BigInteger n = new BigInteger( randomBytes );
            n -= amt;
            randomBytes = n.ToByteArray();
        }
    }
}
