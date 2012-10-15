<p align="center">
<img width="200px"src="http://zaypay.com/images/v2/logo.png"/>
</p>
<br>

Zaypay Plugin for .NET
------------------------------
This plugin allows your .NET application to create and track payments on the Zaypay platform.
For more information about Zaypay, please visit http://www.zaypay.com and http://www.zaypay.com/developers.

Price Setting
------------------------------

The Zaypay API works with __Price Settings__. A *Price Setting* defines the price, the available payment-methods and the countries in which payments can be made. 
Once you have signed-in on www.zaypay.com, you can create a price setting by clicking the *Price Settings* link on the left of the screen.

A Price Setting can be configured with the following modes:

* __Super Easy__
  
  The Super Easy mode allows you to set a price and a price-margin. For example, if you set a price of € 1,00 and a price-margin of 10%, this will allow us to increase the amount to € 1,10 to make a payment possible in certain countries. By doing so, you can get better coverage while still controlling things as tightly as you wish. The engine will always try to get the price right primarily, but will increase to margin if needed.

* __Easy__
  
  You get more control over either countries or payment methods. This means you can tell a Price Setting "I only want to support payments by phone" or "I only accept payments from customers in Germany and Belgium". 

* __Full Control__
  This mode allows you to select the countries where you want to perform transactions and define the payment-methods and its corresponding prices for each respective country.

Each Price Setting is identified by an  __ID__ and __API-KEY__. *API-KEY* should only be known to you. In order to map *ID* with *API-KEY*, you will have to create a file named __Zaypay.json__ in the *App_Data* directory of your project. It should look something like this:

``` csharp

{
  default: ID,
  ID : "API-KEY"
}
```

Where __ID__ is the *price setting ID*, and __API-KEY__ is the *price setting api-key*.
Here is an example of a zaypay.json file:

``` csharp
{
  default: 123456,
  123456: "4c60870f5906a9b16507a62e96f086aa",
  112211: "1121110f5906a9b16507a62e96fass11"
}

```
<br>

Price Setting Object:
--------------------------------------

This plugin comes with the __Zaypay.PriceSetting__ class that provides access to the Zaypay platform. We will demonstrate how easy this works through some examples.

Once you have created a zaypay.json file under the *App_Data* directory as mentioned in the previous paragraph, you can create a price setting object by providing its price setting ID:

``` csharp

PriceSetting ps = new PriceSetting(pId: 123456);
```

You can also create a price setting object with no arguments. In this case, it will try to look for the default ID in the zaypay.json file and will try to find the ID:KEY combination for the default ID.

``` csharp

PriceSetting ps = new PriceSetting();
```

In case you have not created a zaypay.json file, you will have to create a price setting object by providing the price-setting ID AND the API-KEY
 
``` csharp

PriceSetting ps = new PriceSetting(pId: 123456, pKey: "4c60870f5906a9b16507a62e96f086aa");
```

Please note, we'll be using Price Setting object __'ps'__ in all the following examples.

<br>

API Methods:
-----------------------------

The Price Setting class provides several methods to communicate with the Zaypay platform, hence it allows you to create and track payments.

Here is a list of methods that the Price Setting class provides.


### LocaleForIP(string ip)

You would probably want to track your customers' location, so that you can provide them with a preselected *country* and *language*. The *LocaleForIP* method figures out the country and language with your visitor's *IP*.

Call to *LocaleForIP* will return a __LocaleForIPResponse__ object. You can call __Locale()__ method on the object to get the language-country (e.g “nl-NL”)  locale as a string.

__Example:__

``` csharp

LocaleForIPResponse response = ps.LocaleForIP("82.94.123.123");
string locale = response.Locale();
```  
<br/>
### ListLocales(int amount = 0)

Once you have configured out all countries that your price setting will support, you need an easy way to show all supported countries to your customer. The price setting object can list its supported locales by calling this method.

Call to *ListLocales(int amount = 0)* will return __ListLocalesResponse__ object.

__Example:__
 
``` csharp

ListLocalesResponse response = ps.ListLocales();
          
List<Hashtable> countries = response.Countries();
List<Hashtable> languages = response.Languages();
 
bool supported = response.CountrySupported("NL");
```

Below is a small description of the methods used above.


* __CountrySupported(string country)__ 
  
  Returns true if country is supported.


