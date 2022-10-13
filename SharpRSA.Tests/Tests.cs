using System.Numerics;
using NUnit.Framework;

namespace SharpRSA.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCaseSource( typeof( Constants ), nameof( Constants.primes ) )]
        public void RabinMillerKnownPrimes( int prime )
        {
            //Testing if known primes appear as such.
            bool isPrime = Maths.RabinMillerTest( new BigInteger( prime ), 40 );

            Assert.IsTrue( isPrime, "DISCREPANCY: Known prime " + prime + " returned false from the RabinMiller test." );
        }

        /// <summary>
        /// Test the "FindPrime" method with a small bit pool in the RSA class.
        /// </summary>
        [Test]
        public void FindPrimeSmallbit()
        {
            for ( int i = 0; i < 10; i++ )
            {
                BigInteger prime = SharpRSA.Utils.FindPrime( 24 );

                Assert.True( prime != -1, "Smallbit Test " + i + ": FAIL" );
            }
        }

        /// <summary>
        /// Test the "FindPrime" method with a large bit pool in the RSA class.
        /// </summary>
        [Test]
        public void FindPrimeLargebit()
        {
            for ( int i = 0; i < 10; i++ )
            {
                BigInteger prime = SharpRSA.Utils.FindPrime( 512 );
                Assert.True( prime != -1, "Largebit Test " + i + ": FAIL" );
            }
        }

        /// <summary>
        /// Testing the BigInteger class for a reliable byte padding method.
        /// </summary>
        [Test]
        public void BigIntegerPaddingConsistency()
        {
            byte[] b = { 0x01, 0x00, 0xFF, 0x00, 0x00, 0x00 };
            byte[] nopad = { 0x01, 0x00, 0xFF };
            byte[] singlepad = { 0x01, 0x00, 0xFF, 0x00 };
            var big = new BigInteger( b ).ToByteArray();

            Assert.That( big.SequenceEqual( nopad ), Is.Not.EqualTo( big.SequenceEqual( singlepad ) ),
                "The converted and non-converted data differ as such: ORIGINAL: {0}, CONVERTED: {1}. No pattern detected.", Utils.RawByteString( b ), Utils.RawByteString( big ) );
        }
    }
}