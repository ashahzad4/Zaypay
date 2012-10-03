<p align="center">
<img width="200px"src="http://zaypay.com/images/v2/logo.png"/>
</p>
<br>

ZAYPAY api for .NET users
------------------------------

Price Setting
------------------------------

Zaypay API works with so-called Price Settings. There are items you can create by clicking Price Settings link on your dashboard  ( at http://zaypay.com ), that defines what payment methods and countries you want to support, and what prices you would allow. 

Price Settings offer following modes:

*   Super Easy
*   Easy
*   Full Control

Super Easy and Easy modes allow you to set a price to aim at but this price can be overidden dynamically through the API (and as such, through this plugin). In Full Controll, everything can be configured.


Each Price Setting has ID and API-KEY. API-KEY should only be known to you. You will have to create Zaypay.json file in App_Data directory of your project. It should look something like this:

``` ruby

{
  default: ID,
	ID : "API-KEY"
}
```