* __Countries()__
  
  Returns a ( *List of Hashtables* ) of all the countries supported. Each __hashtable__ contains individual country information in key-value format: 

  __Example:__
  
  ``` csharp
  
  {
      "name" => "Netherlands",
      "code" => "NL"
  }
  ```
  
* __Languages()__

  Returns a ( *List of Hashtables* ) of all the languages. Each __hashtable__ contains individual language information in key-value format:
  
  __Example:__
  
  ``` csharp
  

  {
      "code" => "en",
      "native-name" => "English",
      "english-name" => "English"
  }
  ```
If you use dynamic amounts, you can specify the 'amount' in ListLocales(int amount). This will return only the countries that support your specified amount.
<br/><br/>

### ListPaymentMethods(int amount = 0)

Once your customer has selected his/her country, you can list all __payment methods__ supported by that country.
By doing so, your customer can select his/her preferred payment method (e.g. by sms or by call)

Before calling *ListPaymentMethods()*, you have to set the locale property of the price setting object.

``` csharp 

ps.locale = "nl-NL";
```

And then a call to *ListPaymentMethods(int amount = 0)* will return a __PaymentMethodResponse__ object, which provides the following methods:

* __PaymentMethods():__ 

  Returns *List of Hashtables* with each __hashtable__ containing individual payment method information.
  
__Example:__
 
``` csharp

PaymentMethodResponse response = ps.ListPaymentMethods();
List<Hashtable> paymentMethods = response.PaymentMethods();
```

__Note:__

Each __payment method hash__ contains the following key-value pairs :

``` csharp

{
  "charged-amount"                      =>  1.112,
  "name"                                =>  "phone",
  "payment-method-id"                   =>  1,
  "very-short-instructions"             =>  "betaal per telefoon",
  "very-short-instructions-with-amount" =>  "betaal € 0,80 per telefoon",
  "eur-charged-amount"                  =>  1.221,
  "formatted-amount"                    =>  "€ 0,80",
  "payout"                              =>  0.54,
}
```

You can extract any key-value pair from the payment hashtable.

<br/>

### CreatePayment( NameValueCollection options = null )
The *CreatePayment()* methods allows you to create a payment on the Zaypay platform. But before that, it is __mandatory__ to set the locale and the payment method, as selected by your customer.

``` csharp

//example

ps.locale = "nl-NL";
ps.paymentMethodID = 2; // ( 2 is the payment method for sms)
```

You can call *CreatePayment()* with __options__ as an argument. These options allows you to set certain custom variables, which you can use for future reference when you access your payment details on Zaypay.

``` csharp

// example

NameValueCollection options = new NameValueCollection();
options.Add("purchase_id", purchase.ID.ToString());
ps.createPayment(options);
```

A *CreatePayment()* call will return a __PaymentResponse__ object with the following structure:


``` csharp

{
  "payment" =>
  {        
    "id"                            =>  25504212,
    "platform"                      =>  "sms",
    "locale"                        =>  "nl-NL",
    "payload-provided"              =>  false,
    "currency"                      =>  "EUR",
    "formatted-number"              =>  "3111",
    "amount-euro"                   =>  0.8,
    "verification-needed"           =>  false,
    "status"                        =>  "prepared",
    "human-platform"                =>  "sms",
    "created-at"                    =>  "Thu May 14 15:59:15 UTC 2009",
    "customer-mo-message"           =>  "unknown",
    "keyword"                       =>  "PAY",
    "paycode"                       =>  "0284",
    "total-payout"                  =>  1.112,
    "customer-phone-number-hash"    =>  "unknown",
    "number"                        =>  "3111",
    "messages-left-to-be-sent"      =>  2,  
    "messages-to-be-sent"           =>  2,
    "total-amount"                  =>  1.221
    "payment-method-id"             =>  2,
    "partly-customer-phone-number"  =>  "unknown"
 
  },
 
  "status-string"                       => "Uw betaling is voorbereid",
  "short-instructions"                  => "SMS de tekst PAY 0284 naar nummer 3111.",
  "very-short-instructions"             => "betaal per sms",
  "very-short-instructions-with-amount" => "betaal € 0,80 per sms",
  "long-instructions"                   => "SMS alstublieft de tekst PAY 0284 naar telefoonnummer 3111. U zult 2 berichten ontvangen. Hiermee heeft u dan € 0,80 betaald. Verder betaalt u zelf het normale tarief voor het versturen van één SMSje."

}
```


