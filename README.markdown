<p align="center">
<img width="200px"src="http://zaypay.com/images/v2/logo.png"/>
</p>
<br>

ZAYPAY api for .NET users
------------------------------
This api is for .net users who want to interact with zaypay for creating and tracking their payments. 

You can download the api at link

Price Setting
------------------------------

Zaypay API works with __Price Settings__. A *price setting* determines how and in what countries payments 
can be made and how much a consumer needs to pay to complete the payment. 
You can create a price setting by clicking the price setting  link on your dashboard  ( at http://zaypay.com ).

Price Settings offer following modes:

*   Super Easy
*   Easy
*   Full Control

Super Easy and Easy modes allow you to set a price to aim at but this price can be overidden dynamically through the API (and as such, through this plugin). In Full Controll, everything can be configured.


Each Price Setting is identified by an  __ID__ and __API-KEY__. *API-KEY* should only be known to you. In order to map ID with API-KEY, you will have to create __Zaypay.json__ file in *App_Data* directory of your project. It should look something like this:

``` csharp

{
  default: ID,
  ID : "API-KEY"
}
```

This is a sample Zaypay.json file:

``` ruby
{
  default: 123456,
  123456: "4c60870f5906a9b16507a62e96f086aa",
  112211: "1121110f5906a9b16507a62e96fass11"
}

```

