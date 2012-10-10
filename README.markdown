<p align="center">
<img width="200px"src="http://zaypay.com/images/v2/logo.png"/>
</p>
<br>

ZAYPAY api for .NET users
------------------------------
This api is for .net users who want to use zaypay for creating and tracking their payments. 

You can download the api at link

Price Setting
------------------------------

Zaypay API works with __Price Settings__. A *price setting* determines how and in which countries payments 
can be made and by how much a consumer needs to pay in order to complete the payment. 
You can create a price setting by clicking the price setting  link on your dashboard  ( at http://zaypay.com ).

Price Settings offer following modes:

* __Super Easy__
  
  You have the facility of choosing a price you want along with price margin. For example, you can use this payment method to charge € 1,10, if you set the price at € 1, and the margin to 10% you allow us to increase the amount charged to make that payment possible. This way you can get better coverage while still controlling things as tightly as you wish. The engine will always try to get the price right primarily, but will increase to margin if needed.

* __Easy__
  
  You get more control over either countries or payment methods. This means you can tell a Price Setting "I only want to support payments by phone" or "I only accept payments from customers in Germany and Belgium". 

* __Full Control__
  
  You can control every country and within that country every payment method. You're allowed to set prices per payment method per country, which really gives you full control.

Each Price Setting is identified by an  __ID__ and __API-KEY__. *API-KEY* should only be known to you. In order to map *ID* with *API-KEY*, you will have to create __Zaypay.json__ file in *App_Data* directory of your project. It should look something like this:

``` csharp

{
  default: ID,
  ID : "API-KEY"
}
```

Where __ID__ is the *price setting ID*, and __API-KEY__ is the *price setting api-key*.
This is a sample Zaypay.json file:

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

The api comes with __Zaypay.PriceSetting__ class that will provide access to zaypay platform. We will demonstrate how easy this works through some examples.

You can create a price setting object using the price setting ID with the following code:

``` csharp

PriceSetting ps = new PriceSetting(pId: 123456);
```

The price setting class provides the option of creating a price setting object with both ID and API-KEY. That will look something like this :
 
``` csharp

PriceSetting ps = new PriceSetting(pId: 123456, pKey: "4c60870f5906a9b16507a62e96f086aa");
```

You can also create a price setting object with no arguments, in that case it will try to look for default ID in zaypay json file and will try to find the ID:KEY combination for the default ID.

``` csharp

PriceSetting ps = new PriceSetting();
```

Please note, we'll be using Price Setting object __'ps'__ in all the examples.

<br>

API Methods:
-----------------------------

Price Setting class will help you access the price settings that you have created on your Zaypay account. Using this price setting class, you can call different methods to create and track payments.

Here is a list of methods that are supported by Price Setting class.


### LocaleForIP(string ip)

You would probably want to know customers location, so that you can provide them with a preselected *country* and *language*. *LocaleForIP* method will contact zaypay to figure out the country and language of the provided *IP*.

Call to LocaleForIP will return a __LocaleForIPResponse__ object. You can call __Locale()__ method on the object to get the language-country (e.g “nl-NL”)  locale as a string.

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
  
  Gives a ( *List of Hashtables* ) of all the countries supported. Each __hashtable__ contains individual country information in key-value format: 

  __Example:__
  
  ``` csharp
  
  {
      "name" => "Netherlands",
      "code" => "NL"
  }
  ```
  
* __Languages()__

  Give a ( *List of Hashtables* ) of all the languages. Each __hashtable__ contains individual language information in key-value format:
  
  __Example:__
  
  ``` csharp
  

  {
      "code" => "en",
      "native-name" => "English",
      "english-name" => "English"
  }
  ```
If you use dynamic amounts, specify the 'amount' in ListLocales(int amount) and only the countries will be shown that can support that amount.
<br/><br/>

### ListPaymentMethods(int amount = 0)

Once your customer has selected his/her country,  we would want to list  all __payment methods__ supported by that country.

For that you have to first set the locale property of the price setting object.

``` csharp 

ps.locale = "nl-NL";
```

And then make a call to *ListPaymentMethods(int amount = 0)* that will return a __PaymentMethodResponse__ object which provides the following methods:

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

So far the customer's location and desired payment method is known. Let us set these two values on our price setting object:

``` csharp
//example

ps.locale = "nl-NL";
ps.paymentMethodID = 2; // ( 2 is the payment method for sms)
```

Please note that it is __mandatory__ to set *locale* and *paymentMethodID* before calling *CreatePayment* method.

You can provide *CreatePayment* with __options__. You might want to send some custom variables.

``` csharp

// example

NameValueCollection options = new NameValueCollection();
options.Add("purchase_id", purchase.ID.ToString());
```

Now we can make *CreatePayment* call that will return __PaymentResponse__ object containing the response hash having the following structure:


``` csharp

{
  "payment" =>
  {    	
    "id"                            =>  25504212,
    "platform"                      =>  "sms",
    "locale"                        =>  "nl-NL",
    "payload_provided"              =>  false,
    "currency"                      =>  "EUR",
    "formatted_number"              =>  "3111",
    "amount_euro"                   =>  0.8,
    "verification_needed"           =>  false,
    "status"                        =>  "prepared",
    "human_platform"                =>  "sms",
    "created_at"                    =>  "Thu May 14 15:59:15 UTC 2009",
    "customer_mo_message"           =>  "unknown",
    "keyword"                       =>  "PAY",
    "paycode"                       =>  "0284",
    "total_payout"                  =>  1.112,
    "customer_phone_number_hash"    =>  "unknown",
    "number"                        =>  "3111",
    "messages_left_to_be_sent"      =>  2,  
    "messages_to_be_sent"           =>  2,
    "total_amount"                  =>  1.221
    "payment_method_id"             =>  2,
    "partly_customer_phone_number"  =>  "unknown"
 
  },
 
  "status-string"                   =>  "Uw betaling is voorbereid",
  "short_instructions"              => 	"SMS de tekst PAY 0284 naar nummer 3111.",
  "status_string"                   => 	"Uw betaling is voorbereid",
  "very_short_instructions"         => 	"betaal per sms",
  "very_short_instructions_with_amount" => 	"betaal € 0,80 per sms",
  "long_instructions"               => 	"SMS alstublieft de tekst PAY 0284 naar telefoonnummer 3111. U zult 2 berichten ontvangen. Hiermee heeft u dan € 0,80 betaald. Verder betaalt u zelf het normale tarief voor het versturen van één SMSje."

}
```


We provide you with the following convenience methods through PaymentResponse object:
 
* __Instructions():__  Returns a hashtable containing key-value pairs of all types of instructions
     	
* __StatusString():__  Returns status string
 
* __VerificationNeeded():__  Returns a bool value regarding verification needed flag
 
* __VerificationTriesLeft():__ Returns the int value for number of tries left for verification code
 
* __Payment():__  Extracts the payment hash from the hashtable and returns it
 
* __PaymentMethodId():__  Returns the payment method id used
 
* __Platform():__  Returns the platform ( sms or phone ) that is used for the payment
 
* __SubPlatform():__  Returns the subplatform ( "pay per minute" or "pay per call" )if "phone" is used as a platform
 
* __PaymentId():__  Returns the payment id of the payment created

* __GetCustomVariables():__  Returns the NameValueCollection containing key-value pairs of custom variables that were sent along the request for creating payment
     	
* __PayalogueUrl():__  Returns the payalogue url if paylaogue id is used for creating the payment
 
* __Status():__  Returns the status of the payment created


__Example:__
``` csharp

ps.locale = "en-BE";
ps.paymentMethodID = 2;
 
NameValueCollection options = new NameValueCollection();
options.Add("purchase_id", purchase.ID.ToString());
 
PaymentResponse response = ps.CreatePayment(options);
 
// status if the payment
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

You can have access to your __payment__ through this method. However, there will be no instructions returned if the payment is not interactive anymore. For example, there are no instructions when the payment has been paid or it is expired because customer decided to not pay for it.

Call to *ShowPayment* will return __PaymentResponse__ object that has the same features described in create payment section.

__Example:__
 
``` csharp

PaymentResponse response = ps.ShowPayment(123455);
 
// get status of the payment
string status = response.Status();
```
<br/>
### VerificationCode(int paymentID, string code)

In some countries (like USA) we send your customer a __verification code__ in the final (premium) message. Payments that require verification code come with __VerificationNeeded__ flag set to TRUE.

For such payments, you have to present the customer with a form for entering the __verification code__. When you receive the code from the customer, you can make call to Zaypay through __VerificationCode(int paymentID, string code)__ method. This method returns with __PaymentResponse__ object, and if correct code was entered by the user, you will get a response object with payment status of "paid". Your customer has __3 attempts__ to enter the verification code.

__Example:__
 
``` csharp

PaymentResponse response = ps.VerificationCode(123455, "01921");
 
// you can get the status to check if the code was correctly entered by the user
string status = response.Status();
 
// you can check the tries left if code was not correct and you need to ask the customer againg to enter the code
int triesLeft = ps.VerificationTriesLeft();
```
<br/>
### MarkPayloadProvided(int paymentID)

When a payment is made, the desired product must be delivered to the customer. You can use zaypay to *register the status of product delivery*. This way you can track if a customer is misusing the system without having to keep track of all payments in your own database. You get a __"payload-provided"__ that you get in every __payment-hash__.

Calling *MarkPayloadProvided(int paymentID)* method returns the same __PaymentResponse__ object.

__Example:__
 
``` csharp

// valid only for payments that have been paid
 
PaymentResponse response = ps.MarkPayloadProvided(12355);
 
bool payaloadProvided = response.PayaloadProvided();
```