The plugin provides the following convenience methods through PaymentResponse object:
 
* __Instructions():__  Returns a hashtable containing key-value pairs of various instruction-formats
       
* __StatusString():__  Returns the status string, which informs your customers about the status of his/her payment in a more 'humanized' way. 
 
* __VerificationNeeded():__  Returns a bool regarding whether the end_user needs to enter a verification code
 
* __VerificationTriesLeft():__ Returns a int value of number of tries left for the end_user to submit the verification code
 
* __Payment():__  Returns the payment hash from the hashtable
 
* __PaymentMethodId():__  Returns the payment method id used
 
* __Platform():__  Returns the platform ( sms or phone ) that is used for the payment
 
* __SubPlatform():__  Returns the subplatform ( "pay per minute" or "pay per call" ) if "phone" is used as the platform
 
* __PaymentId():__  Returns the payment id of the payment created

* __GetCustomVariables():__  Returns the NameValueCollection containing key-value pairs of custom variables that were sent along the request for creating the payment
     	
* __PayalogueUrl():__  Returns the payalogue url if paylaogue id is used for creating the payment
 
* __Status():__  Returns the status of the payment created


__Example:__
``` csharp

ps.locale = "en-BE";
ps.paymentMethodID = 2;
 
NameValueCollection options = new NameValueCollection();
options.Add("purchase_id", purchase.ID.ToString());
 
PaymentResponse response = ps.CreatePayment(options);
 
// status of the payment
string status = response.Status();
 
// payment id of the payment
int paymentId = response.PaymentId()
 
// payment method id of the payment
int paymentMethodId = response.PaymentMethodId()
 
// get the whole payment object
Hashtable payment = response.Payment();
```
<br/>

### ShowPayment(int paymentID)

You can get access to your __payment__ through this method. 
In most cases, a *ShowPayment* call will also return the payment instructions. However, no instructions will be returned if the payment is no longer active, such as when the customer has completed the transaction or if the payment has expired because the customer has decided to not pay for it.

Call to *ShowPayment* will return a __PaymentResponse__ object that has the same features as described in create payment section.

__Example:__
 
``` csharp

PaymentResponse response = ps.ShowPayment(123455);
 
// get status of the payment
string status = response.Status();
```
<br/>
### VerificationCode(int paymentID, string code)

In some countries, due to local regulations, we send your customer a __verification code__ in the final (premium) message. Payments that require verification code come with __VerificationNeeded__ flag set to TRUE.

For such payments, you have to present the customer a form to submit the __verification code__. Once you receive the verification code from the customer, you can make a call to Zaypay through __VerificationCode(int paymentID, string code)__ method. This method returns a __PaymentResponse__ object, and if the user has entered the verification code correctly, you will get a response object with payment status "paid". Your customer has __3 attempts__ to enter the verification code.

__Example:__
 
``` csharp

PaymentResponse response = ps.VerificationCode(123455, "01921");
 
// you can get the status to check if the code was correctly entered by the user
string status = response.Status();
 
// you can check the number of tries left if the code was incorrect and you will need to ask the customer to enter the code again
int triesLeft = ps.VerificationTriesLeft();
```
<br/>
### MarkPayloadProvided(int paymentID)

The *MarkPayloadProvided()* method allows you to *register the status of product delivery* on the Zaypay platform. The idea is that once your customer has paid, you must deliver the product (be it a file or site access) that he/she has paid for. But in certain cases, you might want to grant him access only once.
In these cases, you can use the *MarkPayloadProvided()* method to track if a customer is misusing the system without having to keep track of all payments in your own database. 
The __"payload-provided"__ key comes with every __payment-hash__.

Calling *MarkPayloadProvided(int paymentID)* method returns the same __PaymentResponse__ object.

__Example:__
 
``` csharp

// valid only for payments that have been paid
 
PaymentResponse response = ps.MarkPayloadProvided(12355);
 
bool payaloadProvided = response.PayaloadProvided();
```￧￿￿￿￿ꢢﾵ瑤ﾞ￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￸���￨￳￩￱￥￵￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿���￨￴￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￿￩￬￪