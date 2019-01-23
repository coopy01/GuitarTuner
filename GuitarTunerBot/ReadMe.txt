Built for Bot Framework 3.x in 2018 so things will definitely be outdated.

Welcome to my voice activated Guitar Tuner project. This uses Azure, LUIS, 

and Bot Framework to enable Cortana to understand the 6 strings of the guitar 

and play the frequency.

1. Update project keys with your own. 
	a. LUIS Model
		i. Intents: The only one you really need is the "play tone" 

intent. The others were just for fun and you may or may not find the 

corresponding code to handle, but feel free to keep or delete.
	b. Bot AppId
		i. In Azure or Bot Framework when you instantiate the bot 

you'll receive keys, appid and apppassword. web.config holds these values in 

lines 11 and 12.
	c. BingSpellCheck
		i. if you want it, set to true in line 21 and update "TODO" 

in line 19 with a key you can obtain here: https://azure.microsoft.com/en-

us/try/cognitive-services/?productId=%2Fproducts%2F56ec2df6dbe2d91324586008

2. Build and update via latest Bot Framework, there may be a ton of breaking 

changes. Use the Nuget package Manager to get the latest.

3. Try with emulator or webchat via text or cortana via voice.

Note: The main dialog is RootLuisDialog.cs, AppId is configured in 

Web.config.
