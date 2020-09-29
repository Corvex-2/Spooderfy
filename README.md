# Spooderfy
easy transparent song overlays for OBS and other recording/streaming software without the requirement to use chroma keying or other filters.

# Setup & Usage

Setup:

Open https://developer.spotify.com/dashboard/ and log in.
Click "Create An App" and create an App. the name and the description doesn't matter.
Click "Edit Settings" and type the following into the field "Redirect URIs" and Click Save
 - https://localhost:8888/

Note: you can use any port you want, as long as it isn't used.

Now we go to Spooderfy and enter the Redirect URI from earlier.
Now you enter the Client ID that is shown in the Spotify Dashboard into Spooderfy.
Now you click on "Show Client Secret" in the Spotify Dashboard and enter it into Spooderfy.
Now you click on "Open" in Spooderfy, your browser should now open an authorization page should now appear, you have to accept this in order to use Spooderfy.

Usage:

Now that the setup is complete you can begin to use Spooderfy. all you now have to do, is to check the "Show Permanent Toast" checkbox. Spooderfy now creates a PNG-Image inside the DATA Directory, called StreamImage.png, add this as an Image Source inside your streaming application and profit!

# MAINTENANCE DISCLAIMER

The source code and the project are by far not high quality, it's relatively old and no maintenance work is being done to it. feel free to report features you feel are missing or issues you encounter in the issues section, though it is very unlikely that i'll add features requested or remove any bugs.

# DEPENDENCIES
- EmbedIO, version="2.9.2" targetFramework=net472
- Newtonsoft.Json version="12.0.3" targetFramework=net472
- SpotifyAPI.Web version="4.2.2" targetFramework=net472
- SpotifyAPI.Web.Auth version="4.2.2" targetFramework=net472
- Unosquare.Swan.Lite version="1.3.1" targetFramework=net472

# COPYRIGHT
This source code is published and licensed under the GPLv3 licensing agreement.
The SUK IT school group reserves itself the right to change the licensing agreement at anytime, and without further notice.
More information about the GPLv3 licensing agreement can be found here: https://github.com/Kayory/Spooderfy/blob/master/GPLv3.license
