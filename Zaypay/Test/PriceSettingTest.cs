using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using Zaypay;
using Zaypay.WebService;
using System.Collections;


namespace Zaypay.Test
{

    [TestFixture]
    class PriceSettingTest
    {

        private Hashtable expectedHash;
        private PriceSetting ps;

        [SetUp]
        protected void SetUp()
        {
            
            expectedHash = new Hashtable();
            ps = Substitute.For<PriceSetting>(111, "thekey");
            ps.PAYALOGUE_ID = 111;
            ps.LOCALE = "nl-NL";
            ps.PAYMENT_METHOD_ID = 2;


        }
        
        [Test]
        public void LocalForIP()
        {

            expectedHash["locale"] = "nl-NL";
            ps.GetResponse("https://secure.zaypay.com/82.94.123.123/pay/111/locale_for_ip?key=thekey").Returns(expectedHash);

            
            LocalForIPResponse actualResponse = new LocalForIPResponse(expectedHash);

            CollectionAssert.AreEquivalent(actualResponse.RESPONSE, expectedHash);
            //Assert.AreEqual("nl-NL", ps.LocaleForIP("82.94.123.123"));

        }

        [Test]
        [ExpectedException(typeof(Exception))]

        public void LocaleForIPWithException()
        {
            ps.LocaleForIP("");

        }

        [Test]        
        public void ListLocales()
        {

            ps.GetResponse("https://secure.zaypay.com//pay/111/list_locales?key=thekey").Returns(expectedHash);

            ListLocalesResponse actualResponse = ps.ListLocales();
            CollectionAssert.AreEquivalent(actualResponse.RESPONSE, expectedHash);

            ChangeHashtable(ref expectedHash);

            ps.GetResponse("https://secure.zaypay.com/123/pay/111/list_locales?key=thekey").Returns(expectedHash);
            actualResponse = null;
            actualResponse = ps.ListLocales(123);
            CollectionAssert.AreEquivalent(actualResponse.RESPONSE, expectedHash);

            

        }

        [Test]
        [ExpectedException(typeof(Exception))]

        public void ListLocaleWithException()
        {
            ps.ID = 0;
            ps.KEY = "";
            ps.ListLocales();
        }
        

        private void ChangeHashtable(ref Hashtable hash)
        {
            hash = null;
            hash = new Hashtable();
            hash["amount"] = "123";
        }

    }
}

